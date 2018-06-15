using UnityEngine;
using System.Collections;


/// <summary>
/// Projectile behaviour
/// </summary>
public class ShotScript : MonoBehaviour
{
	/// <summary>
	/// Damage inflicted
	/// </summary>
	public int damage = 1;

	/// <summary>
	/// Projectile damage the player or these enemies?
	/// </summary>
	public bool isEnemyShot = false;


	void Start()
	{
		// Life time limited
		Destroy(gameObject, 6); // 20sec
	}

	private void Update()
	{
		// Delete if out of scene limits
		if (transform.position.x < -10f || transform.position.y > 10f || transform.position.y < -10f)
		{
			Destroy(gameObject);
		}
	}


}
