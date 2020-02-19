using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N13GLEncounter : MonoBehaviour
{
	public bool
		startEncounter; // Check if the encounter needs to be started or not
	public Animator
		anim; // The animation controler for the enemy

    // Start is called before the first frame update
    void Start()
    {
		startEncounter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (anim.GetBool("startEncounter") == false)
			{
				anim.SetBool("startEncounter", true);
			}
			else
			{
				anim.SetBool("startEncounter", false);
				anim.GetBehaviour<IntroBehaviour>().isFadedIn = false;
				anim.GetBehaviour<IntroBehaviour>().N13GL.GetComponent<_2dxFX_BurnFX>().Destroyed = 1.0f;
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(startEncounter == false)
		{
			startEncounter = true;
		}
	}
}