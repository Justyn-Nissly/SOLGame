using UnityEngine;

public class SwordThrow : MonoBehaviour
{
	#region Public Variables
	public float
		speed;
	public int
		Throwtype;
	public GameObject
        arm,    // Left arm of guardian
		origin; // Starting point
	public bool
		findTarget; // The target is not set
	public Animator
		anim; // Reference to change animation states
	#endregion

	#region Private Variables
	private Vector2
		targetPos; // The target's position
	private float
		spinAngle,     // The sword spins
		radius,        // Radial distance compared to the angle
		distance,      // Distance from origin to target
		externalAngle, // Initial angle from the origin to the target
		moveAngle;     // The current angle of the throw
	private Player
		player; // Reference the player
	private bool
		returnOrigin,
		isThrowing;
	private MeleeGuardian
		guadianCanMove;
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Initialize the missile </summary>
	void Start()
	{
		guadianCanMove = FindObjectOfType<MeleeGuardian>();
		spinAngle  = 0.0f;
		player     = FindObjectOfType<Player>();
		isThrowing = false;
	}

	/// <summary> Lock on to the target and arc towards it </summary>
	void FixedUpdate()
	{
		ThrowSword(Throwtype);
	}

    public void CanAttack()
    {
		findTarget = true;
    }
	#endregion

	#region Utility Functions
	public void ThrowSword(int type)
	{

		if (findTarget)
		{
			isThrowing = true;
			findTarget = false;
			targetPos = player.transform.position;
			//origin.transform.position = arm.transform.position;
			moveAngle = 0.0f;
			externalAngle = Mathf.Atan2(targetPos.y - origin.transform.position.y,
										targetPos.x - origin.transform.position.x);
			distance = Vector2.Distance(origin.transform.position, targetPos);
		}
		else if (type == 1 && isThrowing)
		{
			if (Vector2.Distance(arm.transform.position, targetPos) <= 0.05f)
			{
				returnOrigin = true;
				anim.SetTrigger("Return");
			}
			else if (Vector2.Distance(arm.transform.position, origin.transform.position) <= 0.05f)
			{
				targetPos = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
				isThrowing = !returnOrigin;
				if (returnOrigin)
				{
					arm.transform.position = origin.transform.position;

					if(guadianCanMove != null)
						guadianCanMove.moving = false;

					anim.SetTrigger("Patrol");
					returnOrigin = false;
					isThrowing = false;
				}
			}
			if (isThrowing)
			{
				arm.transform.position = (returnOrigin) ? Vector2.Lerp(arm.transform.position, origin.transform.position, speed * Time.deltaTime) :
				                                          Vector2.Lerp(arm.transform.position, targetPos, speed * Time.deltaTime);
			}
		}
		else if (type == 2 && isThrowing)
		{
			radius = Mathf.Cos(2.0f * moveAngle * Mathf.Deg2Rad - Mathf.PI * 0.5f);
			arm.transform.position = (Vector2)origin.transform.position +
						 new Vector2(Mathf.Cos(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f),
									  Mathf.Sin(moveAngle * Mathf.Deg2Rad + externalAngle - Mathf.PI * 0.25f)).normalized *
									  radius * distance;
			isThrowing = ((moveAngle += speed * 0.4f) < 90.0f);
		}
	}
	#endregion
}