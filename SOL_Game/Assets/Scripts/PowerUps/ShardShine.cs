using System.Collections;
using UnityEngine;

public class ShardShine : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	public SpriteRenderer
		shardSprite;
	#endregion

	#region Private Variables (Empty)
	private SpriteRenderer
		sprite;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		sprite.sortingOrder = shardSprite.sortingOrder - 1;
		/*sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f);*/
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}