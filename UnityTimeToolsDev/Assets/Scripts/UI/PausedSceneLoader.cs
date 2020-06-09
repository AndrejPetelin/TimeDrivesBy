using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedSceneLoader : MonoBehaviour
{
	private string pauseScene = "PausedScene_Asli";
	public bool isPaused = false;
	bool isLoaded = false;
  
    Camera mainCam;


    private void Start()
    {
        mainCam = Camera.main;
    
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!isPaused)
			{
                Globals.camPosition = mainCam.transform.position;
                Globals.camRotation = mainCam.transform.rotation;
				PauseGame();				
			}
			else
			{
				ResumeGame();
			}
		}
	}


	void PauseGame()
	{
		isPaused = true;
		Time.timeScale = 0f;
		SceneManager.LoadScene(pauseScene, LoadSceneMode.Additive);
		isLoaded = true;
	}


	public void ResumeGame()
	{
		isPaused = false;
		Time.timeScale = 1f;
		if (isLoaded)
		{
			SceneManager.UnloadSceneAsync(pauseScene);
			isLoaded = false;
		}
	}
}
