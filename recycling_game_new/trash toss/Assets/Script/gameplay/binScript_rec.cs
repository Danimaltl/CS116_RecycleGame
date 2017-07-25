using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class binScript_rec : MonoBehaviour
{

    public Shader grayscale;
    private Shader defaultShader;
    private Animator animationState;
    public static bool isDigesting = false;
    public static float timer = 0;

    void Start()
    {
        defaultShader = gameObject.GetComponent<Renderer>().material.shader;
        animationState = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDigesting == true)
        {
            gameObject.GetComponent<Renderer>().material.shader = grayscale;
            timer += Time.deltaTime;
            if (timer >= difficultySettings.digestionTime_rec)
            {
                gameObject.GetComponent<Renderer>().material.shader = defaultShader;
                animationState.SetInteger("State", 0);
                gameObject.GetComponent<Collider2D>().enabled = true;
                isDigesting = false;
                timer = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "recycle" || coll.gameObject.tag == "Paper" ||
            coll.gameObject.tag == "Plastic" || coll.gameObject.tag == "Glass" ||
            coll.gameObject.tag == "Metal")
        {
            animationState.SetInteger("State", 1); //switches idle to eating animation
            gameObject.GetComponent<Collider2D>().enabled = false;
            isDigesting = true;
        }
    }
}
