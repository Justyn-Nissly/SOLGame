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
		Any,
	}
	#endregion

	#region Public Variables
	public WeaponTag weaponDestroysObject;
	public List<DoorLogic> doorsUnlocked = new List<DoorLogic>(); // all doors that will be unlocked when this destructible object is destroyed (this list can be empty)
	public Sprite destroyedSprite; // the sprite that is changed to when this destructible object is destroyed 
	public int health;
	public GameObject itemDrop;
	public float percentDropChance;
	#endregion

	#region Private Variables
	private DoorManager doorManager = new DoorManager(); // Used to unlock doors if applicable
	private float damageTimer;
	#endregion

	// Unity Named Methods
	#region Main Methods

	private void Start()
	{
		// add all doors to the door manager
		doorManager.doors.AddRange(doorsUnlocked);
		new UnityEngine.Random();
	}

	private void FixedUpdate()
	{
		damageTimer -= (damageTimer >= 0.0f) ? Time.deltaTime : 0.0f;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// check if the right weapon is whats hitting this object
		if (weaponDestroysObject == WeaponTag.Any && 
			 (collision.gameObject.CompareTag(ConvertTagToString(WeaponTag.HeavyMelee)) || 
			  collision.gameObject.CompareTag(ConvertTagToString(WeaponTag.LightMelee)) || 
			  collision.gameObject.CompareTag(ConvertTagToString(WeaponTag.Ranged))))		  
		{
			damageObject();
		}
		else if (collision.gameObject.CompareTag(ConvertTagToString(weaponDestroysObject)) && damageTimer <= 0.0f)
		{
			damageObject();
		}
	}
	#endregion

	#region Utility Methods (Empty)
	private void damageObject()
	{
		if (--health <= 0)
		{
			if (canDropItem)
			{
				Instantiate(itemDrop, transform.position, Quaternion.identity);
			}
			DestroyObject();
		}
		damageTimer = 0.2f;
	}

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
	public virtual void DestroyObject()
	{
		// unlock any doors that need to be unlocked
		doorManager.UnlockDoors();

		// change to the destroyed sprite if there is one else destroy this gameobject
		if (destroyedSprite != null)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
			if (gameObject.GetComponent<BoxCollider2D>() != null)
			{
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
			}
			if (gameObject.GetComponent<PolygonCollider2D>() != null)
			{
				gameObject.GetComponent<PolygonCollider2D>().enabled = false;
			}
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