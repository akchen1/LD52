using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AreaManager : MonoBehaviour
{
	[SerializeField, Header("Area 0")] private List<Collider2D> a0Colliders;
	[SerializeField] private GameObject a0Camera;
	[SerializeField] private Transform a0Spawn;
	[SerializeField] private SpriteRenderer a0Sprite;   // used for temporary solution to enable/disable spawn area for area 5-6 transiton
	[SerializeField] private SpriteRenderer a0CoverBlock;
	[SerializeField] private List<GameObject> a0Fog;

	[SerializeField, Header("Area 1")] private List<Collider2D> a1Colliders;
	[SerializeField] private Lantern a1Lantern;
	[SerializeField] private GameObject a1Camera;
	[SerializeField] private Transform a1Spawn;
	[SerializeField] private Parallax a1Background;
	[SerializeField] private List<GameObject> a1Fog;


	[SerializeField, Header("Area 2")] private List<Collider2D> a2Colliders;
	[SerializeField] private Lantern a2Lantern;
	[SerializeField] private GameObject a2Camera;
	[SerializeField] private Transform a2Spawn;
	[SerializeField] private Parallax a2Background;
	[SerializeField] private List<GameObject> a2Fog;

	[SerializeField, Header("Area 3")] private List<Collider2D> a3Colliders;
	[SerializeField] private Lantern a3Lantern;
	[SerializeField] private GameObject a3Camera;
	[SerializeField] private Transform a3Spawn;
	[SerializeField] private Parallax a3Background;
	[SerializeField] private  List<GameObject> a3Fog;

	[SerializeField, Header("Area 4")] private List<Collider2D> a4Colliders;
	[SerializeField] private Lantern a4Lantern;
	[SerializeField] private GameObject a4Camera;
	[SerializeField] private Transform a4Spawn;
	[SerializeField] private Parallax a4Background;
	[SerializeField] private  List<GameObject> a4Fog;

	[SerializeField, Header("Area 5")] private List<Collider2D> a5Colliders;
	[SerializeField] private Lantern a5Lantern;
	[SerializeField] private GameObject a5Camera;
	[SerializeField] private Transform a5Spawn;
	[SerializeField] private Parallax a5Background;
	[SerializeField] private  List<GameObject> a5Fog;
	[SerializeField, Header("Area 6")] private List<Collider2D> a6Colliders;
	[SerializeField] private Lantern a6Lantern;
	[SerializeField] private GameObject a6Camera;
	[SerializeField] private Transform a6Spawn;
	[SerializeField] private Parallax a6Background;
	[SerializeField] private  List<GameObject> a6Fog;
	[SerializeField, Header("Area 7")] private List<Collider2D> a7Colliders;
	[SerializeField] private Lantern a7Lantern;
	[SerializeField] private GameObject a7Camera;
	[SerializeField] private Transform a7Spawn;
	[SerializeField] private Parallax a7Background;
	[SerializeField] private  List<GameObject> a7Fog;
	[SerializeField, Header("Area 8")] private List<Collider2D> a8Colliders;
	[SerializeField] private Lantern a8Lantern;
	[SerializeField] private GameObject a8Camera;
	[SerializeField] private Transform a8Spawn;
	[SerializeField] private Parallax a8Background;
	[SerializeField] private  List<GameObject> a8Fog;
	[SerializeField, Header("Area 9")] private List<Collider2D> a9Colliders;
	[SerializeField] private Lantern a9Lantern;
	[SerializeField] private GameObject a9Camera;
	[SerializeField] private Transform a9Spawn;
	[SerializeField] private Parallax a9Background;
	[SerializeField] private List<GameObject> a9Fog;

	[SerializeField] private SpriteRenderer spawnCoverBlock;

	private Dictionary<int, List<Collider2D>> collidersDict = new Dictionary<int, List<Collider2D>>();
	private Dictionary<int, Lantern> lanternsDict = new Dictionary<int, Lantern>();
	private Dictionary<int, GameObject> camerasDict = new Dictionary<int, GameObject>();
	private Dictionary<int, Transform> spawnsDict = new Dictionary<int, Transform>();
	private Dictionary<int, List<GameObject>> fogsDict = new Dictionary<int, List<GameObject>>();
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

		//Add fogs to dictionary
		fogsDict.Add(0, a0Fog);
		fogsDict.Add(1, a1Fog);
		fogsDict.Add(2, a2Fog);
		fogsDict.Add(3, a3Fog);
		fogsDict.Add(4, a4Fog);
		fogsDict.Add(5, a5Fog);
		fogsDict.Add(6, a6Fog);
		fogsDict.Add(7, a7Fog);
		fogsDict.Add(8, a8Fog);
		fogsDict.Add(9, a9Fog);

		currentArea = 0;

	}

    private void Start()
    {
		StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeA"));
	}

	public bool AddSoulToLantern()
	{
		// Check if area exists in dictionary
		if (lanternsDict.ContainsKey(currentArea))
		{
			AudioSystem.Instance.PlaySFX("LanternCollected");

			// Add souls and check if filled
			if (lanternsDict[currentArea].AddSoul())
			{
				// Filled
				ClearCurrentArea();
				ClearFog();

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
		if(!lanternsDict[newArea].AreaCleared()){
			foreach (GameObject GO in fogsDict[newArea])
			{
				GO.SetActive(true);
			}
		}

		CheckExceptions(newArea);
		//Enable new camera and disable old one
		if (backgroundDict[newArea] != backgroundDict[currentArea])
        {
            backgroundDict[currentArea]?.DeactivateParallax();
			backgroundDict[newArea]?.EnableParallax(newArea);
		}

		if (camerasDict[newArea] != camerasDict[currentArea])
		{
			camerasDict[newArea].SetActive(true);
			camerasDict[currentArea].SetActive(false);
		}

		currentArea = newArea;
		ChangeAreaMusic(newArea);

	}

	private void ChangeAreaMusic(int area)
    {
		switch (area)
		{
			case 0:
			case 1:
			case 2:
				StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeA"));

				break;

			case 3:
			case 4:
				StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeB"));

				break;

			case 5:
			case 6:
				StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeC"));

				break;

			case 7:
			case 8:
				if (GetCurrentLantern().AreaCleared())
				{
					StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeE"));
				}
				else
                {
					AudioSystem.Instance.PlayThemeDIntro();

				}

				break;
			case 9:
				StartCoroutine(AudioSystem.Instance.ChangeMusic("ThemeE"));
				break;
		}
	}

	public void ClearCurrentArea()
	{
		//Disable all colliders
		foreach (Collider2D coll in collidersDict[currentArea])
		{
			coll.enabled = false;
		}

		AudioSystem.Instance.PlaySFX("LanternCompleted");
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
	private void ClearFog()
	{
		foreach (GameObject GO in fogsDict[currentArea])
		{
			StartCoroutine(Fade(GO));
		}

	}
    public IEnumerator Fade(GameObject GO)
    {
		GO.GetComponent<Collider2D>().enabled = false;
        SpriteRenderer sr = GO.GetComponent<SpriteRenderer>(); // get the sprite renderer component
        float startAlpha = sr.color.a; // get the starting alpha value
		float fadeTime = 2.0f;
        float t = 0f; // time passed

        while (t < fadeTime)
        {
            t += Time.deltaTime; // increment time passed
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeTime); // calculate new alpha value
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha); // set new alpha value
            yield return null; // wait for next frame
        }
		GO.SetActive(false);
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

		if ((currentArea == 7 || currentArea == 8) && GetCurrentLantern().AreaCleared())
        {
			CinemachineVirtualCamera cinemachineVirtualCamera = camerasDict[newArea].GetComponent<CinemachineVirtualCamera>();
			cinemachineVirtualCamera.Follow = FindObjectOfType<PlayerLauncher>().transform;
			if (cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>() == null)
				cinemachineVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
        }
	}


}
