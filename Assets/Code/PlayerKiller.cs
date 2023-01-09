using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
	private AreaManager areaManager;

	[SerializeField] private Collider2D coll;
	[SerializeField] private GameObject deathAnim;

	// Start is called before the first frame update
	void Start()
	{
		areaManager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			// Player dead
			Vector3? spawnPos = areaManager.GetCurrentSpawn().transform.position;
			if (spawnPos != null)
			{
				// Instantiate death animation at player death location
				Instantiate(deathAnim, collision.gameObject.transform.position, Quaternion.identity);

				// Move Player to spawn position
				collision.gameObject.transform.position = (Vector3)areaManager.GetCurrentSpawn().transform.position;
			}
		}
	}
}
