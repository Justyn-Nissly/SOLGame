using System.Collections;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private float
		alpha;
	private SpriteRenderer
		fadeSprite;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	void Start()
	{
		fadeSprite = GetComponent<SpriteRenderer>();
		fadeSprite.sprite = FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite;
		fadeSprite.sortingOrder = FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sortingOrder - 1;
		alpha = 1.0f;
	}

	void FixedUpdate()
	{
		fadeSprite.color = new Color(fadeSprite.color.r, fadeSprite.color.g, fadeSprite.color.b, alpha);
		if ((alpha -= 0.07f) <= 0.0f)
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}