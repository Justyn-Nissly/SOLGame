using System.Collections;
using UnityEngine;

public class Shard : QuestItem
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private float
		spinTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	public override void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Player>();
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	void FixedUpdate()
	{
		spinTimer += Time.deltaTime * 10.0f;
		sprite.sprite = sprites[(int)(spinTimer) % 6];
	}

	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}