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

		// Movement per direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
	}

	// This function is called every fixed framerate frame, if the MonoBehaviour is enabled
	private void FixedUpdate()
	{
		// Get the component and store the reference
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		// Move the game object
		rb.velocity = movement;
	}
	// updated

}
