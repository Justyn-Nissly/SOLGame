using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testthrow : MonoBehaviour
{

	public GameObject
		origin;
	public bool
		returnOrigin;
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
		if (Input.GetKey(KeyCode.Backspace))
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
			                                              Vector2.Lerp(transform.position, destination,               10 * Time.deltaTime);
		}
	}
}