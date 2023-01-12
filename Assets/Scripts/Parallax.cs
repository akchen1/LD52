using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Cinemachine;
public class Parallax : MonoBehaviour
{
    public Transform background;
    public GameObject mainCamera;
    [SerializeField] private int[] AreaIds;
    [SerializeField] private bool[] isXs;
    [SerializeField] private bool[] isYs;

    private Vector2 offset;
    [SerializeField] private bool parallaxActive;

    private int currentAreaIndex;
    CinemachineBrain CinemachineBrain;
    private void Start()
    {
        CinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    private void Update()
    {
        if (!parallaxActive) return;

        Vector2 position = background.position;
        if (isXs[currentAreaIndex])
        {
            position.x = mainCamera.transform.position.x + offset.x;
            
        }
        if (isYs[currentAreaIndex])
            position.y = mainCamera.transform.position.y + offset.y;

        background.position = position;
    }

    public void EnableParallax(int area)
    {
        if (GetAreaIndex(area) < 0) return;
        StartCoroutine(WaitForTransition());
        currentAreaIndex = GetAreaIndex(area);
        
    }

    public void DeactivateParallax()
    {
        parallaxActive = false;
    }

    private int GetAreaIndex(int area)
    {
        return AreaIds.ToList().IndexOf(area);
    }

    private IEnumerator WaitForTransition()
    {
        yield return 0; // wait for next frame for cinemachine to update
        yield return new WaitUntil(() => !CinemachineBrain.IsBlending);
        parallaxActive = true;
    }
}
