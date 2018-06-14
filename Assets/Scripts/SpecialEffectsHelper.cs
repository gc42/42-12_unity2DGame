using UnityEngine;

/// <summary>
/// Creating instance of particule system from code
/// </summary>

public class SpecialEffectsHelper : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static SpecialEffectsHelper Instance;

	public ParticleSystem smokeEffect;
	public ParticleSystem fireEffect;
	public ParticleSystem fireEffectLittle;

	private void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of SpecialEffectsHelper!!");
		}

		Instance = this;
	}



	/// <summary>
	/// Create an explosion at the given location
	/// </summary>
	/// <param name="position"></param>
	public void Explosion(Vector3 position)
	{
		CreateInstantiate(smokeEffect, position);
		CreateInstantiate(fireEffect, position);
	}

	public void ExplosionPlouf(Vector3 position)
	{
		CreateInstantiate(fireEffectLittle, position);
	}



	/// <summary>
	/// Instantiate a Particule system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem CreateInstantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
		  ) as ParticleSystem;

		// Make sure it will be destroyed
		Destroy( newParticleSystem.gameObject,  0.6f); //newParticleSystem.main.startLifetime

		return newParticleSystem;
	}
}
