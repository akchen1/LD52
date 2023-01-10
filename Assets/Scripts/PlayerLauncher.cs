using UnityEngine;
using System.Collections;
using System.Linq;
public class PlayerLauncher : MonoBehaviour
{
	public enum PlayerState { InPlatform, InAir, Landing, LandingTransition, Dead, Respawning }
	// The Rigidbody2D component of the Player object
	public Rigidbody2D PlayerRigidbody;
	//The strength the player is launched

	//Controller for AimController
	private AimController AC;
	private Collider2D coll;
	private Animator animator;
	private float radius;
	private Vector2 moveDir;

	// Set to the current platform the player is on
	[SerializeField] private Platform currentPlatform;

	// State of the player
	public PlayerState state;

	// Used for rope platform. keep track of original Rigidbody type
	private RigidbodyType2D currentPlatformOriginalRBType;

	// Expected position where the player will end up after jumping
	private Vector3 expectedPosition;
	private Vector2 launchDirection;
	private Vector3 maxLaunchPosition;
	private Vector3 startLaunchPosition;
	private Platform expectedPlatform;

	[SerializeField] private GameObject child;
	float playerRadius;
	private void Start()
	{
		AC = this.gameObject.GetComponent<AimController>();
		radius = AC.GetRadius();
		coll = GetComponent<Collider2D>();
		animator = GetComponentInChildren<Animator>();
		state = PlayerState.InPlatform;
		playerRadius = GetComponent<CircleCollider2D>().radius;
	}

	private void Update()
	{
		if (state == PlayerState.Dead) return;
		BeginJump();

		Travel();

		Move();

		SetAnimation();

		RotateChild();
	}

	private void RotateChild()
    {
        if (state != PlayerState.InPlatform) return;
        Vector2 normal = currentPlatform.GetClosestEdge(transform.position);

        child.transform.rotation = Quaternion.FromToRotation(Vector3.down, normal);

		if (moveDir.magnitude == 0) return;

		Vector3 childScale = child.transform.localScale;
        if (normal.y <= 0) // on top
        {
			childScale.x = moveDir.x < 0 ? -1 : 1;
			child.transform.localScale = childScale;

		} else if (normal.y > 0)
        {
			if (moveDir.y == 0) // im not pressing up or down
			{
				childScale.x = moveDir.x < 0 ? 1 : -1;
			}
			else // im not presssing left or right
			{
				if (normal.x < 0)
					childScale.x = moveDir.y < 0 ? 1 : -1;
				else
					childScale.x = moveDir.y < 0 ? -1 : 1;
			}

			child.transform.localScale = childScale;
		}
    }

	private void SetAnimation()
	{
		animator.SetBool("isDead", state == PlayerState.Dead);
		animator.SetBool("inPlatform", state == PlayerState.InPlatform);
		animator.SetBool("isLanding", state == PlayerState.Landing);
		animator.SetBool("isTransition", state == PlayerState.LandingTransition);
		animator.SetBool("isRespawning", state == PlayerState.Respawning);
	}

	private void Move()
	{
		if (state != PlayerState.InPlatform) return;
		Vector2 move = transform.position;
		moveDir = Vector2.zero;
		if (Input.GetKey(KeyCode.A))
		{
			moveDir.x -= .1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			moveDir.x += .1f;
		}
		if (Input.GetKey(KeyCode.W))
		{
			moveDir.y += .1f;

		}
		if (Input.GetKey(KeyCode.S))
		{
			moveDir.y -= .1f;
		}

		move += moveDir;
		PlayerRigidbody.MovePosition(move);
		animator.SetFloat("Horizontal", move.x);
		animator.SetFloat("Vertical", move.y);
		animator.SetBool("isMoving", moveDir.magnitude >= 0.1f);
	}

	private void Travel()
	{
		if (state != PlayerState.InAir) return;

		CalculateExpectedPosition(startLaunchPosition, (maxLaunchPosition - startLaunchPosition).normalized, (maxLaunchPosition - startLaunchPosition).magnitude);
		// Check if arrived at expected position
		Vector2 direction = (expectedPosition - transform.position).normalized;
		float distance = Vector2.Distance(expectedPosition, transform.position);

		if (distance >= 0.1f)	// Did not arrive at expected position
        {
			PlayerRigidbody.velocity = direction.normalized * 10f;

			return;
        } 

		PlayerRigidbody.velocity = Vector2.zero;

		if (expectedPlatform == null)
		{
			DieHard();
		}
		else
		{
			currentPlatform = expectedPlatform;
			state = PlayerState.Landing;
			coll.isTrigger = false;
			StartCoroutine(Land());
		}
	}


