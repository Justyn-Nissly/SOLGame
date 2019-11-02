using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The parent class for ranged attacks
/// </summary>
public class RangedAttackBase : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Transform firePoint;
	public GameObject bulletPrefab;
	public SpriteRenderer GunSprite;
    public IntValue damageToGive;
    #endregion

    #region Private Variables
    #endregion

    // Unity Named Methods
    #region Main Methods

    #endregion

    #region Utility Methods
    /// <summary>
    /// Instantiates a bullet
    /// </summary>
    public virtual void Shoot()
	{
		EnableGun();
		Invoke("DisableGun", .5f);
	}

	public void EnableGun()
	{
		GunSprite.enabled = true;
	}

	public void DisableGun()
	{
		GunSprite.enabled = false;
	}
	#endregion

	#region Coroutines
	#endregion
}
