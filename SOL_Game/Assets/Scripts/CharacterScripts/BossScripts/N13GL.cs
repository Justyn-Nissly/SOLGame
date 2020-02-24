using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N13GL : MonoBehaviour
{
	#region Enums
	public enum GuardianType
	{
		shieldGuardian,
		gunGuardian,
		hammerGuardian,
		swordGuardian,
		finalGuardian
	}
	#endregion

	#region Public Variables

	#region Shared Variables
	public GuardianType
		currentGuardian; // The current guardian type
	public bool 
		typeIsChanged; // The current guardian type has been changed
	public int
		changething; // Change the thing
	#endregion
	#region Shield Guardian
	public GameObject
		shieldGuardianArm; // The arm of the shield guardian
	#endregion
	#region Gun Guardian
	public GameObject
		gunGuardianArm; // The arm of the gun guardian
	#endregion
	#region Hammer Guardian
	#endregion
	#region Sword Guardian
	#endregion
	#region Final Guardian
	public Animator
		armAnimator,   // The animator for the guardian's arm
		n13glAnimator; // The animator for N13GL
	#endregion
	#endregion

	#region Private Variables
	#region Shared Variables
	private Array
		allGuardianTypes; // All possible guardian types
	private System.Random
		randomGuardian; // The number of the random guardian to choose
	#endregion
	#region Shield Guardian
	#endregion
	#region Gun Guardian
	#endregion
	#region Hammer Guardian
	#endregion
	#region Sword Guardian
	#endregion
	#region Final Guardian
	#endregion
	#endregion

	// Unity Named Methods
	#region Main Methods
	// Start is called before the first frame update
	void Start()
    {
		allGuardianTypes = Enum.GetValues(typeof(GuardianType));
		typeIsChanged    = true;
		changething      = 0;
		currentGuardian = GuardianType.finalGuardian;
	}

    // Update is called once per frame
    void Update()
    {
		/*if(Input.GetKeyDown(KeyCode.Return))
		{
			currentGuardian = GuardianType.shieldGuardian;
			typeIsChanged = true;
		}
		if(Input.GetKeyDown(KeyCode.RightShift))
		{
			currentGuardian = GuardianType.gunGuardian;
			typeIsChanged = true;
		}*/

		if (changething == 5)
		{
			changething = 0;
			currentGuardian = (GuardianType)UnityEngine.Random.Range(0, 4);
			typeIsChanged = false;
		}

		// Check which guardian arm needs to be teleported in
		if(typeIsChanged == false)
		{
			switch(currentGuardian)
			{
				case GuardianType.finalGuardian:
				{
					// Spawn in the final guardian arm
					break;
				}
				case GuardianType.shieldGuardian:
				{
					// Spawn in the shield guardian arm
					typeIsChanged = true;
					shieldGuardianArm.SetActive(true);
					StartCoroutine(SpawnNewArm(shieldGuardianArm, gunGuardianArm));
					break;
				}
				case GuardianType.gunGuardian:
				{
					// Spawn in the gun guardian arm
					typeIsChanged = true;
					gunGuardianArm.SetActive(true);
					StartCoroutine(SpawnNewArm(gunGuardianArm, shieldGuardianArm));
					break;
				}
				case GuardianType.hammerGuardian:
				{
					// Spawn in the hammer guardian arm
					break;
				}
				case GuardianType.swordGuardian:
				{
					// Spawn in the sword guardian arm
					break;
				}
			};
		}

		// Check which attak pattern should be used
		switch (currentGuardian)
		{
			case GuardianType.finalGuardian:
			{
				// Execute the final guardian attack pattern
				FinalGuardianAttack();
				break;
			}
			case GuardianType.shieldGuardian:
			{
				// Execute the shield guardian attack pattern
				ShieldGuardianAttack();
				break;
			}
			case GuardianType.gunGuardian:
			{
				// Execute the gun guardian attack pattern
				GunGuardianAttack();
				break;
			}
			case GuardianType.hammerGuardian:
			{
				// Execute the hammer guardian attack pattern
				HammerGuardianAttack();
				break;
			}
			case GuardianType.swordGuardian:
			{
				// Execute the sword guardian attack pattern
				SwordGuardianAttack();
				break;
			}
		};
	}
	#endregion

	#region Utility Methods
	#region Shared Methods
	#endregion
	#region Shield Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void ShieldGuardianAttack()
	{
		Debug.Log("Shield Attack");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			changething += 1;
		}
	}
	#endregion
	#region Gun Guardian
	/// <summary> The attack pattern for the gun guardian </summary>
	public void GunGuardianAttack()
	{
		Debug.Log("Gun Attack");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			changething += 1;
		}
	}
	#endregion
	#region Hammer Guardian
	/// <summary> The attack pattern for the hammer guardian </summary>
	public void HammerGuardianAttack()
	{
		Debug.Log("Hammer Attack");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			changething += 1;
		}
	}
	#endregion
	#region Sword Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void SwordGuardianAttack()
	{
		Debug.Log("Sword Attack");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			changething += 1;
		}
	}
	#endregion
	#region Final Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void FinalGuardianAttack()
	{
		Debug.Log("Final Attack");
		if (Input.GetKeyDown(KeyCode.Return))
		{
			changething += 1;
		}
	}
	#endregion
	#endregion

	#region Coroutines
	#region Shared Coroutines
	#endregion
	#region Shield Guardian
	#endregion
	#region Gun Guardian
	#endregion
	#region Hammer Guardian
	#endregion
	#region Sword Guardian
	#endregion
	#region Final Guardian
	/// <summary> Switch the primary attack arm to the arm of another guardain</summary>
	private IEnumerator SpawnNewArm(GameObject armToSpawn, GameObject armToDespawn)
	{
		float percentageComplete = 0; // The percentage of completion the teleport animation is at

		// Set the arm that is to be teleported in to invisable
		armToSpawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 1;

		// Teleport the current arm away
		while (percentageComplete < 1)
		{
			armToDespawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(0f, 1f, percentageComplete);
			percentageComplete                                         += Time.deltaTime;
			yield return null;
		}

		// Teleport the new arm in
		percentageComplete = 0;
		while (percentageComplete < 1)
		{
			armToSpawn.GetComponent  <_2dxFX_NewTeleportation2>()._Fade = Mathf.Lerp(1f, 0f, percentageComplete);
			percentageComplete                                         += Time.deltaTime;
			yield return null;
		}
		armToDespawn.SetActive(false);
		armToSpawn.GetComponent<_2dxFX_NewTeleportation2>()._Fade = 0;
	}
	#endregion
	#endregion
}