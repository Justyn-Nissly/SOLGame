using System.Collections;
using UnityEngine;

public class Shard : QuestItem
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SpriteRenderer
		shine;
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
		base.Awake();
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		sprite.sprite = sprites[(int)(spinTimer += Time.deltaTime * 8.0f) % 6];
		if (despawnTimer <= DESPAWN_TIME)
		{
			shine.sortingOrder = LayeredRender.MAX_Y * 2 - 1;
		}
	}
	#endregion

	#region Utility Methods (Empty)
			#endregion

	#region Coroutines (Empty)
			#endregion
}