using UnityEngine;

public class Explosion : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public float
		spreadRate, // How fast the explosion spreads
		maxSpread;  // How far the explosion can reach
	#endregion

	#region Private Variables
	private AudioSource
		source; // Make the sound
	private float
		spread,
		spinAngle; // Make the explosion spin
	private SpriteRenderer
		sprite;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the explosion and play its sound </summary>
	void Awake()
	{
		spread    = 0.0f;
		spinAngle = 0.0f;
		source    = GetComponent<AudioSource>();
		sprite    = GetComponent<SpriteRenderer>();
		source.Play();
	}

	/// <summary> Get the angle to the player and spread the explosion </summary>
	void FixedUpdate()
	{
		if ((spread += spreadRate) > maxSpread * 0.7f)
		{
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b,
			                         1.0f - (spread - maxSpread * 0.7f) / (maxSpread * 0.3f));
		}

		// The explosion expands until it reaches its max size
		if (spread < maxSpread)
		{
			transform.localScale += new Vector3(spreadRate, spreadRate, 0.0f);
			SpinExplosion();
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Utility Methods
	/// <summary> Make the explosion spin (aesthetic effect) </summary>
	void SpinExplosion()
	{
		transform.rotation = Quaternion.AngleAxis(spinAngle -= 4.5f, Vector3.forward);
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}