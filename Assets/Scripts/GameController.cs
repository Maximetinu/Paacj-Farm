using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : SingletonMonoBehaviour<GameController>
{
	static int CurrentPaacjsInScene = 0;

	public static int Score { get; private set; } = 0;
	public static int Highscore { get; private set; }

	static Action<int> onPointUp;
	static Action onGameOver;

	readonly static int paacjLimit = 11;

	void Start()
	{
		CurrentPaacjsInScene = 0;
		Score = 0;
		Highscore = PlayerPrefs.GetInt("highscore", 0);
	}

	public static void Reset()
	{
		Instance.Start();
	}

	public static void PaacjUp()
	{
		CurrentPaacjsInScene++;

		if (paacjLimit <= CurrentPaacjsInScene)
		{
			GameOver();
		}
	}

	public static void PaacjDestroyed()
	{
		CurrentPaacjsInScene--;
	}

	static void GameOver()
	{
		if (Score > PlayerPrefs.GetInt("highscore"))
		{
			PlayerPrefs.SetInt("highscore", Score);
		}
		Destroy(Camera.main.GetComponent<Physics2DRaycaster>());
		onGameOver.Invoke();
	}

	public static void PointUp()
	{
		Score++;
		onPointUp.Invoke(Score);
	}

	public static void RegisterOnPointUp(Action<int> newMethod)
	{
		onPointUp += newMethod;
	}

	public static void UnregisterOnPointUp(Action<int> methodToRemove)
	{
		onPointUp -= methodToRemove;
	}

	public static void RegisterOnGameOver(Action newMethod)
	{
		onGameOver += newMethod;
	}

	public static void UnregisterOnGameOver(Action methodToRemove)
	{
		onGameOver -= methodToRemove;
	}

}
