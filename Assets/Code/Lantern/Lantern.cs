using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
	public int MaxSouls;
	public int CurrentSouls;
	public int Area;

	public GameObject IndicatorFrame;

	public List<GameObject> Indicators;


	// Start is called before the first frame update
	void Start()
	{
		if(MaxSouls == 2)
		{
			Indicators[0].SetActive(false);
			Indicators.RemoveAt(0);
		}
		else if(MaxSouls == 1)
		{
			Indicators[2].SetActive(false);
			Indicators.RemoveAt(2);
			Indicators[1].SetActive(false);
			Indicators.RemoveAt(1);
		}
		// Assign default values
		CurrentSouls = 0;
	}
	public bool AddSoul()
	{
		// Add souls
		if(CurrentSouls < MaxSouls){
			FillSoul();
			CurrentSouls += 1;
		}
		// Check if lantern is filled
		if (CurrentSouls >= MaxSouls)
		{
			// Filled
			IgniteLantern();
			return true;
		}

		// Not filled
		return false;
	}
	private void FillSoul()
	{
		Indicators[CurrentSouls].GetComponent<Animator>().enabled = true;
	}

	private void IgniteLantern(){
		IndicatorFrame.SetActive(false);
		foreach(GameObject go in Indicators){
			go.SetActive(false);
		}
		gameObject.GetComponent<Animator>().enabled = true;
	}

	public bool AreaCleared()
	{
		return CurrentSouls >= MaxSouls;
	}



}
