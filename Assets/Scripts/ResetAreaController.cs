using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetAreaController : MonoBehaviour
{
    [SerializeField] GameObject BirdPrefab;
    [SerializeField] GameObject SwingPlatformPrefab;
    [SerializeField] GameObject ChasingVinesPrefab;

    [SerializeField] List<GameObject> a0Objects;
    [SerializeField] List<GameObject> a1Objects;
    [SerializeField] List<GameObject> a2Objects;
    [SerializeField] List<GameObject> b1Objects;
    [SerializeField] List<GameObject> b2Objects;
    [SerializeField] List<GameObject> c1Objects;
    [SerializeField] List<GameObject> c2Objects;
    [SerializeField] List<GameObject> d1Objects;
    //[SerializeField] List<GameObject> d2Objects;
    [SerializeField] List<GameObject> e1Objects;

    [SerializeField] AreaObjects[] areaObjects;
    private Dictionary<int, List<GameObject>> spawnedObjects;
    
    private AreaManager areaManager;
    private PlayerLauncher player;
    void Start()
    {
        areaManager = FindObjectOfType<AreaManager>();
        player = FindObjectOfType<PlayerLauncher>();

        PopulateDictionary();
    }

    private void PopulateDictionary()
    {
        spawnedObjects = new Dictionary<int, List<GameObject>>();
        spawnedObjects.Add(0, a0Objects);
        spawnedObjects.Add(1, a1Objects);
        spawnedObjects.Add(2, a2Objects);
        spawnedObjects.Add(3, b1Objects);
        spawnedObjects.Add(4, b2Objects);
        spawnedObjects.Add(5, c1Objects);
        spawnedObjects.Add(6, c2Objects);
        spawnedObjects.Add(7, d1Objects);
        spawnedObjects.Add(8, d1Objects);
        spawnedObjects.Add(9, e1Objects);
    }

    public void SyncObjects()
    {
        PopulateDictionary();
        foreach (KeyValuePair<int, List<GameObject>> keyValuePair in spawnedObjects)
        {
            int area = keyValuePair.Key;
            List<GameObject> objects = keyValuePair.Value;

            AreaObjects savedArea = areaObjects[area];
            savedArea.objects.Clear();
            savedArea.positions.Clear();
            savedArea.scales.Clear();

            foreach (GameObject obj in objects)
            {
                GameObject prefab = null;
                bool hangingPlatfromDestroy = true;
                if (obj.GetComponent<BirdAIController>() != null)
                {
                    prefab = BirdPrefab;
                }
                else if (obj.GetComponentInChildren<RopePlatform>() != null)
                {
                    prefab = SwingPlatformPrefab;
                    hangingPlatfromDestroy = obj.GetComponentInChildren<RopePlatform>().isDestroy;
                } else if (obj.GetComponent<ChasingVineController>() != null)
                {
                    prefab = ChasingVinesPrefab;
                }
                savedArea.objects.Add(prefab);
                savedArea.positions.Add(obj.transform.position);
                savedArea.scales.Add(obj.transform.localScale);  // may be problem
                savedArea.hangingPlatformDestroy.Add(hangingPlatfromDestroy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetArea(true);
        }
    }

    public void ResetArea(bool resetPlayer)
    {
        Lantern lantern = areaManager.GetCurrentLantern();

        int areaIndex = areaManager.GetCurrentArea();

        bool onlyRocks = false;
        if (lantern != null)
        {
            if (lantern.AreaCleared())
            {
                onlyRocks = true;
            } else
            {
                lantern.ResetLantern();
            }
        }
        SpawnObjects(areaIndex, onlyRocks);
        if (resetPlayer)
            player.DieHard();
    }

    public void SpawnObjects(int areaIndex, bool rocksOnly)
    {
        foreach (GameObject obj in spawnedObjects[areaIndex])
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects[areaIndex].Clear();

        AreaObjects area = areaObjects[areaIndex];
        for (int i = 0; i < area.objects.Count; i++)
        {
            RopePlatform ropePlatform = area.objects[i].GetComponentInChildren<RopePlatform>();
            if (rocksOnly && ropePlatform == null) continue;
            GameObject spawnedObject = Instantiate(area.objects[i], area.positions[i], Quaternion.identity);
            spawnedObject.transform.localScale = area.scales[i];
            if (ropePlatform != null) ropePlatform.isDestroy = area.hangingPlatformDestroy[i];
            spawnedObjects[areaIndex].Add(spawnedObject);

        }
    }

    public AreaObjects[] GetAreaObjects()
    {
        return areaObjects;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
