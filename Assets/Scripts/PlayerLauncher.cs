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
	private Vector2 LaunchDirection;
	private Vector3 maxPosition;
	Platform expectedPlatform;
	[SerializeField] private GameObject child;

	private void Start()
	{
		AC = this.gameObject.GetComponent<AimController>();
		radius = AC.GetRadius();
		coll = GetComponent<Collider2D>();
		animator = GetComponentInChildren<Animator>();
		state = PlayerState.InPlatform;
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
        float angle = Vector3.Angle(normal, Vector2.up);

        Vector3 temp = Vector3.Cross(normal, Vector3.down);
        Vector3 groundSlopeDirection = Vector3.Cross(temp, normal);
        float groundSlopeAngle = Vector3.Angle(normal, Vector3.up);

        Quaternion rot = Quaternion.FromToRotation(Vector3.down, normal);
        child.transform.rotation = rot;

		//Debug.Log(normal);
        if (normal.y < 0) // on top
        {
            if (moveDir.magnitude != 0)
            {
                Vector3 childScale = child.transform.localScale;
                childScale.x = moveDir.x < 0 ? -1 : 1;
                child.transform.localScale = childScale;

            }

        } else if (normal.y > 0)
        {
            if (moveDir.magnitude != 0)
            {
				//Debug.Log(moveDir);
                Vector3 childScale = child.transform.localScale;
                if (moveDir.y == 0) // im not pressing up or down
                {
                    childScale.x = moveDir.x < 0 ? 1 : -1;

                } else if (moveDir.x == 0)// im not presssing left or right
                {
					if (normal.x < 0)
						childScale.x = moveDir.y < 0 ? 1 : -1;
					else
						childScale.x = moveDir.y < 0 ? -1 : 1;
				}
				else// I am pressing both
                {
                    childScale.x = moveDir.x < 0 ? 1 : -1;
                }

                child.transform.localScale = childScale;

            }
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
		Vector2 direction = maxPosition - transform.position;
		float distance = Vector2.Distance(maxPosition, transform.position);

		if (distance >= 0.1f)   // Did not arrive yet, keep moving to max possible distance
		{
			PlayerRigidbody.velocity = direction.normalized * 10f;
		}

		// Check if arrived at expected position
		direction = expectedPosition - transform.position;
		distance = Vector2.Distance(expectedPosition, transform.position);
		if (distance >= 0.1f)	// Did not arrive at expected position
        {
			return;
        }

		PlayerRigidbody.velocity = Vector2.zero;

		if (expectedPlatform == null)
		{
			//Death Animation
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
            ApplyForce(currentPlatform, LaunchDirection.normalized);

        }
		// wait for land animation 3 frames / 8 fps + transition animation 3 frames / 8 fps
		yield return new WaitForSeconds(3f / 8f);
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
		float distance = mouseDirection.magnitude;

		// Normalize the direction and scale it by the launch force
		mouseDirection = mouseDirection.normalized;

		LaunchDirection = mouseDirection.normalized;
		GameObject nextPlatform = CalculateExpectedPosition(mouseDirection, radius);
		maxPosition = transform.position + (Vector3)mouseDirection * radius;
		if (nextPlatform != null && nextPlatform.tag == "InnerWall")
		{
			return;
		}
		RopePlatform rP = nextPlatform?.GetComponent<RopePlatform>();
		if (rP != null)
        {
			rP.SetLandingPoint(expectedPosition);
        }

		// Set player to trigger, Ignore all collision
		coll.isTrigger = true;
		SetRigidBodyType(currentPlatform, currentPlatformOriginalRBType);

		Platform platform = nextPlatform?.GetComponent<Platform>();
		//SetRigidBodyType(platform, RigidbodyType2D.Static);

		expectedPlatform = platform;
		currentPlatform = null;
		
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
		Debug.Log(platform?.name);
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

	private RaycastHit2D[] CalculateNextPlatform(Vector2 direction, float distance)
	{
		// Calculated expected position
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance, 1 << 3);
		hits = hits.Where(x => x.collider.gameObject != currentPlatform.gameObject).OrderBy(x => Vector3.Distance(transform.position, x.point)).ToArray();
		Debug.DrawRay(transform.position, direction, Color.green, 10f);
		if (hits.Length > 0)    // We hit a wall that isn't the one we are already on
		{
			return hits;
		}
		
		return null;
	}

	private GameObject CalculateExpectedPosition(Vector2 direction, float distance)
    {
		RaycastHit2D[] hits = CalculateNextPlatform(direction, distance);
		if (hits != null)
        {
			expectedPosition = hits[0].point + direction * 0.2f;
			return hits[0].collider.gameObject;
		}
		expectedPosition = (Vector2)transform.position + direction * radius;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (state == PlayerState.InAir) return;
		RopePlatform ropePlatform = collision.GetComponent<RopePlatform>();
		if (ropePlatform != null && expectedPlatform.gameObject == ropePlatform.gameObject)
        {
			transform.position = transform.position + (Vector3)LaunchDirection * 0.2f;
			expectedPosition = transform.position;
			currentPlatform = expectedPlatform;
			state = PlayerState.Landing;
			coll.isTrigger = false;
			StartCoroutine(Land());
		}
    }
}

