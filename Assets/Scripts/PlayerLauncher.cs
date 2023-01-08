using UnityEngine;
using System.Collections;

public class PlayerLauncher : MonoBehaviour
{
    // The Rigidbody2D component of the Player object
    public Rigidbody2D PlayerRigidbody;
    //The strength the player is launched
    public float launchForce;

    //Aiming Objects

    // Checking if the player is already in the air.
    private bool inAir = false;

    //Checking if the mouse is facing the oppisote direction of the object.
    private bool rightDireciton;
    //Controller for AimController
    private AimController AC;
    private float radius;

    //InitalPosition of the Player
    private Vector2 initalPosition;

    private DistanceJoint2D relativeJoint;
    private Collision2D currentGround;
    [SerializeField] private Transform childSprite;
    private void Start() {
        AC = this.gameObject.GetComponent<AimController>();
        radius = AC.GetRadius();
        relativeJoint = GetComponent<DistanceJoint2D>();
    }

    private void Update() {
        
        if (Input.GetMouseButtonUp(0) && inAir == false && rightDireciton)
        {
            inAir = true;
            // Get the position of the mouse click
            Vector3 mousePos = Input.mousePosition;

            // Convert the mouse position to world space
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Calculate the direction and distance from the Player to the mouse click
            Vector2 mouseDirection = worldPos - transform.position;
            float distance = mouseDirection.magnitude;

            // Normalize the direction and scale it by the launch force
            mouseDirection = mouseDirection.normalized; 
            Vector2 launchVelocity = mouseDirection * launchForce;

            initalPosition = transform.position;
            
            if (relativeJoint.enabled)
            {
                relativeJoint.connectedBody = null;
                relativeJoint.enabled = false;
            }
            // Apply the launch force to the Player
            //StartCoroutine(Launch(launchVelocity));
            //PlayerRigidbody.AddForce(launchVelocity, ForceMode2D.Impulse);
        }
        //if(inAir)
        //{
        //    float distanceTraveled = Mathf.Sqrt(Mathf.Pow(transform.position.x - initalPosition.x, 2) + Mathf.Pow(transform.position.y - initalPosition.y, 2));  
        //    if(distanceTraveled > radius){
        //        Debug.Log("DEAD");
        //        inAir = false;
        //        PlayerRigidbody.velocity = Vector3.zero;
        //        PlayerRigidbody.angularVelocity = 0;
        //    }

        //}

        //if (relativeJoint.enabled)
        //{
        //    GameObject connectedBody = relativeJoint.connectedBody.gameObject;
        //    float distance = (connectedBody.transform.position - transform.position).magnitude;
        //    relativeJoint.distance = distance;
        //}

        if (Input.GetKey(KeyCode.A))
        {
            Move(-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(1);

        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.DrawLine(transform.position, collision.collider.bounds.center, Color.cyan);
        offset = collision.GetContact(0).point - (Vector2)transform.position;
    }

    Vector2 offset;
    private Vector2 AlignWithGround()
    {
        if (currentGround == null) return Vector2.zero;
        Vector3 direction = transform.position - currentGround.collider.bounds.center;
        transform.up = direction;
        
        RaycastHit2D ground = Physics2D.Raycast(transform.position, -direction, direction.magnitude, 1<<3);
        if (ground.collider != null)
        {
            Debug.Log(ground.point);
            return ground.point;
            //transform.position = ground.point - offset;
        }
        Debug.DrawRay(transform.position, -direction, Color.blue);
        return Vector2.zero;
        //Debug.DrawRay(transform.position, -transform.up, Color.red);
    }

    private void Move(int dir)
    {
        Vector2 groundPosition = AlignWithGround();
        //childSprite.position = groundPosition;
        //transform.position = (Vector3)transform.position + (transform.right * dir * Time.deltaTime);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentGround = collision;
        //if (collision.gameObject.tag == "Wall")
        //{
        //    inAir = false;
        //    PlayerRigidbody.velocity = Vector3.zero;
        //    PlayerRigidbody.angularVelocity = 0;
        //    PlayerRigidbody.gravityScale = 0;
        //} else if (collision.gameObject.tag == "Swing")
        //{
        //    inAir = false;
        //    relativeJoint.enabled = true;
        //    relativeJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
        //    PlayerRigidbody.velocity = Vector3.zero;
        //}
    }
}

