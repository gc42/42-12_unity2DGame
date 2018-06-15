using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller and behaviour
/// </summary>
public class PlayerScript : MonoBehaviour
{

	/// <summary>
	/// The speed of the ship
	/// </summary>
	public Vector2 speed = new Vector2(20, 20);

	// Store the movement and the component
	private Vector2 movement;
	private Rigidbody2D rb;



	// Update is called once per frame
	void Update ()
	{
		// Get axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		// Movement per direction
		movement = new Vector2(
			speed.x * (inputX + mouseX),
			speed.y * (inputY + mouseY)
			);

		// Shooting
		bool shoot = ( Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) );


		if (shoot)
		{
			WeaponScript weapon = GetComponentInChildren<WeaponScript>();
			if (weapon != null)
			{
				// Attack(false) because the player is not an enemy
				weapon.Attack(false);
				SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
		}

		// Make sure we are not outside the camera bounds
		var distZ = (transform.position - Camera.main.transform.position).z;
		var leftBorder   = Camera.main.ViewportToWorldPoint( new Vector3(0, 0, distZ) ).x;
		var rightBorder  = Camera.main.ViewportToWorldPoint( new Vector3(1, 0, distZ) ).x;
		var topBorder    = Camera.main.ViewportToWorldPoint( new Vector3(0, 0, distZ) ).y;
		var bottomBorder = Camera.main.ViewportToWorldPoint( new Vector3(0, 1, distZ) ).y;

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder,  bottomBorder),
			transform.position.z);

	} // End Update method -------------------------------------------------------



	private void FixedUpdate()
	{
		// Get the component and store the reference
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		// Move the game object
		rb.velocity = movement;
	}

	private void OnCollisionEnter2D(Collision2D otherCollider)
	{
		bool damagePlayer = false;

		// Collision with enemy
		EnemyScript enemy = otherCollider.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Kill the enemy
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null)
			{
				enemyHealth.Damage(enemyHealth.hp, false);
			}

			damagePlayer = true;
		}

		// Damage the player
		if (damagePlayer)
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null)
			{
				playerHealth.Damage(1, false);
			}
		}
	}

	private void OnDestroy()
	{
		// Game Over
		var gameOver = FindObjectOfType<GameOverScript>();

		gameOver.ShowButtons();
	}



}
