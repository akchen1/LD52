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

    private RelativeJoint2D relativeJoint;

    private void Start() {
        AC = this.gameObject.GetComponent<AimController>();
        radius = AC.GetRadius();
        relativeJoint = GetComponent<RelativeJoint2D>();
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
            PlayerRigidbody.AddForce(launchVelocity, ForceMode2D.Impulse);
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


    }

    private IEnumerator Launch(Vector2 launchVelocity)
    {
        yield return 0;
        PlayerRigidbody.AddForce(launchVelocity, ForceMode2D.Impulse);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Get the position of the mouse in world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle between the mouse and the object
        float angle = Vector2.Angle(mouseWorldPosition - transform.position, collision.contacts[0].normal);
        // Check if the angle is within the desired range
        if (angle >= 0 && angle <= 88)
        {
            rightDireciton = true;
        }
        else
        {
            rightDireciton = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 zAxis = new Vector3(0, 0, 1);
            transform.RotateAround(collision.transform.position, zAxis,3);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 zAxis = new Vector3(0, 0, 1);
            transform.RotateAround(collision.transform.position, zAxis,-3);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            inAir = false;
            PlayerRigidbody.velocity = Vector3.zero;
            PlayerRigidbody.angularVelocity = 0;
            PlayerRigidbody.gravityScale = 0;
        } else if (collision.gameObject.tag == "Swing")
        {
            inAir = false;
            relativeJoint.enabled = true;
            relativeJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }
}

