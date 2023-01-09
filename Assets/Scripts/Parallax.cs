using UnityEngine;
using System.Linq;

public class Parallax : MonoBehaviour
{
    public Transform background;
    public Camera mainCamera;
    [SerializeField] private int[] AreaIds;
    private Vector2 offset;
    [SerializeField] private bool parallaxActive;
    [SerializeField] private bool isX;
    [SerializeField] private bool isY;
    private void Start()
    {
        if (parallaxActive)
            offset = background.position - mainCamera.transform.position;

    }

    private void Update()
    {
        if (!parallaxActive) return;
        Vector2 position = background.position;
        if (isX)
        {
            position.x = mainCamera.transform.position.x + offset.x;
            
        }
        if (isY)
            position.y = mainCamera.transform.position.y + offset.y;

        background.position = position;
    }

    public void EnableParallax(int area)
    {
        if (!IsMyArea(area)) return;
        parallaxActive = true;
        offset = background.position - mainCamera.transform.position;
    }

    public void DeactivateParallax(int area)
    {
        if (!IsMyArea(area)) return;
        parallaxActive = false;
    }

    private bool IsMyArea(int area)
    {
        if (AreaIds.Select(x => x == area) != null)
        {
            return true;
        }
        return false;
    }
}
