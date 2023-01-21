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

		PlayWaterSFX(player.gameObject);
	}

	private void PlayWaterSFX(GameObject objectHit)
    {
		if (gameObject.tag != "Water") return;
		string sfx = "";
		if (objectHit.GetComponentInParent<RopePlatform>() != null)
		{
			Debug.Log("Play");
			Debug.Log(objectHit.name);
			sfx = "WaterRockSplash";
		} else if (objectHit.tag == "Player")
        {
			sfx = "WaterFoxSplash";
		}
		AudioSystem.Instance.PlaySFX(sfx);
	}
}
