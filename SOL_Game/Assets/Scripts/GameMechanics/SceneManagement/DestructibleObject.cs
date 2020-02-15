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
	public List<DoorLogic> doorsUnlocked = new List<DoorLogic>(); // all doors that will be unlocked when this destructible object is destroyed (this list can be empty)
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
		// add all doors to the door manager
		doorManager.doors.AddRange(doorsUnlocked);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// check if the right weapon is whats hitting this object
		if (collision.gameObject.CompareTag(ConvertTagToString(weaponDestroysObject)))
		{
			if(health > 0)
			{
				health--;
			}
			else
			{
				DestroyObject();
			}
		}
	}
	#endregion

	#region Utility Methods (Empty)
	/// <summary> converts a tag to its string value for string comparing</summary>
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

	/// <summary> call this method to destroy the destructible object </summary>
	private void DestroyObject()
	{
		// unlock any doors that need to be unlocked
		doorManager.UnlockDoors();

		// change to the destroyed sprite if there is one else destroy this gameobject
		if (destroyedSprite != null)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}
