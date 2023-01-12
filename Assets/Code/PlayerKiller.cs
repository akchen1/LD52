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


	void OnTriggerEnter2D(Collider2D player)
	{
		if (player.gameObject.tag == "Player")
		{
			var objectThatMadeItTrigger = player.gameObject;
			player.gameObject.GetComponent<PlayerLauncher>().DieHard();
		}
	}

}
