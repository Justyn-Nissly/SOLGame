using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testthrow : MonoBehaviour
{
	public MeleeGuardian
		meleeGuardian;

	public Animator
		anim;

	public GameObject
		origin;
	public bool
		returnOrigin,
        shouldThrow;
	public Vector2
		destination;
	// Start is called before the first frame update
	void Start()
    {
		
		returnOrigin = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (shouldThrow)
		{
			if ((Vector2)transform.position == destination)
			{
				returnOrigin = true;
			}
			else if (transform.position == origin.transform.position)
			{
                if (returnOrigin)
				    shouldThrow = false;
				destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
				returnOrigin = false;
			}
			transform.position = (returnOrigin == true) ? Vector2.Lerp(transform.position, origin.transform.position, 10 * Time.deltaTime) :
														  Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);
		}

		//meleeGuardian.moving = false;
	}

 /*   private void Throw()
    {
		//StartCoroutine(HomingSword());
		//StartCoroutine(HomingSword());

		if ((Vector2)transform.position == destination)
		{
			returnOrigin = true;
		}
		else if (transform.position == origin.transform.position)
		{
			destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
			returnOrigin = false;
		}
		transform.position = (returnOrigin == true) ? Vector2.Lerp(transform.position, origin.transform.position, 10 * Time.deltaTime) :
													  Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);

		meleeGuardian.moving = false;
		anim.SetTrigger("Patrol");

	}

	/* public IEnumerator HomingSword()
	{

		if (Input.GetKey(KeyCode.Return))
		{
			if ((Vector2)transform.position == destination)
			{
				returnOrigin = true;
			}
			else if (transform.position == origin.transform.position)
			{
				destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
				returnOrigin = false;
			}
			transform.position = (returnOrigin == true) ? Vector2.Lerp(transform.position, origin.transform.position, 10 * Time.deltaTime) :
														  Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);
		}
		yield return null;
	}
    */




}