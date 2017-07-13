﻿using UnityEngine;
using System.Collections;

public class throwTrash : lerpable
{
    public float digestionTime;
    private int destroyTime = 3;

    private Vector3 lastMousePosition;
    private Vector3 newMousePosition;
    private Vector2 distance;
    private Vector3 distance2;
    private Rigidbody2D rb;
    private bool moveByBelt;
    private bool moveBySwipe;
    private bool startCounting;
    private float time;

	//  Not thrown for now.
	private GameObject throwingTarget = null; 
	//  Pointers to the target objects
    GameObject compost;
    GameObject landfill;
    GameObject recycle;

    /*
    Animator compostanim;
    Animator landfillanim;
    Animator recycleanim;
    */

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
    }


    public override void Update()
    {
		base.Update();
	//  This is the shared update cycle of all throwable trash objects
		if (isLerping ()) {
			//  Do nothing in terms of physics.
			//  Let the lerp handle it.

		} else if (moveBySwipe) {
			//  Presumably distance2 contains the direction of the swipe, 
			//  and the decimal controls the speed.
			//transform.Translate (distance2 * .1f);
			throwAt(recycle);

		} else if (moveByBelt) {
			//  Literally move the item downward if it is on the belt
			transform.Translate (Vector3.down * difficultySettings.moveSpeed);
		} 
		//  What does this do?
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

        // making sure that x and y values are not 0
        if (Mathf.Abs(distance.x) < 0.1f)
            distance.x = 0.1f;
        if (Mathf.Abs(distance.y) < 0.1f)
            distance.y = 0.1f;

        float xsquare = distance.x * distance.x;
        float ysquare = distance.y * distance.y;
		//  so dist2 extracts just direction
        distance2 = distance / (Mathf.Sqrt(xsquare + ysquare));

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
		Debug.Log("Goal: " + checkForGoal(coll.gameObject));
    }

	//  bin collisions
	public bool checkForGoal(GameObject other){
	//  checks for if the current trash scored a point and performs the following logic if so.
	//  returns true on success
		if (other.tag == gameObject.tag)
		{
			difficultySettings.score += 1;
			difficultySettings.playRecord.Add(gameObject.name.Substring(0, gameObject.name.Length - 7));
			if (gameObject.tag == "recycle")
			{
				difficultySettings.digestionTime_rec = digestionTime;
			}

			if (gameObject.tag == "composite")
			{
				difficultySettings.digestionTime_com = digestionTime;
			}
			Destroy(gameObject);
			return true;
		}
		else if (other.tag == "landfill" & !difficultySettings.isTutorial)
		{
			difficultySettings.landfillCounter++;
			Destroy(gameObject);
			return true;
		}
		//  Destroy in all cases, regardless of success
		Destroy(gameObject); //  added
		return false;
	}


}