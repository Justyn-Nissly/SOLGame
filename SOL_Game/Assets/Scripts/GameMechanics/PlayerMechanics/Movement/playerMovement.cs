using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
	private float speed;
	private float moveHorizontal;
	private float moveVertical;
	private Vector2 movement;
	private Rigidbody2D rb2d;
    public Animator animator;



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
		speed = .1f;

		rb2d = GetComponent<Rigidbody2D>();
	 }

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
		
	}

	private void FixedUpdate()
	{
		// Get the Horizontal Axis
		if (Input.GetAxis("Horizontal") > 0)
			moveHorizontal = speed;
		else if (Input.GetAxis("Horizontal") < 0)
			moveHorizontal = -speed;
		else
			moveHorizontal = 0;

		// Get the Vertical Axis
		if (Input.GetAxis("Vertical") > 0)
			moveVertical = speed;
		else if (Input.GetAxis("Vertical") < 0)
			moveVertical = -speed;
		else
			moveVertical = 0;

        // Set the movement vector
		movement = new Vector2(moveHorizontal, moveVertical);

        // Update the values in the Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", speed);

        // Update the Hero's position, taking note of colliders.
        rb2d.MovePosition(movement + rb2d.position);
	}
}
