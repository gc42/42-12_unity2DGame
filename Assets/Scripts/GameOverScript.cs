using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Restart or quit the game
/// </summary>
public class GameOverScript : MonoBehaviour
{
	private GameObject[] buttons;

	private void Awake()
	{
		// Get the buttons
		buttons = GameObject.FindGameObjectsWithTag("button");

		// Disable them
		HideButtons();
		Time.timeScale = 1.0f;
	}


	public void HideButtons()
	{
		foreach (var b in buttons)
		{
			b.SetActive(false);
		}
	}

	public void ShowButtons()
	{
		foreach (var b in buttons)
		{
			b.SetActive(true);
		}
		Time.timeScale = 0.1f;
	}

	public void ExitToMenu()
	{
		// Load the menu
		SceneManager.LoadScene(0);
	}

	public void RestartGame()
	{
		// Restart the level
		SceneManager.LoadScene(1);
	}



}
