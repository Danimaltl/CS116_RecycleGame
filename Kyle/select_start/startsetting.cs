using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startsetting : MonoBehaviour
{
    public difficultySettings settings;
    // Use this for initialization
    void Start()
    {
        //int goal, float gainGla, float gainPla, float gainMet, float gainPaper, float gainComp,
        //float drop, float speed, int limit, float gap
        settings.setDifficulty(10, 1.0f, 1.2f, 1.5f, 0.7f, 0.5f, .011f, .05f, 10, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
