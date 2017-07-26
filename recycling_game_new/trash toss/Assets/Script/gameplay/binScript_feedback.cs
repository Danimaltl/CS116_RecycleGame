using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binScript_feedback : MonoBehaviour {

	public AudioClip accept; // audio clip for "correct" sound
	public AudioClip reject; // audio clip for "incorrect" sound

	private AudioSource audioSource;
	private LineRenderer line;
	private float timer;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		line = GetComponent<LineRenderer>();
		timer = 0;
	}

	void Update() {
		if (timer < 0)
			timer = 0;
		
		if (timer != 0) {
			timer -= Time.deltaTime;
		} else {
			line.enabled = false;
		}
	}

	// when an item collides
	void OnTriggerEnter2D(Collider2D coll) {
		// if tag matches, or item's tag is recyclable and is put in the recycle bin
		if (coll.gameObject.tag == tag || coll.gameObject.GetComponent<throwTrash>().isRecyclable(coll.gameObject) && tag == "recycle") {
			audioSource.clip = accept; // set clip to "correct" sound
			timer = 1;
			line.enabled = true;
		} else { // item is rejected
			audioSource.clip = reject; // set clip to "incorrect" sound
		}
		audioSource.Stop();
		//  Added following line that fixes a bug where the last throw triggers an infinite sound loop on win screen.
		//  This says if the game is not over, play the sound
		if (!difficultySettings.gameOvered) {
			audioSource.Play ();
		}
	}
}
