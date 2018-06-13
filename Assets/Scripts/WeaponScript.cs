using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon script. Launch projectile
/// </summary>

public class WeaponScript : MonoBehaviour
{
	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;

	/// <summary>
	/// Cooldown in seconds between two shots
	/// </summary>
	public float shootingRate = 0.2f;

	//-----------------------
	// -- Cooldown
	//-----------------------
	private float shootCooldown;

	void Start ()
	{
		float random = Random.Range(0.0f, 2.0f);
		shootCooldown = random;
	}

	void Update ()
	{
		if (shootCooldown > 0.0f)
		{
			shootCooldown -= Time.deltaTime;
		}
	}



	//-----------------------
	// -- Shooting from an other script
	//-----------------------

	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			float random = Random.Range(0.0f, 0.2f);
			shootCooldown = shootingRate + random;

			// Create a new shot
			//var shotTransform = Instantiate(shotPrefab, transform.position, Quaternion.Inverse(transform.parent.rotation)) as Transform;
			var shotTransform = Instantiate(shotPrefab, transform.position, transform.parent.rotation) as Transform;

			// Assign position
			shotTransform.position = transform.position;

			// The isEnemy property
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
			}

			// Make the weappon shot always towards it
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.direction = this.transform.right; // that is towards in 2D sprite space
			}
		}
	}

	/// <summary>
	/// Is the weappon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}
