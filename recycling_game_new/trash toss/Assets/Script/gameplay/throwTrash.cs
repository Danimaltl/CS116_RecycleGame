﻿using UnityEngine;
using System.Collections;

public class throwTrash : lerpable
{
	public float digestionTime;
	private int destroyTime = 3;

    //DO NOT TOUCH THESE THEY ARE USED FOR BARSCRIPT TO GET THE INFO NECESSARY
    public static bool correctCollision = false;
    public static GameObject tagHolder;
    // You can do whatever to these
    private Vector3 lastMousePosition;
	private Vector3 newMousePosition;
	private Vector2 distance;
	private Vector3 distance2;
	private Rigidbody2D rb;
	private bool moveByBelt;
	private bool moveBySwipe;
	private bool startCounting;
	private float time;
    GameObject temp;

    //  Not thrown for now.
    private GameObject throwingTarget = null; 
	//  Pointers to the target objects
	GameObject compost;
	GameObject landfill;
	GameObject recycle;
	GameObject otherBin;


	public override void Start()
	{
		base.Start();
        //  Reset these variables every step
        moveByBelt = true; //  move the object down if true, basically
		moveBySwipe = false; //  set to true after the finger is released
		startCounting = false; //  countdown til automatated item destruction when true
		time = 0; //  presumably the time passed since the counter was activated
		//starts idle animations
		compost = GameObject.Find("composite bin");
		//compostanim = compost.GetComponent<Animator> ();

		landfill = GameObject.Find("landfill bin");
		//landfillanim = landfill.GetComponent<Animator> ();

		recycle = GameObject.Find("recycle bin");
		//recycleanim = recycle.GetComponent<Animator> ();

		otherBin = GameObject.Find("other bin");
	}


	public override void Update()
	{
		base.Update();
		//  This is the shared update cycle of all throwable trash objects
		if (isLerping ()) {
			throwingTarget.GetComponent<bin_controller> ().anticipatingBad = false;
			throwingTarget.GetComponent<bin_controller> ().anticipatingGood = false;
			//  Do nothing in terms of physics.
			//  Let the lerp handle it.
			if (!matchesBin (throwingTarget)) {
				//  Uh oh! The finger has realeased the trash and it's going to the wrong bin!
				//  Make the bin wince
				throwingTarget.GetComponent<bin_controller> ().anticipatingBad = true;
			} else {
				//  make the bin excited to get a good trash
				throwingTarget.GetComponent<bin_controller> ().anticipatingGood = true;
			}
		} else if (moveBySwipe) {
			//  The buffer is the drag distance that is tolerated before anything happens
			float distanceBuffer = 0.2f;
			float horizontalSensitivity = 0.2f;
			//  Presumably distance2 contains the direction of the swipe, 
			//  and the decimal controls the speed.
			//transform.Translate (distance2 * .1f);
			GameObject closestBin = raycastForBin(this.gameObject.transform.position, distance2.normalized);
			if (closestBin != null) {
				throwAt (closestBin);
			} else {
				//  Neither swiped up nor down. Do neither
				moveByBelt = true;
				moveBySwipe = false;
			}
			//  Convert the drag vector into a discrete direction
			/*
			if (Mathf.Abs (distance2.x) > horizontalSensitivity) {
				//  horizontal > vertical
				if (distance2.x > distanceBuffer) {
					//right
					throwAt(compost);
				} else if (distance2.x < -distanceBuffer) {
					//left
					throwAt(recycle);
				}
			} else {
				//  v > h
				if (distance2.y > distanceBuffer) {
					//down
					throwAt (otherBin);
				} else if (distance2.y < -distanceBuffer) {
					//up
					throwAt (landfill);
				} else {
					//  Neither swiped up nor down. Do neither
					moveByBelt = true;
					moveBySwipe = false;
				}
			}
			*/

		} else if (moveByBelt) {
			//  Literally move the item downward if it is on the belt
			transform.Translate (Vector3.down * difficultySettings.moveSpeed);
		} 
		//  Check to see if the object should be destroyed yet
		timeOut(destroyTime);
	}

