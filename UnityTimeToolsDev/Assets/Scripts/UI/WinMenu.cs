﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene2");
	}

	public void PlayAgain()
	{
		SceneManager.LoadScene("Level1");
	}
}
