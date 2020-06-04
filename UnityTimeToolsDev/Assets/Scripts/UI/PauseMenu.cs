using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
	private string menuScene = "MainMenuScene2";
    public Camera mainCam;

    private void Start()
    {
        Debug.Log("PAUSE START");
       // mainCam = Camera.main;
        mainCam.transform.position = Globals.camPosition;
        mainCam.transform.rotation = Globals.camRotation;
    }

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
