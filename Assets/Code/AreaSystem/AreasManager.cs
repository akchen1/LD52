using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasManager : MonoBehaviour
{
	[SerializeField, Header("Level 1")] private List<Collider2D> a1Colliders;
	[SerializeField] private Lantern a1Lantern;

	[SerializeField, Header("Level 2")] private List<Collider2D> a2Colliders;
	[SerializeField] private Lantern a2Lantern;

	[SerializeField, Header("Level 3")] private List<Collider2D> a3Colliders;
	[SerializeField] private Lantern a3Lantern;

	[SerializeField, Header("Level 4")] private List<Collider2D> a4Colliders;
	[SerializeField] private Lantern a4Lantern;

	[SerializeField, Header("Level 5")] private List<Collider2D> a5Colliders;
	[SerializeField] private Lantern a5Lantern;

	private Dictionary<int, List<Collider2D>> collidersDict = new Dictionary<int, List<Collider2D>>();
	private Dictionary<int, Lantern> lanternsDict = new Dictionary<int, Lantern>();

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
	}

	private void Update()
	{
		// All these are used for testing
		// Press the number to open corresponding area, LeftShift + Number will close it
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			OpenArea(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.LeftShift))
		{
			CloseArea(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			OpenArea(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && Input.GetKeyDown(KeyCode.LeftShift))
		{
			CloseArea(2);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			OpenArea(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && Input.GetKeyDown(KeyCode.LeftShift))
		{
			CloseArea(3);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			OpenArea(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4) && Input.GetKeyDown(KeyCode.LeftShift))
		{
			CloseArea(4);
		}

		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			OpenArea(5);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5) && Input.GetKeyDown(KeyCode.LeftShift))
		{
			CloseArea(5);
		}
	}

	public void OpenArea(int area)
	{
		// Check if area exists in dictionary
		if (collidersDict.ContainsKey(area))
		{
			// Disable all colliders
			foreach (Collider2D coll in collidersDict[area])
			{
				coll.enabled = false;
			}

			return;
		}

		// Log error
		Debug.Log("Can't open area " + area + " - not found");
	}

	public void CloseArea(int area)
	{
		// Check if area exists in dictionary
		if (collidersDict.ContainsKey(area))
		{
			// Enable all colliders
			foreach (Collider2D coll in collidersDict[area])
			{
				coll.enabled = true;
			}

			return;
		}

		// Log error
		Debug.Log("Can't close area " + area + " - not found");
	}

	public bool AddSoulsToLantern(int area, int amount)
	{
		// Check if area exists in dictionary
		if (lanternsDict.ContainsKey(area))
		{
			// Add souls and check if filled
			if (lanternsDict[area].AddSouls(amount))
			{
				// Filled, open area
				OpenArea(area);

				// return true so player can increment their area
				return true;
			}

			return false;
		}

		// Log error
		Debug.Log("Lantern not found in area " + area);
		return false;
	}
}
