using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bin_controller : MonoBehaviour {

	float currentMood = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//  Make sure the mood is a legal mood, to be safe
		currentMood = capMood (currentMood);

	}

	public int capMood( float rawMood ){
		//  return an int capped at 0 and 10
		return (int)Mathf.Round(Mathf.Max(0, Mathf.Min(10, rawMood)));
	}
}
