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
	private bool iAmEnemyShot = false;

	/// <summary>
	/// Inflict damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount, bool iAmEnemyShot)
	{
		hp -= damageCount;

		if (hp <= 0)
		{
			// Explosion

			if (iAmEnemyShot == true)
			{
				SpecialEffectsHelper.Instance.ExplosionPlouf(new Vector3(transform.position.x, transform.position.y, -2.0f));
			}
			else
				SpecialEffectsHelper.Instance.Explosion(new Vector3(transform.position.x, transform.position.y, -2.0f));

			SoundEffectsHelper.Instance.MakeExplosionSound();
			// Dead
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this collision a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		iAmEnemyShot = false;

		if (tag == "shot_enemy")
		{
			iAmEnemyShot = true;
		}

			
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage, iAmEnemyShot);

				// Destroy the shot
				SoundEffectsHelper.Instance.MakeExplosionSound();
				Destroy(shot.gameObject);
			}
		}
	}
}
