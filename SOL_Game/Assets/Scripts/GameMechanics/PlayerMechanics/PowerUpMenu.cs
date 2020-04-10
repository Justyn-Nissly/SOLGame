using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpMenu : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public PowerUp[]
		powerUps; // Power ups to display
	#endregion

	#region Private Variables
	private SpriteRenderer
		sprite; // Reference the object's sprite
	private Player
		player; // Reference the player
	public float
		maxSize, // Graphic's maximum size
		size;    // Current size
	private PlayerControls
		inputActions; // Reference player controls
	private bool
		grow; // Check if the menu should grow or shrink
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		player = FindObjectOfType<Player>();
		sprite = GetComponent<SpriteRenderer>();
		size = 0.0f;
		maxSize = transform.localScale.x;
		inputActions = player.inputActions;
	}

	void FixedUpdate()
	{
		for (int i = 0; i < PowerUp.HEAL; i++)
		{
			if (powerUps[i].powerUpSprite.enabled = (size > 0.0f))
			{
				powerUps[i].powerUpSprite.color =
					new Color(powerUps[i].powerUpSprite.color.r, powerUps[i].powerUpSprite.color.g,
					          powerUps[i].powerUpSprite.color.b, player.powerUpTimers[i] * player.powerUpDivided);
			}
		}
		if (powerUps[PowerUp.HEAL].powerUpSprite.enabled = (size > 0.0f))
		{
			transform.localScale = new Vector3(size, size, 1.0f);
			powerUps[PowerUp.HEAL].powerUpSprite.color   =
			new Color(powerUps[PowerUp.HEAL].powerUpSprite.color.r, powerUps[PowerUp.HEAL].powerUpSprite.color.g,
			          powerUps[PowerUp.HEAL].powerUpSprite.color.b, (player.medKits > 0) ? 1.0f : 0.0f);
		}
		sprite.enabled = (size > 0.0f);

		if ((grow = player.usingPowerUp) && size < maxSize)
		{
			size += maxSize * 0.1f;
		}
		else if (grow == false && size >= 0.0f)
		{
			size -= maxSize * 0.1f;
		}

	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}