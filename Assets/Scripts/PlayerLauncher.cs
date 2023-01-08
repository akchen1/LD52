using UnityEngine;
using System.Collections;
using System.Linq;
public class PlayerLauncher : MonoBehaviour
{
    public enum PlayerState { InPlatform, InAir, Dead}
    // The Rigidbody2D component of the Player object
    public Rigidbody2D PlayerRigidbody;
    //The strength the player is launched

    //Controller for AimController
    private AimController AC;
    private Collider2D coll;
    private Animator animator;
    private float radius;

    // Set to the current platform the player is on
    [SerializeField] private Platform currentPlatform;

    // State of the player
    public PlayerState state;

    // Used for rope platform. keep track of original Rigidbody type
    private RigidbodyType2D currentPlatformOriginalRBType;

    // Expected position where the player will end up after jumping
    private Vector3 expectedPosition;

    private void Start() {
        AC = this.gameObject.GetComponent<AimController>();
        radius = AC.GetRadius();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        state = PlayerState.InPlatform;
    }

    private void Update() {
        if (state == PlayerState.Dead) return;
        BeginJump();

        Travel();

        Move();

        SetAnimation();
    }

    private void SetAnimation()
    {
        animator.SetBool("isDead", state == PlayerState.Dead);
        animator.SetBool("inPlatform", state == PlayerState.InPlatform);
    }

    private void Move()
    {
        if (state != PlayerState.InPlatform) return;
        Vector2 move = transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            move.x -= .1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.x += .1f;


        }
        if (Input.GetKey(KeyCode.W))
        {
            move.y += .1f;

        }
        if (Input.GetKey(KeyCode.S))
        {
            move.y -= .1f;


        }
        PlayerRigidbody.MovePosition(move);
    }

    private void Travel()
    {
        if (state != PlayerState.InAir) return;

        if (Vector2.Distance(expectedPosition, transform.position) >= 0.1f)
        {
            Vector2 direction = expectedPosition - transform.position;
            PlayerRigidbody.velocity = direction.normalized * 10f;
            return;
        }

        PlayerRigidbody.velocity = Vector2.zero;
        if (currentPlatform == null)
        {
            // Insert dead function
            Debug.Log("DEAD");
            state = PlayerState.Dead;
            PlayerRigidbody.velocity = Vector3.zero;
            PlayerRigidbody.angularVelocity = 0;
        }
        else
        {
            state = PlayerState.InPlatform;
            coll.isTrigger = false;
        }
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
        

        GameObject nextPlatform = CalculateNextPlatform(mouseDirection);
        if (nextPlatform != null && nextPlatform.tag == "InnerWall")
        {
            return;
        }
        // Set player to trigger, Ignore all collision
        coll.isTrigger = true;
        SetRigidBodyType(currentPlatform, currentPlatformOriginalRBType);

        Platform platform = nextPlatform?.GetComponent<Platform>();
        SetRigidBodyType(platform, RigidbodyType2D.Static);

        currentPlatform = platform;

        state = PlayerState.InAir;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Get the position of the mouse click
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to world space
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void SetRigidBodyType(Platform platform, RigidbodyType2D type)
    {
        if (platform == null) return;
        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        currentPlatformOriginalRBType = rb.bodyType;
        rb.bodyType = type;

    }

    private GameObject CalculateNextPlatform(Vector2 direction)
    {
        // Calculated expected position
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, radius, 1 << 3);
        hits = hits.Where(x => x.collider.gameObject != currentPlatform.gameObject).OrderBy(x => Vector3.Distance(transform.position, x.point)).ToArray();

        if (hits.Length > 0)    // We hit a wall that isn't the one we are already on
        {
            expectedPosition = hits[0].point + direction * 0.15f;
            return hits[0].collider.gameObject;
        }
        expectedPosition = (Vector2)transform.position + direction * radius;
        return null;
    }
}

