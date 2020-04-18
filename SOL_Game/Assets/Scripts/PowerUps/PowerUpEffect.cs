using System.Collections;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public AudioSource
		activateShield,
		activatePower,
		activateSpeed,
		heal,
		pickup;
	public SpriteRenderer
		powerSprite;
	public GameObject
		fadeSprite;
	public Color
		playerColor,
		shieldColor;
	public Material
		startMaterial,
		shieldMaterial;
	public _2dxFX_NewTeleportation2
		playerMaterial;
	#endregion

	#region Private Variables
	private Player
		player;
	private SpriteRenderer
		playerSprite;
	private float
		alpha;
	private bool
		spriteTrail,
		speedJustUsed;
	private int
		medkits;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Determine the power up type </summary>
	public virtual void Start()
	{
		player = FindObjectOfType<Player>();
		playerSprite = player.GetComponent<SpriteRenderer>();
		playerColor = playerSprite.color;
		startMaterial = playerSprite.material;
		playerMaterial = player.GetComponent<_2dxFX_NewTeleportation2>();
		powerSprite = GetComponent<SpriteRenderer>();
		powerSprite.color = Color.red;
		alpha = 0.0f;
		shieldColor = new Color(0.2f, 1.0f, 0.6f, 1.0f);
	}

	/// <summary> Power ups eventually disappear after dropping </summary>
	void FixedUpdate()
	{
		if (medkits != player.medKits)
		{
			if (medkits > player.medKits)
			{
				Debug.Log("HEAL");
				heal.Play();
			}
			medkits = player.medKits;
		}

		powerSprite.sortingOrder = playerSprite.sortingOrder - 5;

		spriteTrail = !spriteTrail;

		ApplyEffects();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("PowerUp"))
		{
			pickup.Play();
		}
	}
	#endregion

	#region Utility Methods
	private void ApplyEffects()
	{
		if (player.powerUpTimers[PowerUp.SHIELD] > 0.0f && player.powerUpsActive[PowerUp.SHIELD])
		{
			if (playerMaterial.enabled && activateShield.isPlaying == false)
			{
				activateShield.Play();
			}
			playerMaterial.enabled = false;
			playerSprite.material = shieldMaterial;
			playerSprite.color = shieldColor;
		}
		else
		{
			playerSprite.material = startMaterial;
			playerSprite.color = playerColor;
			playerMaterial.enabled = true;
		}

		if (player.powerUpTimers[PowerUp.POWER] > 0.0f && player.powerUpsActive[PowerUp.POWER])
		{
			if (alpha <= 0.0f && activatePower.isPlaying == false)
			{
				activatePower.Play();
			}
			alpha += (alpha < 1.0f) ? 0.1f : 0.0f;
			powerSprite.sprite = playerSprite.sprite;
			powerSprite.color = new Color(1.0f, 0.0f, 0.0f, alpha);
		}
		else
		{
			alpha -= (alpha > 0.0f) ? 0.1f : 0.0f;
			powerSprite.color = new Color(1.0f, 0.0f, 0.0f, alpha);
		}

		if (spriteTrail && (player.powerUpTimers[PowerUp.SPEED] > 0.0f && player.powerUpsActive[PowerUp.SPEED]))
		{
			if (speedJustUsed)
			{
				speedJustUsed = false;
				activateSpeed.Play();
			}
			Instantiate(fadeSprite, transform.position, Quaternion.identity);
		}
		else if (player.powerUpTimers[PowerUp.SPEED] <= 0.0f || player.powerUpsActive[PowerUp.SPEED] == false)
		{
			speedJustUsed = true;
		}
	}
	#endregion

	#region Coroutines
	#endregion
}