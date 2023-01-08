using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VerticalScrollCamera : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private float scrollMargin;
	[SerializeField] private float minY;
	[SerializeField] private float maxY;

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
		float playerY = Camera.main.WorldToScreenPoint(player.transform.position).y;
		float cameraY = Camera.main.WorldToScreenPoint(virtualCamera.transform.position).y;

		if (playerY > (cameraY + screenHalfHeight - scrollMargin))
		{
			virtualCamera.transform.position += Vector3.up * Time.deltaTime;
		}
		else if (playerY < (cameraY - screenHalfHeight + scrollMargin))
		{
			virtualCamera.transform.position -= Vector3.up * Time.deltaTime;
		}
	}
}
