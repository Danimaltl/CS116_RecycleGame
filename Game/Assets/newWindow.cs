﻿using UnityEngine;
using System.Collections;

public class newWindow : MonoBehaviour {

	public string game; 
	public string SignIn; 
	public string Profile; 
	public string Main;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeToGame(){
		Application.LoadLevel (game);	// load the game Level
	}

	public void changeSignIn(){
		Application.LoadLevel (SignIn);	// load the game Level
	}

	public void changeProfile(){
		Application.LoadLevel (Profile);	// load the game Level
	}

	public void changeMain(){
		Application.LoadLevel (Main);	// load the game Level
	}

}
