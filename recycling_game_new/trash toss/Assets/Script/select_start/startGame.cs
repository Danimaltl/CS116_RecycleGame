using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class startGame : MonoBehaviour
{
    public Button startButton;
    public Button completeButton;
    public Button Title;
	public Animator ani;
	public float dropRate;
	public float speed;
	public float gap;
	//public Button zbc;

    void Start()
    {
        Button start = startButton.GetComponent<Button>();
        start.onClick.AddListener(startOnClick);
        Button complete = completeButton.GetComponent<Button>();
        complete.onClick.AddListener(completeOnClick);
        Button title = Title.GetComponent<Button>();
        title.onClick.AddListener(goTitle);
		//Button test = zbc.GetComponent<Button> ();
		//test.onClick.AddListener (change);
    }

    void Update()
    {
        if (difficultySettings.score == difficultySettings.levelGoal)
        {
            disable(transform.Find("other").gameObject);
            enable(transform.Find("Level complete").gameObject);
			ani.enabled = false;
            enable(completeButton.gameObject);

        }
        if (difficultySettings.gameOvered)
        {
            disable(transform.Find("other").gameObject);
            enable(transform.Find("game over").gameObject);
            enable(Title.gameObject);
        }
    }
    void startOnClick()
    {
        difficultySettings.isStarted = true;
        //transform.FindChild("game start").gameObject.SetActive(false);
        enable(transform.Find("other").gameObject);
        disable(startButton.gameObject);
		disable(transform.Find("Level Panel").gameObject);
    }

    void completeOnClick()
    {
		difficultySettings.levelGoal += 3;
		changeDifficulties ();
		difficultySettings.levelCounter++;
        disable(completeButton.gameObject);
        disable(transform.Find("Level complete").gameObject);
        levelManager.LoadPlayScene();
    }

    void goTitle()
    {
        levelManager.LoadTitleScene();
    }

	/*void change()
	{
		difficultySettings.spawnGap = .1f;
	}*/

    private void enable(GameObject obj) { obj.SetActive(true); }
    private void disable(GameObject obj) { obj.SetActive(false); }
	private void changeDifficulties()
	{
        difficultySettings.barDropRate = difficultySettings.barDropRate * dropRate;
        difficultySettings.barGainRatePlastic = difficultySettings.barGainRatePlastic * dropRate;
        difficultySettings.barGainRatePaper = difficultySettings.barGainRatePaper * dropRate;
        difficultySettings.barGainRateMetal = difficultySettings.barGainRateMetal * dropRate;
        difficultySettings.barGainRateCompost = difficultySettings.barGainRateCompost * dropRate;
        difficultySettings.barGainRateGlass = difficultySettings.barGainRateGlass * dropRate;
        difficultySettings.spawnGap = difficultySettings.spawnGap * gap;
        difficultySettings.moveSpeed = difficultySettings.moveSpeed * speed;
    }
}
