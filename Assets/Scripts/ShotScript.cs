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
		Destroy(gameObject, 20); // 20sec
	}


}
