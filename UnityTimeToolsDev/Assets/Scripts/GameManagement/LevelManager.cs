using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	//public GameObject loadingPanel;
	//public Slider loadingProgressBar;
		
     public void LoadLevel(int sceneIndex)
    {
		if (sceneIndex < SceneManager.sceneCountInBuildSettings)
		{
			StartCoroutine(LoadAsync(sceneIndex));
		}
		else  //TODO
		{
			StartCoroutine(LoadAsync(0));
		}
		
	}

    IEnumerator LoadAsync(int sceneIndex)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

		//loadingPanel.SetActive(true);
		while (!asyncLoad.isDone)
		{
			//float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			//loadingProgressBar.value = progress;
			//Debug.Log(progress);
			yield return null;
		}
	}
}
