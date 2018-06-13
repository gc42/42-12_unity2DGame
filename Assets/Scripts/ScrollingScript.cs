using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that shoud be assigned to a layer
/// </summary>
public class ScrollingScript : MonoBehaviour
{
	/// <summary>
	/// Scrolling speed
	/// </summary>
	public Vector2 speed = new Vector2(2, 2);

	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);

	/// <summary>
	/// Movement shoud be applied to camera
	/// </summary>
	public bool isLinkedToCamera = false;

	/// <summary>
	/// Background is infinite
	/// </summary>
	public bool isLooping = false;


	// List of children with a renderer
	private List<SpriteRenderer> backgroundPart;

	private Vector2 repeatableSize;




	private void Start()
	{
		// For infinite background only
		if (isLooping)
		{
			//---------------------------------------------------------------------------------
			// Retrieve background objects:
			// -- We need to know what this background is made of
			// -- Store a reference of each object
			// -- Order those items in the order of the scrolling, so we know the item that will be the first to be recycled
			// -- Compute the relative position between each part before they start moving
			//---------------------------------------------------------------------------------

			// Initialise an empty list
			backgroundPart = new List<SpriteRenderer>();

			// Get all the children of the layer with a renderer
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				SpriteRenderer r = child.GetComponent<SpriteRenderer>();

				// Add only the visible children
				if (r != null)
				{
					backgroundPart.Add(r);
				}
			}

			if (backgroundPart.Count == 0)
			{
				Debug.LogError("Nothing to scroll!!");
			}

			// Sort by position
			// Note: Get the children from left to right.
			// Later, we would need to add a few conditions to handle
			// all the possible scrolling directions.
			backgroundPart = backgroundPart.OrderBy(
				t => t.transform.position.x * (-1 * direction.x)).ThenBy(
				t => t.transform.position.y * (-1 * direction.y)).ToList();

			// Get the size of the repeatable parts
			var first = backgroundPart.First();
			var last  = backgroundPart.Last();

			repeatableSize = new Vector2(
				Mathf.Abs(last.transform.position.x - first.transform.position.x),
				Mathf.Abs(last.transform.position.y - first.transform.position.y));

		}
	}



	void Update ()
	{
		// Movement
		Vector3 movement = new Vector3(
		speed.x * direction.x,
		speed.y * direction.y,
		0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

		// Move the camera
		if (isLinkedToCamera)
		{
			Camera.main.transform.Translate(movement);
		}


		// Loop
		if (isLooping)
		{
			//--------------------
			// Check if the object is before, in or after the Camera bounds
			//--------------------

			// Camera borders
			var distZ = (transform.position - Camera.main.transform.position).z;
			float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distZ)).x;
			float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distZ)).x;
			float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distZ)).y;
			float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distZ)).y;

			// Determine entry and exit border using direction
			Vector3 exitBorder  = Vector3.zero;
			Vector3 entryBorder = Vector3.zero;

			if (direction.x < 0)
			{
				entryBorder.x = rightBorder;
				exitBorder.x  = leftBorder;
			}
			else if (direction.x > 0)
			{
				entryBorder.x = leftBorder;
				exitBorder.x  = rightBorder;
			}

			if (direction.y < 0)
			{
				entryBorder.y = topBorder;
				exitBorder.y  = bottomBorder;
			}
			else if (direction.y > 0)
			{
				entryBorder.y = bottomBorder;
				exitBorder.y  = topBorder;
			}











			// Get the first object.
			// Remember that the list is ordered from left (x position) to right
			// and top to bottom
			SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

			if (firstChild != null)
			{
				bool checkVisible = false;

				// Check if we are after the camera
				// The check is on the position first, as IsVisibleFrom is a heavy method
				// Here again, we check the border depending on the direction
				if (System.Math.Abs(direction.x) > Mathf.Epsilon)
				{
					if (   (direction.x < 0 && (firstChild.transform.position.x < exitBorder.x))
					    || (direction.x > 0 && (firstChild.transform.position.x > exitBorder.x)))
					{
						checkVisible = true;
					}
				}
				if (System.Math.Abs(direction.y) > Mathf.Epsilon)
				{
					if (   (direction.y < 0 && (firstChild.transform.position.y < exitBorder.y))
						|| (direction.y > 0 && (firstChild.transform.position.y > exitBorder.y)))
					{
						checkVisible = true;
					}
				}







				// Check if the sprite is really visible on the camera or not
				if (checkVisible)
				{
					//----------------------------------------------------------
					// The object was in the camera bounds but isn't anymore.
					// -- We need to recycle it
					// -- That means he was the first, he's now the last
					// -- And we physically moves him to the further position possible
					//----------------------------------------------------------

					if (firstChild.IsVisibleFrom(Camera.main) == false)
					{


						// Set the position of the recycled one to be AFTER the last child.
						firstChild.transform.position = new Vector3(
							
							firstChild.transform.position.x + ((repeatableSize.x + firstChild.bounds.size.x) * -1 * direction.x),
							firstChild.transform.position.y + ((repeatableSize.y + firstChild.bounds.size.y) * -1 * direction.y),
							firstChild.transform.position.z);

						// Set the recycled child to the last position
						// of the backgroundPart list
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
		}
	}
}