	public void throwAt(GameObject targetObj){
		//  Throws the current trash object at the specified gameObject
		setLerpTarget(targetObj.transform.position);
		throwingTarget = targetObj;
	}

	public override void lerpTargetReached(){
		base.lerpTargetReached (); //  turns the lerp mechanic off
		//  Now activate the throwing target as if there was a collision.
		if (null != throwingTarget) {
			checkForGoal (throwingTarget);
			//  and then clean up
			throwingTarget = null;
		}
	}

	void OnMouseDown()
	{
		lastMousePosition = Input.mousePosition;
	}


	void OnMouseUp()
	{
		// disable collider so player cannot swipe twice (so much for that)

		moveByBelt = false;
		newMousePosition = Input.mousePosition;
		//  If just distance is used, the objects move incredibly fast
		//  but players control speed of object
		distance = newMousePosition - lastMousePosition;

		float xsquare = distance.x * distance.x;
		float ysquare = distance.y * distance.y;
		//  so dist2 extracts just direction
		distance2 = distance / distance.magnitude;

		//  speed of the throw proportional to the finger drag distance
		float minimumSpeed = 7f;
		speed = Mathf.Max(minimumSpeed, distance.magnitude/10f);

		moveBySwipe = true;
		startCounting = true;
	}


	private void timeOut(float timer)
	{
		//  Throw the object in the landfill after the item counter diminishes
		//bool exist = false;

		if (startCounting)
			time += Time.deltaTime;
		if (time > timer)
		{
			difficultySettings.landfillCounter++;
			Destroy(gameObject);
		}
	}


	// bin collisions
	void OnTriggerEnter2D(Collider2D coll)
	{
		checkForGoal(coll.gameObject);
	}

	public bool matchesBin(GameObject bin){
        return bin.tag == gameObject.tag || isRecyclable(this.gameObject) && bin.tag == "recycle";
	}

	public bool isRecyclable( GameObject trashObject){
		return  trashObject.tag == "Plastic" || trashObject.tag == "Paper" ||
				trashObject.tag == "Metal" || trashObject.tag == "Glass";
	}

	//  bin collisions
	public bool checkForGoal(GameObject other){
		//  checks for if the current trash scored a point and performs the following logic if so.
		//  returns true on success
		correctCollision = false;
		temp = gameObject;

		// check if this is tutorial and is being used with arrows
		if (other.tag != "spawnSpot") {
			
			//otherwise its recycle and create a temp to store tag
			if (isRecyclable (this.gameObject)) {
				temp = (GameObject)Instantiate (gameObject);
				//print("Before Change  " + gameObject.tag);
				temp.tag = "recycle";
				//print("After Change  " + gameObject.tag);
				//print (temp.tag);
			}
			if (matchesBin (other)) {
				difficultySettings.score += 1;
				difficultySettings.playRecord.Add (gameObject.name.Substring (0, gameObject.name.Length - 7));
				//if (gameObject.tag == "recycle") {
				//	difficultySettings.digestionTime_rec = digestionTime;
				//}
				if (gameObject.tag == "composite") {
					print (difficultySettings.score + " Composite");
					difficultySettings.digestionTime_com = digestionTime;
					//tagHolder = gameObject;
					if (!difficultySettings.isTutorial) { 
						tagHolder = (GameObject)Instantiate (gameObject);
					}
					correctCollision = true;
				} else if (gameObject.tag == "recycle" || temp.tag == "recycle") {
					difficultySettings.digestionTime_rec = digestionTime;
					if (!difficultySettings.isTutorial) {
						tagHolder = (GameObject)Instantiate (gameObject);
					}
					//tagHolder = gameObject;
					correctCollision = true;
				}
				Destroy (gameObject);
				Destroy (temp);
				//print (gameObject);
				//print (difficultySettings.score);
				other.GetComponent<bin_controller> ().animateCorrect ();
				return true;
			}
       /* else if (other.tag == temp.tag)
        {
            print(difficultySettings.score);
            difficultySettings.score += 1;
            difficultySettings.playRecord.Add(gameObject.name.Substring(0, gameObject.name.Length - 7));
            if (gameObject.tag == "recycle" || temp.tag == "recycle")
            {
                difficultySettings.digestionTime_rec = digestionTime;
                if (!difficultySettings.isTutorial)
                {
                    tagHolder = (GameObject)Instantiate(gameObject);
                }
                //tagHolder = gameObject;
                correctCollision = true;
            }
            Destroy(gameObject);
            Destroy(temp);
            //print(difficultySettings.score + " SCORE");
            print(other.GetComponent<bin_controller>());
            other.GetComponent<bin_controller>().animateCorrect();
            return true;
        }*/

        else {
				//  Increment penalty
				difficultySettings.landfillCounter++;
				//  Give feedback for the correct bin
				flashCorrectBin ();
				//  Destroy in all cases, regardless of success
				Destroy (gameObject); //  added
				Destroy (temp);
				other.GetComponent<bin_controller> ().animateIncorrect ();
				return false;
			}

		}
		return false;
	}

