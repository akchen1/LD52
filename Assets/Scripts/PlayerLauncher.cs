using UnityEngine;
using System.Collections;
using System.Linq;
public class PlayerLauncher : MonoBehaviour
{
    public enum PlayerState { InPlatform, InAir, Landing, LandingTransition, Dead}
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

    [SerializeField] private GameObject child;

    private void Start() {
        AC = this.gameObject.GetComponent<AimController>();
        radius = AC.GetRadius();
        coll = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        state = PlayerState.InPlatform;
    }

    private void Update() {
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

        //Debug.Log(groundSlopeDirection);

        //child.transform.rotation = Quaternion.Euler(0, 0, angle);
        Quaternion rot = Quaternion.FromToRotation(Vector3.down, normal);
        child.transform.rotation = rot;

        //Debug.Log(normal.y);

        if (normal.y < 0) // on top
        {
            Debug.Log("On top");
            if (moveDir.magnitude != 0)
            {
                Vector3 childScale = child.transform.localScale;
                childScale.x = moveDir.x < 0 ? -1 : 1;
                child.transform.localScale = childScale;

            }

        } else if (normal.y > 0)
        {
            Debug.Log("On Bottom");

            if (moveDir.magnitude != 0)
            {
                Vector3 childScale = child.transform.localScale;
                if (moveDir.y == 0) // im not pressing up or down
                {
                    childScale.x = moveDir.x < 0 ? 1 : -1;

                } else if (moveDir.x == 0)// im not presssing left or right
                {
                    childScale.x = moveDir.y < 0 ? 1 : -1;
                }
                else// I am pressing both
                {
                    childScale.x = moveDir.x < 0 ? 1 : -1;
                }

                Debug.Log(moveDir);
                //childScale.x = moveDir.x < 0 && moveDir.y < 0 ? 1 : -1;
                child.transform.localScale = childScale;

            }
        }
        //Debug.DrawRay(transform.position, Vector3.up, Color.red);
        //Debug.DrawRay(transform.position, normal, Color.blue);
    }

    private void SetAnimation()
    {
        animator.SetBool("isDead", state == PlayerState.Dead);
        animator.SetBool("inPlatform", state == PlayerState.InPlatform);
        animator.SetBool("isLanding", state == PlayerState.Landing);
        animator.SetBool("isTransition", state == PlayerState.LandingTransition);
    }
    private void Move()
    {
        if (state != PlayerState.InPlatform) return;
        Vector2 move = transform.position;
        moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= .1f;
            //if (child.transform.rotation.z >= 180)
            //{
            //    child.GetComponent<SpriteRenderer>().flipX = false;

            //}
            //else
            //{

            //child.GetComponent<SpriteRenderer>().flipX = true;
            //}

        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += .1f;
            //if (child.transform.rotation.z >= 180)
            //{
            //    child.GetComponent<SpriteRenderer>().flipX = true;

            //}
            //else
            //{

            //    child.GetComponent<SpriteRenderer>().flipX = false;
            //}

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
        animator.SetBool("isMoving", move.magnitude > 0.1f);
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
            ////Death Animation
            //child.transform.rotation = Quaternion.identity;
            //state = PlayerState.Dead;
            //PlayerRigidbody.velocity = Vector3.zero;
            //PlayerRigidbody.angularVelocity = 0;

            //StartCoroutine(PlayerRespawn());
            
        }
        else
        {
            state = PlayerState.Landing;
            coll.isTrigger = false;
            StartCoroutine(Land());
        }
    }

    private IEnumerator Land()
    {
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
    private IEnumerator PlayerRespawn(){
        yield return new WaitForSeconds(1.167f);
        //Death Function
        GameObject spawnGO = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>().GetCurrentSpawn();
        transform.position = spawnGO.transform.position;
        state = PlayerState.InPlatform;
        currentPlatform = spawnGO.GetComponent<Respawn>().SpawnPlatform;
    }
}

