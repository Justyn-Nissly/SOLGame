using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthIncrease : QuestItem
{
    public FloatValue heartContainers;
    public FloatValue playerHealth;
	private float spinTimer;

	public override void Awake()
	{
		despawnTimer = 1000000.0f;
		sprite = GetComponent<SpriteRenderer>();
		player = GameObject.FindObjectOfType<Player>();
		spinTimer = 0.0f;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		spinTimer += Time.deltaTime * 8.0f;
		sprite.sprite = sprites[(int)(spinTimer % 4.0f)];
	}

	public override void OnTriggerEnter2D(Collider2D other)
   {
		base.OnTriggerEnter2D(other);
      if(other.CompareTag("Player") && !other.isTrigger)
      {
		if (heartContainers.runTimeValue < Globals.MAX_PLAYER_HEALTH)
		{
			heartContainers.runTimeValue += 1;
		}
         playerHealth.runTimeValue = playerHealth.initialValue = heartContainers.runTimeValue * 2.0f;
         other.transform.GetComponent<Player>().playerHealthHUD.ChangeNumberOfHearts();
      }
   }
}