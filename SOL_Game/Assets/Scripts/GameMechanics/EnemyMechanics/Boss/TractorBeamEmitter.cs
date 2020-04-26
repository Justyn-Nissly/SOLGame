using UnityEngine;

public class TractorBeamEmitter : MonoBehaviour
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		tractorBeam; // Instantiate tractor beams
	public bool
		emitting; // The tractor beam is on
	public float
		beamDelay; // Time between emitting beams
	#endregion

	#region Private Variables
	private float
		beamTimer; // Time left before launching another beam
	#endregion

	// Unity Named Methods
	#region Main Methods
	/// <summary> Set time between firing beams </summary>
	void Start()
	{
		beamTimer = beamDelay;
	}

	/// <summary> Fire a beam after the counter counts down </summary>
	void FixedUpdate()
	{
		// Fire only if the tractor beam is enabled
		if (emitting)
		{
			if (beamTimer > 0.0f)
			{
				beamTimer -= Time.deltaTime;
			}
			else
			{
				beamTimer = beamDelay;
				//Instantiate(tractorBeam, transform.position, Quaternion.identity);  this throws the player off the map if using the shield
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}