using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectLevel : MonoBehaviour
{
    public difficultySettings settings;
    public Button level1;
	public Button level2;
	public Button level3;
	public Button level4;
	public Button level5;


	// Use this for initialization
	void Start ()
    {
        Button lvl1 = level1.GetComponent<Button>();
        lvl1.onClick.AddListener(selectLevel1);
		Button lvl2 = level2.GetComponent<Button>();
		lvl2.onClick.AddListener(selectLevel2);
		Button lvl3 = level3.GetComponent<Button>();
		lvl3.onClick.AddListener(selectLevel3);
		Button lvl4 = level4.GetComponent<Button>();
		lvl4.onClick.AddListener(selectLevel4);
		Button lvl5 = level5.GetComponent<Button>();
		lvl5.onClick.AddListener(selectLevel5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void selectLevel1()
    {
        settings.setDifficulty(5, 0, 0, 0, 0, 0, 0, 0, 100, 2f);
        difficultySettings.tutorialLevelCounter = 1;
		levelManager.LoadNextScene();
    }

	void selectLevel2()
	{
		settings.setDifficulty(8, 0, 0, 0, 0, 0, 0, 0, 100, 2f);
        difficultySettings.tutorialLevelCounter = 2;
        levelManager.LoadNextScene();
	}

	void selectLevel3()
	{
		settings.setDifficulty(11, 0, 0, 0, 0, 0, 0, 0, 100, 2f);
        difficultySettings.tutorialLevelCounter = 3;
        levelManager.LoadNextScene();
	}

	void selectLevel4()
	{
		settings.setDifficulty(14, 0, 0, 0, 0, 0, 0, 0, 100, 2f);
        difficultySettings.tutorialLevelCounter = 4;
        levelManager.LoadNextScene();
	}

	void selectLevel5()
	{
		settings.setDifficulty(17, 0, 0, 0, 0, 0, 0, 0, 100, 2f);
        difficultySettings.tutorialLevelCounter = 5;
        levelManager.LoadNextScene();
	}
}
