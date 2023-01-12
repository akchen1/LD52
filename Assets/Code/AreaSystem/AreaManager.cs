using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
	public int StartArea;
	[SerializeField, Header("Area 0")] private List<Collider2D> a0Colliders;
	[SerializeField] private GameObject a0Camera;
	[SerializeField] private Transform a0Spawn;
	[SerializeField] private SpriteRenderer a0Sprite;   // used for temporary solution to enable/disable spawn area for area 5-6 transiton
	[SerializeField] private SpriteRenderer a0CoverBlock;

	[SerializeField, Header("Area 1")] private List<Collider2D> a1Colliders;
	[SerializeField] private Lantern a1Lantern;
	[SerializeField] private GameObject a1Camera;
	[SerializeField] private Transform a1Spawn;
	[SerializeField] private Parallax a1Background;

	[SerializeField, Header("Area 2")] private List<Collider2D> a2Colliders;
	[SerializeField] private Lantern a2Lantern;
	[SerializeField] private GameObject a2Camera;
	[SerializeField] private Transform a2Spawn;
	[SerializeField] private Parallax a2Background;

	[SerializeField, Header("Area 3")] private List<Collider2D> a3Colliders;
	[SerializeField] private Lantern a3Lantern;
	[SerializeField] private GameObject a3Camera;
	[SerializeField] private Transform a3Spawn;
	[SerializeField] private Parallax a3Background;

	[SerializeField, Header("Area 4")] private List<Collider2D> a4Colliders;
	[SerializeField] private Lantern a4Lantern;
	[SerializeField] private GameObject a4Camera;
	[SerializeField] private Transform a4Spawn;
	[SerializeField] private Parallax a4Background;

	[SerializeField, Header("Area 5")] private List<Collider2D> a5Colliders;
	[SerializeField] private Lantern a5Lantern;
	[SerializeField] private GameObject a5Camera;
	[SerializeField] private Transform a5Spawn;
	[SerializeField] private Parallax a5Background;

	[SerializeField, Header("Area 6")] private List<Collider2D> a6Colliders;
	[SerializeField] private Lantern a6Lantern;
	[SerializeField] private GameObject a6Camera;
	[SerializeField] private Transform a6Spawn;
	[SerializeField] private Parallax a6Background;
	[SerializeField, Header("Area 7")] private List<Collider2D> a7Colliders;
	[SerializeField] private Lantern a7Lantern;
	[SerializeField] private GameObject a7Camera;
	[SerializeField] private Transform a7Spawn;
	[SerializeField] private Parallax a7Background;
	[SerializeField, Header("Area 8")] private List<Collider2D> a8Colliders;
	[SerializeField] private Lantern a8Lantern;
	[SerializeField] private GameObject a8Camera;
	[SerializeField] private Transform a8Spawn;
	[SerializeField] private Parallax a8Background;
	[SerializeField, Header("Area 9")] private List<Collider2D> a9Colliders;
	[SerializeField] private Lantern a9Lantern;
	[SerializeField] private GameObject a9Camera;
	[SerializeField] private Transform a9Spawn;
	[SerializeField] private Parallax a9Background;

	[SerializeField] private SpriteRenderer spawnCoverBlock;

	private Dictionary<int, List<Collider2D>> collidersDict = new Dictionary<int, List<Collider2D>>();
	private Dictionary<int, Lantern> lanternsDict = new Dictionary<int, Lantern>();
	private Dictionary<int, GameObject> camerasDict = new Dictionary<int, GameObject>();
	private Dictionary<int, Transform> spawnsDict = new Dictionary<int, Transform>();
	private Dictionary<int, Parallax> backgroundDict = new Dictionary<int, Parallax>();

	private int currentArea;

	private void Awake()
	{
		// Add all colliders into dictionary
		collidersDict.Add(0, a0Colliders);
		collidersDict.Add(1, a1Colliders);
		collidersDict.Add(2, a2Colliders);
		collidersDict.Add(3, a3Colliders);
		collidersDict.Add(4, a4Colliders);
		collidersDict.Add(5, a5Colliders);
		collidersDict.Add(6, a6Colliders);
		collidersDict.Add(7, a7Colliders);
		collidersDict.Add(8, a8Colliders);
		collidersDict.Add(9, a9Colliders);

		// Add all lanterns into dictionary
		lanternsDict.Add(1, a1Lantern);
		lanternsDict.Add(2, a2Lantern);
		lanternsDict.Add(3, a3Lantern);
		lanternsDict.Add(4, a4Lantern);
		lanternsDict.Add(5, a5Lantern);
		lanternsDict.Add(6, a6Lantern);
		lanternsDict.Add(7, a7Lantern);
		lanternsDict.Add(8, a8Lantern);
		lanternsDict.Add(9, a9Lantern);

		// Add all cameras into dictionary
		camerasDict.Add(0, a0Camera);
		camerasDict.Add(1, a1Camera);
		camerasDict.Add(2, a2Camera);
		camerasDict.Add(3, a3Camera);
		camerasDict.Add(4, a4Camera);
		camerasDict.Add(5, a5Camera);
		camerasDict.Add(6, a6Camera);
		camerasDict.Add(7, a7Camera);
		camerasDict.Add(8, a8Camera);
		camerasDict.Add(9, a9Camera);

		// Add spawns into dictionary
		spawnsDict.Add(0, a0Spawn);
		spawnsDict.Add(1, a1Spawn);
		spawnsDict.Add(2, a2Spawn);
		spawnsDict.Add(3, a3Spawn);
		spawnsDict.Add(4, a4Spawn);
		spawnsDict.Add(5, a5Spawn);
		spawnsDict.Add(6, a6Spawn);
		spawnsDict.Add(7, a7Spawn);
		spawnsDict.Add(8, a8Spawn);
		spawnsDict.Add(9, a9Spawn);

		// Add backgrounds to dictionary
		backgroundDict.Add(0, null);
		backgroundDict.Add(1, a1Background);
		backgroundDict.Add(2, a2Background);
		backgroundDict.Add(3, a3Background);
		backgroundDict.Add(4, a4Background);
		backgroundDict.Add(5, a5Background);
		backgroundDict.Add(6, a6Background);
		backgroundDict.Add(7, a7Background);
		backgroundDict.Add(8, a8Background);
		backgroundDict.Add(9, a9Background);
	
		currentArea = 0;
		//EnterArea(6);
	}
	public bool AddSoulToLantern()
	{
		// Check if area exists in dictionary
		if (lanternsDict.ContainsKey(currentArea))
		{
			// Add souls and check if filled
			if (lanternsDict[currentArea].AddSoul())
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

		CheckExceptions(newArea);

		//Enable new camera and disable old one
		if (camerasDict[newArea] != camerasDict[currentArea]){
			camerasDict[newArea].SetActive(true);
			camerasDict[currentArea].SetActive(false);
        }


		if (backgroundDict[newArea] != backgroundDict[currentArea])
        {
            backgroundDict[currentArea]?.DeactivateParallax();
            backgroundDict[newArea]?.EnableParallax(newArea);
		}

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

	public Lantern? GetCurrentLantern()
	{
		// Check if area exists in dictionary
		if (lanternsDict.ContainsKey(currentArea))
		{
			// Return lantern
			return lanternsDict[currentArea];
		}

		// Log error
		Debug.Log("Lantern not found in area " + currentArea);
		return null;
	}

	public GameObject GetCurrentSpawn()
	{
		// Check if area exists in dictionary
		if (spawnsDict.ContainsKey(currentArea))
		{
			// Return lantern
			return spawnsDict[currentArea].gameObject;
		}

		// Log error
		Debug.Log("Spawn position not found in area " + currentArea);
		return null;
	}

	public int GetCurrentArea()
	{
		return currentArea;
	}

	private void CheckExceptions(int newArea)
    {
		if ((currentArea == 5 && newArea == 6) || (currentArea == 6 && newArea == 5))
		{
			spawnCoverBlock.enabled = true;
			a0Sprite.enabled = false;

		}
		else
		{
			spawnCoverBlock.enabled = false;
			a0Sprite.enabled = true;
		}

		if ((currentArea == 0 && newArea == 1) || (currentArea == 1 && newArea == 0))
        {
			a0CoverBlock.enabled = true;
        } else
        {
			a0CoverBlock.enabled = false;
        }
	}
}
