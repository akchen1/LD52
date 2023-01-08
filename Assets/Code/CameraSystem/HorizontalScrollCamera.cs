using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HorizontalScrollCamera : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float scrollMargin;
	[SerializeField] private float minX;
	[SerializeField] private float maxX;

	private GameObject player;
	private float screenHalfHeight;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		screenHalfHeight = virtualCamera.m_Lens.OrthographicSize;
	}

	// Update is called once per frame
	void Update()
	{
		float playerX = Camera.main.WorldToScreenPoint(player.transform.position).x;
		float cameraX = Camera.main.WorldToScreenPoint(virtualCamera.transform.position).x;

		if (playerX > (cameraX + screenHalfHeight - scrollMargin))
		{
			virtualCamera.transform.position += Vector3.right * Time.deltaTime;
		}
		else if (playerX < (cameraX - screenHalfHeight + scrollMargin))
		{
			virtualCamera.transform.position -= Vector3.right * Time.deltaTime;
		}
	}
}
