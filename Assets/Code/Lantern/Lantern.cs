using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
	public int MaxSouls;
	public int CurrentSouls;
	public int Area;

	// Start is called before the first frame update
	void Start()
	{
		// Assign default values
		CurrentSouls = 0;
	}

	public bool AddSouls(int amount)
	{
		// Add souls
		CurrentSouls += amount;

		// Check if lantern is filled
		if (CurrentSouls >= MaxSouls)
		{
			// Filled
			return true;
		}

		// Not filled
		return false;
	}
}
