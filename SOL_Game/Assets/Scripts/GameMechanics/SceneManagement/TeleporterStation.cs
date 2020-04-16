using UnityEngine;

public class TeleporterStation : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public SpriteRenderer
		portal;
	public ConveyorBelt
		tractorField;
	public CircleCollider2D
		teleportPoint;
	public int
		teleporterOrder; // Order in which the teleporters are unlocked
	public bool
		spawnHere;
	#endregion

	#region Private Variables
	private float
		returnTimer;
	private bool
		canReturn;
	private Player
		player;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Turn on the conveyor belt </summary>
	void Awake()
	{
		returnTimer = 7.0f;
		player = FindObjectOfType<Player>();
		UpdateTeleporterStatus();
	}

	/// <summary> Turn on the conveyor belt </summary>
	void FixedUpdate()
	{
		if (canReturn == false)
		{
			returnTimer -= Time.deltaTime;
			if (returnTimer <= 0.0f && teleporterOrder == Globals.bossesDefeated || Globals.wyrmDefeated)
			{
				canReturn = true;
				tractorField.direction = ConveyorBelt.Direction.Up;
			}
			else if (returnTimer <= 1.0f)
			{
				teleportPoint.enabled = true;
				tractorField.GetComponent<BoxCollider2D>().isTrigger = (teleporterOrder == Globals.bossesDefeated || Globals.wyrmDefeated);
			}
			else if (returnTimer >= 2.9f && returnTimer <= 3.0f)
			{
				tractorField.direction = ConveyorBelt.Direction.Down;
				player.UnFreezePlayer();
				player.playerMovementAmount = Vector2.down * 0.01f;
				player.playerMovementAmount = Vector2.zero;
			}
			else if (returnTimer > 3.0f)
			{
				player.FreezePlayer();
			}
		}
	}
	#endregion

	#region Utility Methods
	public void UpdateTeleporterStatus()
	{
		portal.enabled = (teleporterOrder == Globals.bossesDefeated || spawnHere || Globals.wyrmDefeated);
		tractorField.GetComponent<BoxCollider2D>().isTrigger = teleporterOrder == Globals.bossesDefeated ||
		                                                       spawnHere || Globals.wyrmDefeated;
		teleportPoint.enabled = (teleporterOrder == Globals.bossesDefeated || Globals.wyrmDefeated);
		canReturn = false;
		tractorField.direction = ConveyorBelt.Direction.Up;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}