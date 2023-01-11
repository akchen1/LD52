using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
	private AreaManager areaManager;
	public int Area;

	private void Awake()
	{
		areaManager = GameObject.FindGameObjectWithTag("AreaManager").GetComponent<AreaManager>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(areaManager.GetCurrentArea() != Area){
			if (other.gameObject.tag == "Player")
			{
				areaManager.EnterArea(Area);
			}
		}
	}
}
