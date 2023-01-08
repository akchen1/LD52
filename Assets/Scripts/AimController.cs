using UnityEngine;

public class AimController : MonoBehaviour
{
    public GameObject Player; // the player GameObject
    public GameObject Arrow;

    private float growthSpeed = 0.02f;
    private float startSize = 0.1f;
    private float originalSize = 1.0f;
    private float powerScale;

    void Start()
    {
        // Set the initial scale of the arrow
        Arrow.SetActive(false);
        Arrow.transform.localScale = new Vector3(startSize, startSize, 1);
    }

    void FixedUpdate()
    {

        // Control the length of the arrow based on the left mouse button
        if (Input.GetMouseButton(0))
        {
            Arrow.SetActive(true);
            // Rotate the arrow towards the mouse position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2(mousePos.y - Arrow.transform.position.y, mousePos.x - Arrow.transform.position.x) * Mathf.Rad2Deg;
            Arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Increase the size of the arrow by the growth speed
            Arrow.transform.localScale += new Vector3(growthSpeed, growthSpeed, 0);

            // Clamp the size of the arrow to the original size
            Arrow.transform.localScale = new Vector3(
                Mathf.Min( Arrow.transform.localScale.x, originalSize),
                Mathf.Min( Arrow.transform.localScale.y, originalSize),
                1
            );
            powerScale = Arrow.transform.localScale.x;
        }
        else
        {
            Arrow.SetActive(false);
            Arrow.transform.localScale = new Vector3(startSize, startSize, 1);
        }
    }
    public float GetPowerScale(){
        return powerScale;
    }
}