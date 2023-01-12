using UnityEngine;
using System.Linq;

public class Parallax : MonoBehaviour
{
    public Transform background;
    public GameObject mainCamera;
    public GameObject areaCamera;
    [SerializeField] private int[] AreaIds;
    [SerializeField] private bool[] isXs;
    [SerializeField] private bool[] isYs;

    private Vector2 offset;
    [SerializeField] private bool parallaxActive;
    [SerializeField] private bool isX;
    [SerializeField] private bool isY;
    bool isInArea;
    private int currentArea;
    private void Start()
    {
        if (parallaxActive)
            offset = background.position - mainCamera.transform.position;
        isInArea = false;
        //offset = background.position - areaCamera.transform.position;

    }

    private void Update()
    {
        if (!parallaxActive) return;

        Vector2 position = background.position;
        if (isXs[IsMyArea(currentArea)])
        {
            position.x = mainCamera.transform.position.x + offset.x;
            
        }
        if (isYs[IsMyArea(currentArea)])
            position.y = mainCamera.transform.position.y + offset.y;

        background.position = position;
    }

    public void EnableParallax(int area)
    {
        if (IsMyArea(area) < 0) return;
        parallaxActive = true;
        currentArea = area;
        
    }

    public void DeactivateParallax(int area)
    {
        parallaxActive = false;
        isInArea = false;
    }

    private int IsMyArea(int area)
    {
        int index = AreaIds.ToList().IndexOf(area);
        return index;
        //if (AreaIds.Select(x => x == area) != null)
        //{
        //    return true;
        //}
        //return false;
    }
}
