using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficultySettings : MonoBehaviour
{
    public static float timer;
    public static bool isStarted;
    public static bool isCompleted;
    public static bool gameOvered;
    public static bool isTutorial;
    public static int score; // lives/pollution bar
    public static int landfillCounter;
    public static float barGainRatePlastic;
    public static float barGainRateMetal;
    public static float barGainRateGlass;
    public static float barGainRatePaper;
    public static float barGainRateCompost;
    public static float barDropRate;
    public static float moveSpeed;
    public static int levelGoal;
    public static int landfillLimit; // chances before failing endless mode
    public static float spawnGap;
    public static float digestionTime_rec;
    public static float digestionTime_com;
    public static List<string> playRecord;
    public static List<string> failedRecord;
    public static int levelCounter = 1;
    public static int tutorialLevelCounter;

    // Use this for initialization
    void Start()
    {
        isStarted = false;
        isCompleted = false;
        gameOvered = false;
        isTutorial = false;
        score = 0;
        landfillCounter = 0;
        playRecord = new List<string>();
        failedRecord = new List<string>();
        //setDifficulty(10, 1, .01f, .05f, 10);
        print("game start: level goal: " + levelGoal + "    gainRateGlass: " + barGainRateGlass +
            "    gainRatePlastic: " + barGainRatePlastic + "    gainRateMetal: " + barGainRateMetal +
            "    gainRateCompost: " + barGainRateCompost + "    gainRatePaper: " + barGainRatePaper +
            "    dropRate" + barDropRate
            + "    speed: " + moveSpeed + "    limit: " + landfillLimit);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
            timer += Time.deltaTime;
        //print(timer);


        //lever goal
        if (score == levelGoal)
            isCompleted = true;


        //game over
        if (landfillCounter >= landfillLimit)
            gameOvered = true;
    }

    public void setDifficulty(int goal, float gainGla, float gainPla, float gainMet, float gainPaper, float gainComp,
    float drop, float speed, int limit, float gap)
    {
        setGoal(goal);
        setGainRate(gainGla, gainPla, gainMet, gainPaper, gainComp);
        setDropRate(drop);
        setSpeed(speed);
        setLimit(limit);
        setSpawnGap(gap);
    }

    private void setGoal(int goal) { levelGoal = goal; }
    private void setGainRate(float rateGlass, float ratePlastic, float rateMetal, float ratePaper, float rateCompost)
    {
        barGainRateGlass = rateGlass; barGainRatePlastic = ratePlastic; barGainRateMetal = rateMetal;
        barGainRatePaper = ratePaper; barGainRateCompost = rateCompost;
    }
    private void setDropRate(float rate) { barDropRate = rate; }
    private void setSpeed(float speed) { moveSpeed = speed; }
    private void setLimit(int limit) { landfillLimit = limit; }
    private void setSpawnGap(float gap) { spawnGap = gap; }

}
