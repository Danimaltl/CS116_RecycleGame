using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pollution_Bar_Script : MonoBehaviour {

	[SerializeField]
	private Image content;
	private int landfillCount;
	private int limit;

	// Use this for initialization
	void Start () {
		limit = difficultySettings.landfillLimit;
		content.fillAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		landfillCount = difficultySettings.landfillCounter;
		content.fillAmount = (float) (((float)landfillCount) / limit);
	}
}