	public void flashCorrectBin(){
		//  Animates the arrow over the bin that the trash belongs to
		int answerFlashDuration = 36;
		if (matchesBin (compost)) {
			compost.gameObject.GetComponent<bin_controller> ().flashArowTimer = answerFlashDuration;
		} else if (matchesBin (landfill)) {
			landfill.gameObject.GetComponent<bin_controller> ().flashArowTimer = answerFlashDuration;
		} else if (matchesBin (recycle)) {
			recycle.gameObject.GetComponent<bin_controller> ().flashArowTimer = answerFlashDuration;
		} else if (matchesBin (otherBin)) {
			otherBin.gameObject.GetComponent<bin_controller> ().flashArowTimer = answerFlashDuration;
		}
	}

	public GameObject raycastForBin(Vector3 origin, Vector3 direction){
		//  Return the closest bin to any point in the line provided
		GameObject closestBin = null;
		float closestDistanceToCheckpoint = 99999;

		int rayIncrement = 3;
		for (int rayDistance = 0; rayDistance < 100; rayDistance += rayIncrement){
			Vector3 pointToCheck = origin + direction * rayDistance; //  Travel incrementally along this direction as loop continues
			//  Go through all the bins and find the closest one to this point
			float distanceFromBinToRayPoint = Vector3.Distance(recycle.gameObject.transform.position , pointToCheck);
			if (distanceFromBinToRayPoint < closestDistanceToCheckpoint) {
				//  New record found
				closestBin = recycle;
				closestDistanceToCheckpoint = distanceFromBinToRayPoint;
			}
			distanceFromBinToRayPoint =  Vector3.Distance(compost.gameObject.transform.position , pointToCheck);
			if (distanceFromBinToRayPoint < closestDistanceToCheckpoint) {
				//  New record found
				closestBin = compost;
				closestDistanceToCheckpoint = distanceFromBinToRayPoint;
			}
			distanceFromBinToRayPoint =  Vector3.Distance(landfill.gameObject.transform.position , pointToCheck);
			if (distanceFromBinToRayPoint < closestDistanceToCheckpoint) {
				//  New record found
				closestBin = landfill;
				closestDistanceToCheckpoint = distanceFromBinToRayPoint;
			}
			distanceFromBinToRayPoint =  Vector3.Distance(otherBin.gameObject.transform.position , pointToCheck);
			if (distanceFromBinToRayPoint < closestDistanceToCheckpoint) {
				//  New record found
				closestBin = otherBin;
				closestDistanceToCheckpoint = distanceFromBinToRayPoint;
			}
		}
		return closestBin;
	}

}