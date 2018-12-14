using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreHUD : MonoBehaviour
{
	public string scoreTextPreamble = "PAACJS";
	public string highscoreTextPreamble = "BEST";
	public Text scoreText;
	public Text highscoreText;

	void WriteScore(int score)
	{
		scoreText.text = scoreTextPreamble + " " + score.ToString("0000");
	}

	public void PrintHighscore()
	{
		highscoreText.text = highscoreTextPreamble + " " + PlayerPrefs.GetInt("highscore", 0).ToString("0000");
	}

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
		GameController.RegisterOnPointUp(WriteScore);
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
		GameController.UnregisterOnPointUp(WriteScore);
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		PrintHighscore();
	}
}
