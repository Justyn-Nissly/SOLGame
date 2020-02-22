using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N13GL : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Animator
		armAnimator,   // The animator for the guardian's arm
		n13glAnimator; // The animator for N13GL
	public BoxCollider2D
		attackCollider,otherone; // The collider that triggers the attack
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
		{
			armAnimator.SetTrigger("Attack");
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			armAnimator.SetTrigger("Idle");
		}
    }

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider == attackCollider)
		{
			armAnimator.SetTrigger("Attack");
			Debug.Log("AAAAAAAAAAAAAAA");
		}
		else if (collider == otherone)
		{
			Debug.Log("nope");
		}
		
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines
	#endregion
}