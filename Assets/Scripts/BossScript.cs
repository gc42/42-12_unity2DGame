using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss script. Boss enemy generic behaviur
/// </summary>
public class BossScript : MonoBehaviour
{
	private bool hasSpawn;

	// Component references
	private MoveScript moveScript;
	private WeaponScript[] weapons;
	private Animator animator;
	private SpriteRenderer[] renderers;
	private Collider2D coll;

	// Boss patern (not really an AI)
	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;

	private float aiCooldown;
	private bool isAttacking;
	private Vector2 positionTarget;

	private void Awake()
	{
		// Retreive all the weapons only once
		weapons = GetComponentsInChildren<WeaponScript>();

		// Retreive scripts to disable when not spawned
		moveScript = GetComponent<MoveScript>();
		coll = GetComponent<Collider2D>();

		// Get the animator
		animator = GetComponent<Animator>();

		// Get all the renderers in childrens
		renderers = GetComponentsInChildren<SpriteRenderer>();
	}


	private void Start ()
	{
		hasSpawn = false;

		// Disable everything
		// -- Collider
		coll.enabled = false;
		// -- Moving
		moveScript.enabled = false;
		// -- Shooting
		foreach (WeaponScript weapon in weapons)
		{
			weapon.enabled = false;
		}

		// Default behaviour
		isAttacking = false;
		aiCooldown = maxAttackCooldown;
	}
	
	private void Update ()
	{
		// Check if the Boss has spawned
		if (hasSpawn == false)
		{
			// We check just the first renderer for simplicity.
			// But we don't know if it's the body, eye or mouth...
			if (renderers[0].IsVisibleFrom(Camera.main))
			{
				Spawn();
			}
		}
		else
		{
			// AI
			// -----------------------
			// Move or attack. Permute. Repeat.
			aiCooldown -= Time.deltaTime;

			if (aiCooldown <= 0f)
			{
				isAttacking = !isAttacking;
				aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
				positionTarget = Vector2.zero;

				// Set or unset the attack animation
				animator.SetBool("Attack", isAttacking);
			}

			// Attack
			// ------------------------
			if (isAttacking)
			{
				// Stop any movement
				moveScript.direction = Vector2.zero;

				foreach (WeaponScript weapon in weapons)
				{
					if (weapon != null && weapon.enabled && weapon.CanAttack)
					{
						weapon.Attack(true);
						SoundEffectsHelper.Instance.MakeEnemyShotSound();
					}
				}
			}

			// Move
			// -------------------------
			else
			{
				// Define a target ?
				if (positionTarget == Vector2.zero)
				{
					// Get a point on the screen, convert it to world
					Vector2 randomPoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

					positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
				}

				// Are we at the target? If so, find a new one
				if (coll.OverlapPoint(positionTarget))
				{
					// reset, will be set at the next frame
					positionTarget = Vector2.zero;
				}

				// Go to the point
				Vector3 direction = ((Vector3)positionTarget - this.transform.position);

				// Remember to use the move script
				moveScript.direction = Vector3.Normalize(direction);
			}
		}
	}


	private void Spawn()
	{
		hasSpawn = true;

		// Enable everything
		// -- Collider
		coll.enabled = true;
		// -- Moving
		moveScript.enabled = true;
		// -- Shooting
		foreach (WeaponScript weapon in weapons)
		{
			weapon.enabled = true;
		}

		// Stop the main scrolling
		foreach (ScrollingScript scrolling in FindObjectsOfType<ScrollingScript>())
		{
			if (scrolling.isLinkedToCamera)
			{
				scrolling.speed = Vector2.zero;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D otherCollider2D)
	{
		// tacking damage? Change animation
		ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			if (shot.isEnemyShot == false)
			{
				// Stop attack and start move away
				aiCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
				isAttacking = false;
			}
		}
	}

	private void OnDrawGizmos()
	{
		// A little tip: you can display debug information in your scene with Gizmos
		if (hasSpawn && isAttacking == false)
		{
			Gizmos.DrawSphere(positionTarget, 0.25f);
		}
	}

}
