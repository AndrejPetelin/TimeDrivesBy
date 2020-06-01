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

	AudioClipControls music;

	bool hasDestinationReached;
	bool isGameOver;
	float timer;
	bool hasAudioPlayed;
    ExplosionEffect effect;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			hasDestinationReached = true;
		}
	}

    private void Start()
    {
        effect = GetComponent<ExplosionEffect>();
        effect.PlayParticlesAt(transform.position);

		music = FindObjectOfType<AudioClipControls>();
    }


    public void GameOver()
	{
		isGameOver = true;
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("MainMenuScene");
		}

		if (hasDestinationReached)
		{
			music.VolumeTransition(0f, 0.1f);
			EndLevel(endLevelBGImage, false, endLevelAudio);
		}
		else if (isGameOver) 
		{
			music.VolumeTransition(0f, 0.1f);
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
				if (SceneManager.GetActiveScene().buildIndex + 2 < SceneManager.sceneCountInBuildSettings)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				}
				else
				{
					SceneManager.LoadScene("WinScene");
				}
			}

		}

	}
}
