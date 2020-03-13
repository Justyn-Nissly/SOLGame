using UnityEngine;

public class BehemothShield : MonoBehaviour
{
	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private BehemothController
		behemoth;
	private SpriteRenderer
		sprite;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		sprite   = GetComponent<SpriteRenderer>();
		behemoth = FindObjectOfType<BehemothController>();
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		if (FindObjectOfType<OrbController>() == null || (behemoth.enemyMovement.enabled == true))
		{
			sprite.enabled = false;
		}
		else
		{
			sprite.enabled = true;
		}
	}
	#endregion

	#region Utility Functions (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}