﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	public string highscoreTextPreamble = "BEST";
	public Text highscoreText;

	void Start()
	{
		highscoreText.text = highscoreText.text = highscoreTextPreamble + " " + PlayerPrefs.GetInt("highscore", 0).ToString("0000");
	}

	public void Play()
	{
		SceneManager.LoadScene("Gameplay");
	}
}
