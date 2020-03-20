using System.Collections;
using UnityEngine;

public class Shard : QuestItem
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SpriteRenderer
		shine;
	public AudioSource
		shimmer;
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
			if (shimmer.volume > 0.0f)
			{
				shimmer.volume -= Time.deltaTime * 0.25f;
			}
		}
		else
		{
			shimmer.volume =
				1.0f / (Mathf.Pow(Vector2.Distance(transform.position, player.transform.position), 0.6f) + 0.01f) - 0.2f;
			if (shimmer.volume < 0.0f)
			{
				shimmer.volume = 0.0f;
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
			#endregion

	#region Coroutines (Empty)
			#endregion
}