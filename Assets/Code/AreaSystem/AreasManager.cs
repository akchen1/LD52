using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreasManager : MonoBehaviour
{
	[SerializeField, Header("Level 1")] private List<Collider2D> colliders1;

	[SerializeField, Header("Level 2")] private List<Collider2D> colliders2;

	[SerializeField, Header("Level 3")] private List<Collider2D> colliders3;

	[SerializeField, Header("Level 4")] private List<Collider2D> colliders4;

	[SerializeField, Header("Level 5")] private List<Collider2D> colliders5;

	private Dictionary<int, List<Collider2D>> collidersDict = new Dictionary<int, List<Collider2D>>();

	private void Awake()
	{
		// Add all colliders into dictionary
		collidersDict.Add(1, colliders1);
		collidersDict.Add(2, colliders2);
		collidersDict.Add(3, colliders3);
		collidersDict.Add(4, colliders4);
		collidersDict.Add(5, colliders5);
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
		}
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
		}
	}
}
