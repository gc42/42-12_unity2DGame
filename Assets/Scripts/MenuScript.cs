using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
{
	public void StartTheGame ()
	{
		// "" is the name of the scene to launch to start the game
		SceneManager.LoadScene("2DGame01");
	}

}
