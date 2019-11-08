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
		// get the horizontal movement input
		if (Input.GetAxis("Horizontal") > 0)
			moveHorizontal = speed;
		else if (Input.GetAxis("Horizontal") < 0)
			moveHorizontal = -speed;
		else
			moveHorizontal = 0;

		// get the vertical movement input
		if (Input.GetAxis("Vertical") > 0)
			moveVertical = speed;
		else if (Input.GetAxis("Vertical") < 0)
			moveVertical = -speed;
		else
			moveVertical = 0;

		movement = new Vector2(moveHorizontal, moveVertical);

		// move the player if there not colliding with a object

		rb2d.MovePosition(movement + rb2d.position);
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
//        }
	}
}