	public void DieHard()
	{
		child.transform.rotation = Quaternion.identity;
		state = PlayerState.Dead;
		PlayerRigidbody.velocity = Vector3.zero;
		PlayerRigidbody.angularVelocity = 0;

		StartCoroutine(PlayerRespawn());
	}

	private IEnumerator Land()
	{
		if (currentPlatform.GetComponent<RopePlatform>() != null)
        {
			//Vector2 direction = transform.position - ;
            ApplyForce(currentPlatform, launchDirection.normalized);

        }
		// wait for land animation 3 frames / 8 fps + transition animation 3 frames / 8 fps
		yield return new WaitForSeconds(3f / 8f - 0.1f);
		state = PlayerState.LandingTransition;
		yield return new WaitForSeconds(3f / 8f);

		state = PlayerState.InPlatform;
	}

	private void BeginJump()
	{
		if (state != PlayerState.InPlatform) return;
		if (!(Input.GetMouseButtonUp(0))) return;


		Vector3 worldPos = GetMouseWorldPosition();

		// Calculate the direction and distance from the Player to the mouse click
		Vector2 mouseDirection = worldPos - transform.position;

		// Normalize the direction and scale it by the launch force
		mouseDirection = mouseDirection.normalized;


		GameObject nextPlatform = CalculateExpectedPosition(transform.position, mouseDirection, radius);
		if ((nextPlatform != null && nextPlatform.tag == "InnerWall"))
		{
			return;
		}

		maxLaunchPosition = transform.position + (Vector3)mouseDirection * radius;
		launchDirection = mouseDirection.normalized;
		// Set player to trigger, Ignore all collision
		coll.isTrigger = true;
		SetRigidBodyType(currentPlatform, currentPlatformOriginalRBType);

		Platform platform = nextPlatform?.GetComponent<Platform>();
		//SetRigidBodyType(platform, RigidbodyType2D.Static);

		expectedPlatform = platform;

        startLaunchPosition = transform.position;
        state = PlayerState.InAir;

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, mouseDirection);
		child.transform.rotation = rot;
	}
	private Vector3 GetMouseWorldPosition()
	{
		// Get the position of the mouse click
		Vector3 mousePos = Input.mousePosition;

		// Convert the mouse position to world space
		return Camera.main.ScreenToWorldPoint(mousePos);
	}

	private void ApplyForce(Platform platform, Vector2 direction)
    {
		if (platform == null) return;
		Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
		if (rb == null) return;
        rb.AddForce(direction * 50, ForceMode2D.Impulse);
    }

	private void SetRigidBodyType(Platform platform, RigidbodyType2D type)
	{
		if (platform == null) return;
		Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
		if (rb == null) return;
		currentPlatformOriginalRBType = rb.bodyType;
		rb.bodyType = type;
	}

	private RaycastHit2D[] CalculateNextPlatform(Vector2 startPosition, Vector2 direction, float distance)
	{
		// Calculated expected position
		
		Vector2 offset = direction * playerRadius * transform.localScale;
		RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition + offset, direction, distance - playerRadius, 1 << 3);
		hits = hits.Where(x => x.collider.gameObject != currentPlatform?.gameObject).OrderBy(x => Vector3.Distance(startPosition, x.point)).ToArray();

		if (hits.Length > 0)    // We hit a wall that isn't the one we are already on
		{
			return hits;
		}
		
		return null;
	}

	private GameObject CalculateExpectedPosition(Vector2 startPosition, Vector2 direction, float distance)
    {
		RaycastHit2D[] hits = CalculateNextPlatform(startPosition, direction, distance);
		if (hits != null)
        {
			expectedPosition = hits[0].point + direction * 0.1f;
			return hits[0].collider.gameObject;
		}
		expectedPosition = startPosition + direction * distance;
		return null;
	}

	private IEnumerator PlayerRespawn()
	{
		yield return new WaitForSeconds(1.167f);
		//Death Function
		GameObject spawnGO = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>().GetCurrentSpawn();
		transform.position = spawnGO.transform.position;
		state = PlayerState.Respawning;

		yield return new WaitForSeconds(0.833f);
		currentPlatform = spawnGO.GetComponent<Respawn>().SpawnPlatform;
		gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
		state = PlayerState.InPlatform;
	}
}

