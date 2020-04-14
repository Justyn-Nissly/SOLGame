using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthIncrease : QuestItem
{
	public int itemNumber;
	public Sprite[] sprites;
	public SpriteRenderer glow;
	private float spinTimer;

	public override void Awake()
	{
		base.Awake();
		if (Globals.acquiredHealthIncrease[itemNumber])
		{
			Destroy(gameObject);
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		spinTimer += Time.deltaTime * 8.0f;
		sprite.sprite = sprites[(int) (spinTimer % 4.0f)];
		if (despawnTimer <= QuestItem.DISAPPEAR)
		{
			glow.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	public override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !other.isTrigger)
		{
			base.OnTriggerEnter2D(other);
			{
				if (pickUp)
				{
					if (heartContainers.runTimeValue < Globals.MAX_PLAYER_HEALTH)
					{
						heartContainers.runTimeValue += 1;
					}
					Globals.acquiredHealthIncrease[itemNumber] = true;
					other.GetComponent<Player>().playerHealthHUD.ChangeNumberOfHearts();
					glow.sortingOrder = LayeredRender.MAX_Y * 2 - 1;
				}
			}
		}
	}
}