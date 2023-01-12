using UnityEngine;

public class AimController : MonoBehaviour
{
    public GameObject Player; // the player GameObject
    public GameObject Arrow;
    private float radius; 

    void Start()
    {
        // Set the initial scale of the arrow
        Arrow.SetActive(false);
    }

    void FixedUpdate()
    {

        // Control the length of the arrow based on the left mouse button
        if (Input.GetMouseButton(1))
        {
            Arrow.SetActive(true);
            // Rotate the arrow towards the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2(mousePos.y - Arrow.transform.position.y, mousePos.x - Arrow.transform.position.x) * Mathf.Rad2Deg;
            Arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            Arrow.SetActive(false);
        }
    }

    public float GetRadius(){
        return (Arrow.GetComponent<SpriteRenderer>().bounds.size.x)/2;
    }

}