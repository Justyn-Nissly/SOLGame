using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N13GL : MonoBehaviour
{
	#region Enums
	public enum AttackPattern
	{
		shieldGuardianPattern,
		gunGuardianPattern,
		hammerGuardianPattern,
		swordGuardianPattern,
		finalGuardianPattern
	}
	#endregion

	#region Public Variables

	#region Shared Variables
	public AttackPattern
		currentGuardianPattern; // The current guardian attack pattern type
	public GameObject
		currentGuardian, // The current guardian that was chosen
		nextGuardian;    // The next guardian that will be chosen
	public Sprite
		gunSprite,     // The sprite for the gun    guardian arm
		hammerSprite,  // The sprite for the hammer guardian arm
		nextArmSprite, // The sprite for the next   guardian arm to be spawned in
		shieldSprite,  // The sprite for the shield guardian arm
		swordSprite;   // The sprite for the sword  guardian arm
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
		allGuardianPatternTypes; // All possible guardian attack pattern types
	private System.Random
		randomGuardianPattern; // The number of the random guardian attack pattern to choose
	private Color32 guardianColour = new Color32(0x3C, 0x71, 0x6F, 0xFF);
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
		allGuardianPatternTypes = Enum.GetValues(typeof(AttackPattern));
		typeIsChanged    = true;
		changething      = 0;
		currentGuardianPattern = AttackPattern.finalGuardianPattern;
	}

    // Update is called once per frame
    void Update()
    {
		if (changething == 5)
		{
			changething = 0;
			currentGuardianPattern = (AttackPattern)UnityEngine.Random.Range(0, 4);
			typeIsChanged = true;
			currentGuardian = shieldGuardianArm;
		}

		// Check which guardian arm needs to be teleported in
		if(typeIsChanged == true)
		{
			switch(currentGuardianPattern)
			{
				case AttackPattern.finalGuardianPattern:
				{
					// Spawn in the final guardian arm
					break;
				}
				case AttackPattern.shieldGuardianPattern:
				{
					// Spawn in the shield guardian arm
					typeIsChanged = false;
					//shieldGuardianArm.SetActive(true);
					nextArmSprite = shieldSprite;
					GetComponent<Animator>().SetTrigger("SwitchArm");
					/*StartCoroutine(SpawnNewArm(shieldGuardianArm, gunGuardianArm));*/
					break;
				}
				case AttackPattern.gunGuardianPattern:
				{
					// Spawn in the gun guardian arm
					typeIsChanged = false;
					//gunGuardianArm.SetActive(true);
					nextArmSprite = gunSprite;
					GetComponent<Animator>().SetTrigger("SwitchArm");
					/*shieldGuardianArm.GetComponent<_2dxFX_NewTeleportation2>().TeleportationColor = guardianColour;*/
					/*StartCoroutine(SpawnNewArm(gunGuardianArm, shieldGuardianArm));*/
					break;
				}
				case AttackPattern.hammerGuardianPattern:
				{
					// Spawn in the hammer guardian arm
					break;
				}
				case AttackPattern.swordGuardianPattern:
				{
					// Spawn in the sword guardian arm
					break;
				}
			};
		}

		// Check which attak pattern should be used
		switch (currentGuardianPattern)
		{
			case AttackPattern.finalGuardianPattern:
			{
				// Execute the final guardian attack pattern
				FinalGuardianAttackPattern();
				break;
			}
			case AttackPattern.shieldGuardianPattern:
			{
				// Execute the shield guardian attack pattern
				ShieldGuardianAttackPattern();
				break;
			}
			case AttackPattern.gunGuardianPattern:
			{
				// Execute the gun guardian attack pattern
				GunGuardianAttackPattern();
				break;
			}
			case AttackPattern.hammerGuardianPattern:
			{
				// Execute the hammer guardian attack pattern
				HammerGuardianAttackPattern();
				break;
			}
			case AttackPattern.swordGuardianPattern:
			{
				// Execute the sword guardian attack pattern
				SwordGuardianAttackPattern();
				break;
			}
		};
	}
	#endregion

	#region Utility Methods
	#region Shared Methods
	public void SwitchArms()
	{
		shieldGuardianArm.GetComponent<SpriteRenderer>().sprite = nextArmSprite; // Take this and set this to "nextSprite" rather than just shieldSprite....It is 3:37...go to bed...
	}
	#endregion
	#region Shield Guardian
	/// <summary> The attack pattern for the shield guardian </summary>
	public void ShieldGuardianAttackPattern()
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
	public void GunGuardianAttackPattern()
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
	public void HammerGuardianAttackPattern()
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
	public void SwordGuardianAttackPattern()
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
	public void FinalGuardianAttackPattern()
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