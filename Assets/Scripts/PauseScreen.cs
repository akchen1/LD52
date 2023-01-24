using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    private static PauseScreen _instance;
    public static PauseScreen Instance { get { return _instance; } }

	[SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	public bool IsPaused()
	{
		return pauseScreen.activeSelf;
	}
}
