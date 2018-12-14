using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
	void Awake()
	{
		gameObject.SetActive(false);
		GameController.RegisterOnGameOver(HandleGameOver);
	}

	void HandleGameOver()
	{
		gameObject.SetActive(true);
	}

	public void TryAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		GameController.UnregisterOnGameOver(HandleGameOver);
		GameController.Reset();
	}

	public void ReturnToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
		GameController.UnregisterOnGameOver(HandleGameOver);
		GameController.Reset();
	}
}
