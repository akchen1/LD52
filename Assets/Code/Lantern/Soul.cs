using UnityEngine;

public class Soul : MonoBehaviour
{
	// Start is called before the first frame update
	private AreaManager AM;

	private Lantern lantern;

	private Vector3 targetPosition;

	void Start()
	{
		AM = GameObject.Find("Area Manager").GetComponent<AreaManager>();
		lantern = AM.GetCurrentLantern();
		targetPosition = lantern.Indicators[lantern.CurrentSouls].transform.position;

		float angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x);

		// Convert the angle from radians to degrees
		float zRotation = angle * Mathf.Rad2Deg;

		// Set the object's z-axis rotation to the calculated angle
		transform.rotation = Quaternion.Euler(0, 0, zRotation);
	}
	private void Update()
	{
		// Set the movement speed
		float speed = 7.5f;

		// Get the object's current position
		Vector2 currentPosition = transform.position;

		// Calculate the next position
		Vector2 nextPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

		// Update the object's position
		transform.position = nextPosition;
		if (transform.position == targetPosition)
		{
			AM.AddSoulToLantern();
			Destroy(gameObject);
		}
	}
}
