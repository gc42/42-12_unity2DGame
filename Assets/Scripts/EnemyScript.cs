using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy generic behaviour
/// </summary>
public class EnemyScript : MonoBehaviour
{
	private bool hasSpawn;
	private MoveScript moveScript;
	private WeaponScript[] weapons;
	private Collider2D colliderComponent;
	private SpriteRenderer rendererComponent;

	void Awake()
	{
		// Retrieve the weapon only once
		weapons = GetComponentsInChildren<WeaponScript>();

		// Retreive scripts to could disable them when not spawn
		moveScript = GetComponent<MoveScript>();
		colliderComponent = GetComponent<Collider2D>();
		rendererComponent = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		hasSpawn = false;

		// Disable everything
		// -- Collider
		colliderComponent.enabled = false;
		// -- Moving
		moveScript.enabled = false;
		// -- Shooting
		foreach (WeaponScript weapon in weapons)
		{
			weapon.enabled = false;
		}
	}



	void Update ()
	{
		// Check if the enemy has spawned
		if (hasSpawn == false)
		{
			if (rendererComponent.IsVisibleFrom(Camera.main))
			{
				Spawn();
			}
		}
		else
		{
			// Auto-fire
			foreach (WeaponScript weapon in weapons)
			{
				if (weapon != null && weapon.CanAttack)
				{
					// Attack(true) because here we are an enemy
					weapon.Attack(true);
				}
			}

			// If out of camera, destroy the game object
			if (rendererComponent.IsVisibleFrom(Camera.main) == false)
			{
				Destroy(gameObject);
			}
		}

	}

	// Activate him self
	private void Spawn()
	{
		hasSpawn = true;

		// Enable everything
		// -- Collider
		colliderComponent.enabled = true;
		// -- Moving
		moveScript.enabled = true;
		// -- Shooting
		foreach (WeaponScript weapon in weapons)
		{
			weapon.enabled = true;
		}
	}
}
