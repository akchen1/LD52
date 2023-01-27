using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerLauncher : MonoBehaviour
{
	public enum PlayerState { InPlatform, InAir, Landing, LandingTransition, Dead, Respawning, EndTransition, End }
	// The Rigidbody2D component of the Player object
	public Rigidbody2D PlayerRigidbody;

	//Controller for AimController
	private AimController AC;
	private Collider2D coll;
	private Animator animator;
	private float radius;   // launch radius
	private Vector2 moveDir;

	// Set to the current platform the player is on
	[SerializeField] private Platform currentPlatform;
	[SerializeField] private Image endScreen;

	// State of the player
	public PlayerState state;

	// Expected position where the player will end up after jumping
	private Vector3 expectedPosition;
	private Vector2 launchDirection;
	private Vector3 maxLaunchPosition;
	private Vector3 startLaunchPosition;
	private Platform expectedPlatform;
	private Vector2 normal;

	private float directionScale;   // Keeps the player moving in the same direction if holding down a key

	[SerializeField] private GameObject child;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float airTravelSpeed;
	float playerRadius;
	private void Start()
	{
		AC = this.gameObject.GetComponent<AimController>();
		radius = AC.GetRadius();
		coll = GetComponent<Collider2D>();
		animator = GetComponentInChildren<Animator>();
		state = PlayerState.InPlatform;
		playerRadius = GetComponent<CircleCollider2D>().radius;
		directionScale = 1;
	}

	private void Update()
	{
		if (state == PlayerState.Dead) return;
		if (PauseScreen.Instance.IsPaused()) return;
		BeginJump();
		Travel();
		Move();
		RotateChild();
		SetAnimation();

		//Debug.DrawRay(transform.position, child.transform.right, Color.red);
		//Debug.DrawRay(transform.position, child.transform.up, Color.green);

		//CheckEnd();
	}

	//private void CheckEnd()
	//{
	//	if (state != PlayerState.InPlatform) return;
	//	EndingTrigger end = currentPlatform?.GetComponentInChildren<EndingTrigger>();
	//	if (end != null)
	//	{
	//		StartCoroutine(EndGame());

	//	}
	//}

	public IEnumerator EndGame()
	{
		endScreen.gameObject.SetActive(true);
		PlayerRigidbody.bodyType = RigidbodyType2D.Static;
		state = PlayerState.EndTransition;
		yield return new WaitForSecondsRealtime(3f / 6f);
		state = PlayerState.End;
		yield return new WaitForSeconds((7f / 6) * 5f);

		for (float t = 0f; t <= 2f; t += Time.deltaTime)
		{
			float normalizedTime = t / 2f;
			Color color = endScreen.color;
			color.a = normalizedTime;
			endScreen.color = color;
			yield return 0;
		}

	}
	private void RotateChild()
	{
		if (state != PlayerState.InPlatform) return;
		normal = currentPlatform.GetClosestEdge(transform.position);

		float angle = Vector2.SignedAngle(Vector2.down, normal);
		child.transform.eulerAngles = new Vector3(0, 0, angle);

		Vector3 childScale = child.transform.localScale;
		childScale.x = Mathf.Sign(-moveDir.x) * Mathf.Sign(normal.y);

		child.transform.localScale = childScale;
		//Debug.DrawRay(child.transform.position, child.transform.up, Color.red, 2f);


	}

	private void SetAnimation()
	{
		animator.SetBool("isDead", state == PlayerState.Dead);
		animator.SetBool("inPlatform", state == PlayerState.InPlatform);
		animator.SetBool("isLanding", state == PlayerState.Landing);
		animator.SetBool("isTransition", state == PlayerState.LandingTransition);
		animator.SetBool("isRespawning", state == PlayerState.Respawning);
		animator.SetBool("isEnd", state == PlayerState.End);
		animator.SetBool("isEndTransition", state == PlayerState.EndTransition);
	}

	private void Move()
	{
		if (state != PlayerState.InPlatform) return;
		Vector2 move = transform.position;
		moveDir = Vector2.zero;
		float speed = 0;
		int normalScale = 1;    // scales the up direction depending if going left or right

		//if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
		//{
		//          directionScale = Mathf.Sign(-normal.y);
		//      }

		if (Input.GetKey(KeyCode.A))
		{
			speed += moveSpeed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			speed -= moveSpeed;
			normalScale = -1;
		}

		moveDir = directionScale * (Vector2)child.transform.right * speed * 1.1f;

		Vector2 trueDirection = moveDir + ((Vector2)child.transform.up * speed * normalScale) * 0.5f;
		//Debug.DrawRay(child.transform.position, moveDir.normalized, Color.green, 2f);

		move += trueDirection;
		//Debug.DrawRay(child.transform.position, trueDirection.normalized, Color.blue, 2f);
		PlayerRigidbody.MovePosition(move);
		animator.SetFloat("Horizontal", move.x);
		animator.SetFloat("Vertical", move.y);
		animator.SetBool("isMoving", moveDir.magnitude > 0);

	}

	private void Travel()
	{
		if (state != PlayerState.InAir) return;

		Vector2 moveVector = maxLaunchPosition - startLaunchPosition;

		bool checkIfSame = currentPlatform.GetComponent<RopePlatform>() != null;

		GameObject expected = CalculateExpectedPosition(startLaunchPosition, moveVector.normalized, moveVector.magnitude, checkIfSame);
		expectedPlatform = expected?.GetComponent<Platform>();

		// Check if arrived at expected position
		Vector2 direction = (expectedPosition - transform.position).normalized;
		float distance = Vector2.Distance(expectedPosition, transform.position);
		float maxDistance = Vector2.Distance(expectedPosition, startLaunchPosition);
		float speed = Mathf.Clamp(airTravelSpeed * (distance / maxDistance), 10, airTravelSpeed);
		if (distance >= 0.1f)   // Did not arrive at expected position
		{
			PlayerRigidbody.velocity = direction.normalized * speed;
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
			expectedPlatform = null;
			state = PlayerState.Landing;
			coll.isTrigger = false;
			StartCoroutine(Land());
		}
	}


	public void DieHard()
	{
		child.transform.rotation = Quaternion.identity;
		state = PlayerState.Dead;
		SetAnimation();
		PlayerRigidbody.velocity = Vector3.zero;
		PlayerRigidbody.angularVelocity = 0;
		AudioSystem.Instance.PlaySFX("FoxDeath");

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

		//normal = currentPlatform.GetClosestEdge(transform.position);
		//directionScale = Mathf.Sign(-normal.y);
	}

	private void BeginJump()
	{
		if (state != PlayerState.InPlatform) return;
		if (!(Input.GetMouseButtonUp(0))) return;
		if (EventSystem.current.IsPointerOverGameObject()) return;

		Vector3 worldPos = GetMouseWorldPosition();

		// Calculate the direction and distance from the Player to the mouse click
		Vector2 mouseDirection = worldPos - transform.position;

		// Normalize the direction and scale it by the launch force
		mouseDirection = mouseDirection.normalized;


		GameObject nextPlatform = CalculateExpectedPosition(transform.position, mouseDirection, radius, false);
		if (currentPlatform.gameObject == nextPlatform?.gameObject && Vector3.Distance(expectedPosition, transform.position) <= 1f)
		{
			return;
		}

		Platform platform = nextPlatform?.GetComponent<Platform>();
		if (platform?.GetComponent<RopePlatform>() != null && currentPlatform.gameObject == platform.gameObject)
		{
			return;
		}

		maxLaunchPosition = transform.position + (Vector3)mouseDirection * radius;
		launchDirection = mouseDirection.normalized;
		coll.isTrigger = true;
		expectedPlatform = platform;

		startLaunchPosition = transform.position;
		state = PlayerState.InAir;

		Quaternion rot = Quaternion.FromToRotation(Vector3.down, mouseDirection);
		child.transform.rotation = rot;

		AudioSystem.Instance.PlaySFX("FoxDash");
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
		rb.AddForce(direction * 100, ForceMode2D.Impulse);
	}


	private RaycastHit2D[] CalculateNextPlatform(Vector2 startPosition, Vector2 direction, float distance, bool checkIfSame)
	{
		// Calculated expected position

		Vector2 offset = direction * playerRadius * transform.localScale;
		RaycastHit2D[] hits = Physics2D.RaycastAll(startPosition + offset, direction, distance - playerRadius, 1 << 3);
		hits = hits
			.Where(x =>
			{
				if (checkIfSame)
				{
					return x.collider.gameObject != currentPlatform?.gameObject && currentPlatform?.gameObject != expectedPlatform?.gameObject;
				}
				return true;
			})
			.OrderBy(x => Vector3.Distance(startPosition, x.point)).ToArray();

		if (hits.Length > 0)    // We hit a wall that isn't the one we are already on
		{
			return hits;
		}

		return null;
	}

	private GameObject CalculateExpectedPosition(Vector2 startPosition, Vector2 direction, float distance, bool checkIfSame)
	{
		RaycastHit2D[] hits = CalculateNextPlatform(startPosition, direction, distance, checkIfSame);
		if (hits != null)
		{

			expectedPosition = hits[0].point - direction * 0.15f;
			return hits[0].collider.gameObject;
		}
		expectedPosition = startPosition + direction * distance;
		return null;
	}

	private IEnumerator PlayerRespawn()
	{
		yield return new WaitForSeconds(1.167f);
		//Death Function
		AreaManager areaManager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
		GameObject spawnGO = areaManager.GetCurrentSpawn();
		transform.position = spawnGO.transform.position;
		state = PlayerState.Respawning;

		int currentArea = areaManager.GetCurrentArea();
		if (currentArea == 7 || currentArea == 8)
		{
			FindObjectOfType<ChasingVineCam>().ResetPosition();
			FindObjectOfType<ResetAreaController>().ResetArea(false);
			AudioSystem.Instance.ResetAudio();
			areaManager.EnterArea(currentArea);
		}

		yield return new WaitForSeconds(0.833f);
		currentPlatform = spawnGO.GetComponent<Respawn>().SpawnPlatform;
		gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
		state = PlayerState.InPlatform;
	}
}

