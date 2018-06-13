using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creating instance of sound from code with no effort
/// </summary>
public class SoundEffectsHelper : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static SoundEffectsHelper Instance;

	public AudioClip explosionSound;
	public AudioClip playerShotSound;
	public AudioClip enemyShotSound;

	private void Awake()
	{
		// Register singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of SoundEffectsHelper");
		}

		Instance = this;
	}

	public void MakeExplosionSound()
	{
		MakeSound(explosionSound);
	}

	public void MakePlayerShotSound()
	{
		MakeSound(playerShotSound);
	}

	public void MakeEnemyShotSound()
	{
		MakeSound(enemyShotSound);
	}



	/// <summary>
	/// Play a given sound
	/// </summary>
	/// <param name="originalClip"></param>
	private void MakeSound(AudioClip originalClip)
	{
		// As it is not a 3D audio clip, position doesn't matter.
		AudioSource.PlayClipAtPoint(originalClip, transform.position);
	}
}
