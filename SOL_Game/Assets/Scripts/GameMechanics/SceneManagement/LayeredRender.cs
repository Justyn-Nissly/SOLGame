using UnityEngine;

public class LayeredRender : MonoBehaviour
{
	#region Enums and Defined Constants
	public const int MAX_Y = 10000; // Prevent the layer order form being negative
	#endregion

	#region Public Variables
	public bool
		shouldUpdate; // Check if the object needs to update its render layer
	#endregion

	#region Private Variables
	private SpriteRenderer
		sprite; // Reference the object's sprite
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Overlay objects further up to appear in front of them </summary>
	void Awake()
	{
		sprite              = GetComponent<SpriteRenderer>();
		sprite.sortingOrder = MAX_Y - ((int) (transform.position.y * 1000.0f) >> 6);
	}

	/// <summary> Overlay objects further up to appear in front of them </summary>
	void FixedUpdate()
	{
		if (shouldUpdate)
		{
			sprite.sortingOrder = MAX_Y - ((int)(transform.position.y * 1000.0f) >> 6);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}