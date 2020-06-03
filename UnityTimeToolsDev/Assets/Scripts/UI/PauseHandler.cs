using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseHandler : MonoBehaviour
{
	public GameObject pauseMenuUI;
	private string menuScene = "MainMenuScene2";
	bool isPaused = false;


	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
		}

		if (isPaused)
		{
			PauseGame();
		}
		else
		{
			ResumeGame();
		}
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		pauseMenuUI.SetActive(true);
	}

	public void ResumeGame()
	{
		isPaused = false;
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
	}

	public void BackToMainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(menuScene);	
	}
}
