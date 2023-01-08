using UnityEngine;
using System.Collections;

public class PlayerLauncher : MonoBehaviour
{
    // The Rigidbody2D component of the Player object
    public Rigidbody2D PlayerRigidbody;
    //The strength the player is launched
    public float launchForce;

    public float Speed;

    // Checking if the player is already in the air.
    private bool inAir = false;

    //Checking if the mouse is facing the oppisote direction of the object.
    private bool rightDireciton;
    private bool isMoving;


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
            float chargedPower = gameObject.GetComponent<AimController>().GetPowerScale() * launchForce;
            Vector2 launchVelocity = mouseDirection * chargedPower;

            PlayerRigidbody.AddForce(launchVelocity, ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.A) && !inAir){
            PlayerRigidbody.velocity = Vector2.left * Speed;
            isMoving = true;
        }
        if(Input.GetKeyDown(KeyCode.D) && !inAir){
            PlayerRigidbody.velocity = Vector2.right * Speed;
            isMoving = true;
        }
        if((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && !inAir){
            PlayerRigidbody.velocity = Vector3.zero;
            PlayerRigidbody.angularVelocity = 0;
            isMoving = false;
        }
        
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
        if(!inAir && !isMoving)
        {
            PlayerRigidbody.velocity = Vector3.zero;
            PlayerRigidbody.angularVelocity = 0;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && inAir)
        {
            inAir = false;
        }
    }
}

