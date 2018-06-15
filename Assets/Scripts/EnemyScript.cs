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
		UnSpawn();
	}



	void Update ()
	{
		// Check if the enemy has spawned
		if (hasSpawn == false)
		{
			if (rendererComponent.IsVisibleFrom(Camera.main)== true)
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
					SoundEffectsHelper.Instance.MakeEnemyShotSound();
				}
			}

			// If out of camera, spawn the game object at the other side of the playground
			if (rendererComponent.IsVisibleFrom(Camera.main) == false)
			{
				float randomDistance = Random.Range(0f, 3f);
				Vector2 repeatableSize = GetComponentInParent<ScrollingScript>().repeatableSize;


				transform.position = new Vector2(
					transform.position.x + repeatableSize.x + randomDistance,
					transform.position.y);
				UnSpawn();
			}

		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			UnSpawn();
		}
		if (Input.GetKeyUp(KeyCode.A))
		{
			UnSpawn();
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

	// Deactivate him self
	private void UnSpawn()
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
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
	}
}
