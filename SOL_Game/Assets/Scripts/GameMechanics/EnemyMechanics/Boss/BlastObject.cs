using UnityEngine;

public class BlastObject : MonoBehaviour
{
	#region Enums and Defined Constants (Empty)
	#endregion

	#region Public Variables
	public float
		delayTime; // Starting time until the blast appears
	#endregion

	#region Private Variables
	private float
		delayTimer; // Time until the blast appears
	private bool
		isBlasting; // Check if the blasts are appearing
	private CircleCollider2D
		blastCollider; // Makes the player take damage if caught in a blast
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Start the blast delay </summary>
	void Start()
	{
		delayTimer            = delayTime;
		isBlasting            = true;
		blastCollider         = GetComponent<CircleCollider2D>();
	}

	/// <summary> Count down the blast timer then blast </summary>
	void FixedUpdate()
	{
		// Count down and do no damage to the player before the actual blast
		delayTimer            -= Time.deltaTime;
		blastCollider.enabled  = false;

		// Make the blast appear
		if (delayTimer <= 0.0f && isBlasting == true)
		{
			// Replace the line below with the animation
			transform.localScale  *= (new Vector2(4.0f, 4.0f));
			isBlasting             = false;
			blastCollider.enabled  = true;
		}

		// After the blast the object despawns
		if (delayTimer <= -1.6f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary> Damage the player if caught in the blast </summary>
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			// Replace the line below with actual damage code
			Debug.Log("Ouch!");
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}