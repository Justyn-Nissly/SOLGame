using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerMovementTest : MonoBehaviour
{
	public Animator animator;
	public int speed;
	private Rigidbody2D PlayerRigidbody2D;

	private void Start()
	{
		PlayerRigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
    {
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

		animator.SetFloat("Horizontal", movement.x);
		animator.SetFloat("Vertical",   movement.y);
		animator.SetFloat("Magnitude",  movement.magnitude);

		transform.position = transform.position + movement * speed * Time.deltaTime;

		PlayerRigidbody2D.velocity = new Vector2(0, 0);
    }
}
