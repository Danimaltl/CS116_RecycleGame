using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binRenderer : MonoBehaviour {

    private LineRenderer[] lineRenderers;
    private string otherTag = "";

	// Use this for initialization
	void Start () {
        lineRenderers = GetComponentsInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        otherTag = gameObject.GetComponent<Collider2D>().tag;
        //otherTag = gameObject.tag;
        print(otherTag);
	}

    void OnTriggerEnter2D(Collider2D other) {

        otherTag = other.gameObject.tag;

        if (otherTag == "recycle") {
            lineRenderers[0].enabled = true;
            lineRenderers[1].enabled = false;
            lineRenderers[2].enabled = false;
        } else if (otherTag == "landfill") {
            lineRenderers[0].enabled = false;
            lineRenderers[1].enabled = true;
            lineRenderers[2].enabled = false;
        } else if (otherTag == "composite") {
            lineRenderers[0].enabled = false;
            lineRenderers[1].enabled = false;
            lineRenderers[2].enabled = true;
        } else {

        }

    }
}
