using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingVineCam : MonoBehaviour
{
	[SerializeField] private float camMoveSpeed;
	public bool IsMoving = false;
	public GameObject Vine;

	private Vector3 originPos;

	// Start is called before the first frame update
	void Start()
	{
		originPos = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (IsMoving && !PauseScreen.Instance.IsPaused())
		{
			transform.position += new Vector3(camMoveSpeed * Time.deltaTime, 0);
		}
	}

	public void ResetPosition()
	{
		IsMoving = false;
		transform.position = originPos;

		if (Vine != null)
		{
			Destroy(Vine);
		}
	}
}
