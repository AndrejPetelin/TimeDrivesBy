using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
	private string menuScene = "MainMenuScene2";


	public void BackToMainMenu()
	{
		FindObjectOfType<PausedSceneLoader>().ResumeGame();
		SceneManager.LoadScene(menuScene);
	}

	public void OnResumeGame()
	{
		FindObjectOfType<PausedSceneLoader>().ResumeGame();
	}
}
