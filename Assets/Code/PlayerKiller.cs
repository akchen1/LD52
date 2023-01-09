using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
	private AreaManager areaManager;

	[SerializeField] private Collider2D coll;

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
			collision.gameObject.GetComponent<PlayerLauncher>().DieHard();
		}
	}
}
