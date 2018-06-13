using UnityEngine;
using System.Collections;

/// <summary>
/// Simply moves the current game object
/// </summary>

public class MoveScript : MonoBehaviour
{
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);

	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);

	private Vector2 movement;
	private Rigidbody2D rb;


	// Update is called once per frame
	void Update()
	{
		// Movement
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
	}

	private void FixedUpdate()
	{
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		// Apply movement to rigid body
		rb.velocity = movement;
	}


}
