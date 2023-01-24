using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingVineController : MonoBehaviour
{
	[SerializeField] private GameObject chasingVineSpawn;
	[SerializeField] private GameObject chasingVine;
	[SerializeField] private ChasingVineCam chasingVineCam;
	//[SerializeField] private Respawn respawn;	
	//[SerializeField] private Respawn newRespawn;
	[SerializeField] private float maxX;
	[SerializeField] private float spawnDelay;
	[SerializeField] private float movespeed;

	private bool isChasing;
	private bool isSpawning;
	private float timer;

	// Start is called before the first frame update
	void Start()
	{
		isChasing = false;
		isSpawning = false;
		timer = 0f;

		if (chasingVineCam == null)
		{
			chasingVineCam = FindObjectOfType<ChasingVineCam>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isSpawning)
		{
			// Check for specific frame to start chasing
			if (chasingVineSpawn.GetComponent<SpriteRenderer>().sprite.name == "vinechase1_8")
			{
				isSpawning = false;
				isChasing = true;
			}
		}
		if (isChasing)
		{
			// Check if chasing vine has been enabled
			if (!chasingVine.activeSelf)
			{
				chasingVine.SetActive(true);
				chasingVine.transform.SetParent(chasingVineCam.transform);
				chasingVineCam.Vine = chasingVine;
				chasingVineCam.IsMoving = true;
			}

			// Update position of chasing vine
			if (chasingVine.transform.position.x >= maxX)
			{
				chasingVineCam.IsMoving = false;
				isChasing = false;
				AudioSystem.Instance.PlayThemeDEnd();
			}
		}
	}

	private IEnumerator DelayedVineSpawn(float delay)
	{
		yield return new WaitForSeconds(delay);
		//respawn.gameObject.transform.position = newRespawn.gameObject.transform.position;
		//respawn.SpawnPlatform = newRespawn.SpawnPlatform;
		chasingVineSpawn.SetActive(true);
		isSpawning = true;
		isChasing = false;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		// Check for player collision
		if (collider.gameObject.tag == "Player")
		{
			// TODO: Play audio queue
			StartCoroutine(DelayedVineSpawn(spawnDelay));
			AudioSystem.Instance.PlayThemeDLoop();
		}
	}
}
