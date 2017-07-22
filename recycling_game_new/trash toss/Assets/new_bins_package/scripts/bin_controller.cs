using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bin_controller : MonoBehaviour {
	//  This class control the visual logic of the bin
	//  Without interfering with mechanics
	private const float DEFAULT_LID_POSITION = 0.675f;

	public float normalizedBreath = 1f;
	private float currentMood = 10f;
	private float lidPosition = 1f;
	private float lidSpeed = 0f;

	private bool isTouchingBadTrash = false;
	public bool anticipatingBad = false;
	public bool anticipatingGood = false;
	private float startedBreathing;



	// Use this for initialization
	void Start () {
		startedBreathing = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		//  Make sure the mood is a legal mood, to be safe
		currentMood = capMood (currentMood);

		//  keep breathing 
		float breathGoal = Mathf.Sin(Time.time - startedBreathing);//*.5f + 1.25f;
		float breathSpeed = 0.01f;
		if (normalizedBreath > breathGoal) {
			normalizedBreath -= breathSpeed;
		} else {
			normalizedBreath += breathSpeed;
		}
		//  Move the upper body up and down with the breath
		this.gameObject.transform.GetChild (0).transform.position = new Vector3(this.gameObject.transform.GetChild (0).transform.position.x, 
																				-normalizedBreath/4  -0.925f, 
																				this.gameObject.transform.GetChild (0).transform.position.z);

		//  Random mood swings (for testing)
		int testFrequency = 200; //  bigger number = fewer blinks
		if (Random.Range(0, testFrequency) <= 1) {
			animateCorrect();
		}
		if (Random.Range(0, testFrequency) <= 1) {
			animateIncorrect();
		}

		//  Make some objects invisible
		controlVisibility ();

		//  Swing lid back towards the default position
		float lidAccel = 0;
		float accelerationMagnitude = 0.01f;
		//  Determine lid accel direction
		if (lidPosition > DEFAULT_LID_POSITION) {
			//  Less force closing because shorter distance. 
			lidAccel = -accelerationMagnitude/4;
		}
		if (lidPosition < DEFAULT_LID_POSITION) {
			lidAccel = accelerationMagnitude;
		}
		lidSpeed += lidAccel; //  Add acceleration to speed
		lidSpeed = lidSpeed * .93f; //  Friction
		lidPosition =  Mathf.Max(0f, Mathf.Min(1f, lidSpeed + lidPosition)); //  add speed to position

		if (anticipatingBad) {
			lidPosition = 0;
			normalizedBreath = 1f;
		} else if (anticipatingGood) {
			normalizedBreath = 0f;
		}
		anticipatingBad = false;
		anticipatingGood = false;
	}

	private void controlVisibility(){
		if (isTouchingBadTrash) {
			//  Turn on normal body
			GetComponent<Renderer>().enabled = false;
			this.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
			this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = false;
			//  Turn on the special squished sprite
			this.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
			normalizedBreath = 1f;
		} else {
			//  Turn on normal body
			GetComponent<Renderer>().enabled = true;
			this.gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
			this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = true;
			//  turn off squished sprite
			this.gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
		}
		//  Now reset the variable after reading every step
		isTouchingBadTrash = false;
	}

	public int capMood( float rawMood ){
		//  return an int capped at 0 and 10
		return (int)Mathf.Round(Mathf.Max(0, Mathf.Min(10, rawMood)));
	}

	public void setMood(int newMood){
		currentMood = capMood(newMood);
	}

	public int getMood (){
		return capMood (currentMood);
	}

	public void setLid(float newLid){
		lidPosition = Mathf.Max(0f, Mathf.Min(1f, newLid));
	//	lidInMotion = true;
	}

	public float getLid (){
		return lidPosition;
	}

	//  The animate functions don't change scoring logic.
	public void animateCorrect(){
		//  Animate the bin as if this was the right bin to throw trash into.
		setLid(1f); //  open lid completely
		setMood(capMood(currentMood +1f)); //  Increase the mood
		normalizedBreath = 0f;
	}
	public void animateIncorrect(){
		setLid(0f);
		isTouchingBadTrash = true;
		//  Animate the bin as if a the wrong trash was just thrown in
		//  but don't mess with the game score. Aesthetic only
		setMood(capMood(currentMood -1f)); //  Decrease the mood
		//normalizedBreath = 1f;

	}
}
