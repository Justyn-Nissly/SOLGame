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
	private bool
		showShield;
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
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a +
			((FindObjectOfType<OrbController>() != null && (behemoth.enemyMovement.enabled)) ? 0.02f : -0.02f));

		if (sprite.color.a <= 0.0f)
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.0f);
		}
		else if (sprite.color.a >= 1.0f)
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);
		}
	}
	#endregion

	#region Utility Functions (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}