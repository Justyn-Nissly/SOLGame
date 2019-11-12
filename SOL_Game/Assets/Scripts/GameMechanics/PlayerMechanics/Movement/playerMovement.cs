﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
	public float speed;
	private float moveHorizontal;
	private float moveVertical;
	private Vector2 movement;
	private Rigidbody2D rb2d;
    public Animator animator;

	private Player player;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
		//+speed = .1f;

		rb2d = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
	 }

	private void FixedUpdate()
	{
		if (player.canMove)
		{
			movement = new Vector2(Mathf.RoundToInt(Input.GetAxis("Horizontal")) * speed, Mathf.RoundToInt(Input.GetAxis("Vertical")) * speed);

			// Update the values in the Animator
			animator.SetFloat("Horizontal", movement.x);
			animator.SetFloat("Vertical", movement.y);
			animator.SetFloat("Magnitude", movement.magnitude);

			// Update the Hero's position, taking note of colliders.
			rb2d.MovePosition(movement + rb2d.position);
		}
	}
}
