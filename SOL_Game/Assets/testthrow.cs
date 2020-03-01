using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testthrow : MonoBehaviour
{

	public GameObject
		origin,
		swordArm;
	public bool
		returnOrigin;
	public Vector2
		destination;
	public Animator
		anim;
	// Start is called before the first frame update
	void Start()
    {
		swordArm.transform.position = this.transform.position;
		returnOrigin = false;
	}

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			anim.SetBool("isIdle", false);
		}
		if(Input.GetKeyDown(KeyCode.RightShift))
		{
			anim.SetBool("isIdle", true);
		}
	}
	public void ThrowSword()
	{
		if (transform.position == origin.transform.position)
		{
			destination = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
			returnOrigin = false;
		}
		while(returnOrigin == false)
		{ 
			if ((Vector2)transform.position == destination)
			{
				returnOrigin = true;
			}
			transform.position = Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);
		}
		while(returnOrigin == true)
		{
			if(transform.position == origin.transform.position)
			{
				returnOrigin = false;
			}
			transform.position = Vector2.Lerp(transform.position, origin.transform.position, 10 * Time.deltaTime);
		}
	}
}