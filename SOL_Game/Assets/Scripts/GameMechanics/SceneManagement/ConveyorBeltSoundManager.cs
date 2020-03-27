using UnityEngine;

public class ConveyorBeltSoundManager : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public ConveyorBelt[]
		conveyorBelts;
	#endregion

	#region Private Variables
	private float
		checkTimer;
	private AudioSource
		sound;
	private bool
		active;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Awake()
	{
		checkTimer = 0.0f;
		sound = GetComponent<AudioSource>();
	}

	void FixedUpdate()
	{
		if ((checkTimer += Time.deltaTime) > 0.2f)
		{
			checkTimer = 0.0f;
			foreach (ConveyorBelt belt in conveyorBelts)
			{
				if (active = belt.isMoving)
				{
					break;
				}
			}
		}
		if (active)
		{
			sound.Play();
		}
	}
	#endregion

	#region Utility Methods
	#endregion

	#region Coroutines (Empty)
	#endregion
}