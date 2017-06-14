﻿using UnityEngine;
using System.Collections;

public class throwTrash : MonoBehaviour
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

    GameObject compost;
    GameObject landfill;
    GameObject recycle;

    /*
    Animator compostanim;
    Animator landfillanim;
    Animator recycleanim;
    */

    void Start()
    {
        moveByBelt = true;
        moveBySwipe = false;
        startCounting = false;
        time = 0;
        //starts idle animations
        compost = GameObject.Find("composite bin");
        //compostanim = compost.GetComponent<Animator> ();

        landfill = GameObject.Find("landfill bin");
        //landfillanim = landfill.GetComponent<Animator> ();

        recycle = GameObject.Find("recycle bin");
        //recycleanim = recycle.GetComponent<Animator> ();
    }

    void Update()
    {
        if (moveByBelt)
            transform.Translate(Vector3.down * difficultySettings.moveSpeed);
        if (moveBySwipe)
            transform.Translate(distance2 * .1f);
        timeOut(destroyTime);
    }


    void OnMouseDown()
    {
        lastMousePosition = Input.mousePosition;
    }


    void OnMouseUp()
    {
        // disable collider so player cannot swipe twice

        moveByBelt = false;
        newMousePosition = Input.mousePosition;
        distance = newMousePosition - lastMousePosition;

        // making sure that x and y values are not 0
        if (Mathf.Abs(distance.x) < 0.1f)
            distance.x = 0.1f;
        if (Mathf.Abs(distance.y) < 0.1f)
            distance.y = 0.1f;

        float xsquare = distance.x * distance.x;
        float ysquare = distance.y * distance.y;
        distance2 = distance / (Mathf.Sqrt(xsquare + ysquare));

        /*
		rb = GetComponent<Rigidbody2D> ();
		rb.isKinematic = false;
		rb.AddForce (distance2 * multiplier);
        */

        moveBySwipe = true;
        startCounting = true;
    }

    private void timeOut(float timer)
    {
        //bool exist = false;

        if (startCounting)
            time += Time.deltaTime;
        if (time > timer)
        {
            /*
            foreach (string items in difficultySettings.failedRecord)
            {
                if (items.Equals(gameObject.name.Substring(0, gameObject.name.Length - 7)))
                    exist = true;
            }
            if (!exist)
			    difficultySettings.failedRecord.Add (gameObject.name.Substring(0, gameObject.name.Length - 7));
                */
            difficultySettings.landfillCounter++;
            Destroy(gameObject);
        }
    }

    /*
    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objectPosition;
    } 
    */


    // bin collisions
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == gameObject.tag)
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
        }
        else if (coll.gameObject.tag == "landfill" & !difficultySettings.isTutorial)
        {
            difficultySettings.landfillCounter++;
            Destroy(gameObject);
        }
    }
}