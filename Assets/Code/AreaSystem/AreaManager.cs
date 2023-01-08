using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
	[SerializeField, Header("Area 1")] private List<Collider2D> a1Colliders;
	[SerializeField] private Lantern a1Lantern;
	[SerializeField] private GameObject a1Camera;

	[SerializeField, Header("Area 2")] private List<Collider2D> a2Colliders;
	[SerializeField] private Lantern a2Lantern;
	[SerializeField] private GameObject a2Camera;

	[SerializeField, Header("Area 3")] private List<Collider2D> a3Colliders;
	[SerializeField] private Lantern a3Lantern;
	[SerializeField] private GameObject a3Camera;

	[SerializeField, Header("Area 4")] private List<Collider2D> a4Colliders;
	[SerializeField] private Lantern a4Lantern;
	[SerializeField] private GameObject a4Camera;

	[SerializeField, Header("Area 5")] private List<Collider2D> a5Colliders;
	[SerializeField] private Lantern a5Lantern;
	[SerializeField] private GameObject a5Camera;

	private Dictionary<int, List<Collider2D>> collidersDict = new Dictionary<int, List<Collider2D>>();
	private Dictionary<int, Lantern> lanternsDict = new Dictionary<int, Lantern>();
	private Dictionary<int, GameObject> camerasDict = new Dictionary<int, GameObject>();

	private int currentArea;

	private void Awake()
	{
		// Add all colliders into dictionary
		collidersDict.Add(1, a1Colliders);
		collidersDict.Add(2, a2Colliders);
		collidersDict.Add(3, a3Colliders);
		collidersDict.Add(4, a4Colliders);
		collidersDict.Add(5, a5Colliders);

		// Add all lanterns into dictionary
		lanternsDict.Add(1, a1Lantern);
		lanternsDict.Add(2, a2Lantern);
		lanternsDict.Add(3, a3Lantern);
		lanternsDict.Add(4, a4Lantern);
		lanternsDict.Add(5, a5Lantern);

		// Add all cameras into dictionary
		camerasDict.Add(1, a1Camera);
		camerasDict.Add(2, a2Camera);
		camerasDict.Add(3, a3Camera);
		camerasDict.Add(4, a4Camera);
		camerasDict.Add(5, a5Camera);

		currentArea = 0;
	}

	public bool AddSoulsToLantern(int amount)
	{
		// Check if area exists in dictionary
		if (lanternsDict.ContainsKey(currentArea))
		{
			// Add souls and check if filled
			if (lanternsDict[currentArea].AddSouls(amount))
			{
				// Filled
				ClearCurrentArea();

				// return true so player can increment their area
				return true;
			}

			return false;
		}

		// Log error
		Debug.Log("Lantern not found in area " + currentArea);
		return false;
	}

	public void EnterArea(int newArea)
	{
		// Enable all colliders
		foreach (Collider2D coll in collidersDict[newArea])
		{
			coll.enabled = true;
		}

		// Enable new camera and disable old one
		camerasDict[newArea].SetActive(true);
		camerasDict[newArea].SetActive(false);

		currentArea = newArea;
	}

	public void ClearCurrentArea()
	{
		//Disable all colliders
		foreach (Collider2D coll in collidersDict[currentArea])
		{
			coll.enabled = false;
		}
	}
}