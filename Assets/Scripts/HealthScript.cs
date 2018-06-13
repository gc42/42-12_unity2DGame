using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle hitpoints and damages
/// </summary>

public class HealthScript : MonoBehaviour
{
	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int hp = 1;

	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	/// <summary>
	/// Inflict damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount)
	{
		hp -= damageCount;

		if (hp <= 0)
		{
			// Explosion
			SpecialEffectsHelper.Instance.Explosion(new Vector3(transform.position.x, transform.position.y, -1.0f));
			Debug.Log("Explosion called");
			// Dead
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this collision a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);

				// Destroy the shot
				Destroy(shot.gameObject);
			}
		}
	}
}
