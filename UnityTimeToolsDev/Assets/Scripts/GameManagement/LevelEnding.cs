using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnding : MonoBehaviour
{
	public float fadeDuration = 1f;
	public float displayImageDuration = 3f;
	public GameObject player;
	public CanvasGroup endLevelBGImage;
	public AudioSource endLevelAudio;
	public CanvasGroup gameOverBGImage;
	public AudioSource gameOverAudio;

	bool hasDestinationReached;
	bool isGameOver;
	float timer;
	bool hasAudioPlayed;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			Debug.Log("Destination is reached");
			hasDestinationReached = true;
		}
	}


	public void GameOver()
	{
		isGameOver = true;
	}


	private void Update()
	{
		if (hasDestinationReached)
		{
			EndLevel(endLevelBGImage, false, endLevelAudio);
		}
		else if (isGameOver) 
		{
			EndLevel(gameOverBGImage, true, gameOverAudio);
		}
	}

	void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
	{
		if (!hasAudioPlayed)
		{
			audioSource.Play();
			hasAudioPlayed = true;
		}

		timer += Time.deltaTime;
		imageCanvasGroup.alpha = timer / fadeDuration;

		if (timer > fadeDuration + displayImageDuration)
		{
			if (doRestart)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
			else
			{
				// If next index in Build Settings less than number of scenes in Build Settings load next scene
				if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
				else  //TODO
				{
					SceneManager.LoadScene("MainMenuScene");
					//SceneManager.LoadScene(0);
				}
			}

		}

	}
}
