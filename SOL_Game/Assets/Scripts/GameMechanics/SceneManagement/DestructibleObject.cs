using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	#region Enums
	[System.Serializable]
	public enum WeaponTag
	{
		HeavyMelee,
		LightMelee,
		Ranged,
	}
	#endregion

	#region Public Variables
	public WeaponTag weaponDestroysObject;
	public List<DoorLogic> doorsUnlocked = new List<DoorLogic>(); // all door that will be unlocked when this destructible object is destroyed (this list can be empty)
	public Sprite destroyedSprite; // the sprite that is changed to when this destructible object is destroyed 
	public int health;
	#endregion

	#region Private Variables
	private DoorManager doorManager = new DoorManager(); // this manager has the logic to control a list of door (in this case used to unlock them all)
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		// add all doors to the door manager and lock all the doors because they should be locked
		doorManager.doors.AddRange(doorsUnlocked);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(ConvertTagToString(weaponDestroysObject)))
		{
			// unlock any doors that need to be unlocked
			doorManager.UnlockAllDoors();

			// change to the destroyed sprite if there is one else destroy this gameobject
			if(destroyedSprite != null)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
			}
			else
			{
				Destroy(gameObject);
			}

		}
	}
	#endregion

	#region Utility Methods (Empty)
	private string ConvertTagToString(WeaponTag weaponTag)
	{
		string weaponTagString;

		if (weaponTag == WeaponTag.Ranged)
		{
			weaponTagString = "Projectile";
		}
		else if (weaponTag == WeaponTag.LightMelee)
		{
			weaponTagString = "PlayerLightWeapon";
		} 
		else
		{
			weaponTagString = "PlayerHeavyWeapon";
		}


		return weaponTagString;
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
