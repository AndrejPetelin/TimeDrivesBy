using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
	public AudioMixer audioMixer;
    

	public void SetVolumeMusic(float volume)
	{
		audioMixer.SetFloat("volumeMusic", volume); 
	}

	public void SetVolumeSounds(float volume)
	{
		audioMixer.SetFloat("volumeSFX", volume);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}
}
