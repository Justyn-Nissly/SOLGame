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
	public Transform gunSpawnPoint;
	public GameObject bulletPrefab;
	public GameObject gunPrefab;
	public FloatValue damageToGive;
	public float BulletShootingDelay = .2f;
	#endregion

	#region Private Variables
	#endregion

	// Unity Named Methods
	#region Main Methods

	#endregion

	#region Utility Methods
	public RangedAttackBase()
	{

	}

	public RangedAttackBase(Transform _firePoint, Transform _gunSpawnPoint, GameObject _bulletPrefab, GameObject _gunPrefab, FloatValue _damageToGive, float _BulletShootingDelay)
	{
		firePoint = _firePoint;
		bulletPrefab = _bulletPrefab;
		damageToGive = _damageToGive;
		BulletShootingDelay = _BulletShootingDelay;
		gunSpawnPoint = _gunSpawnPoint;
		gunPrefab = _gunPrefab;
	}


	/// <summary>
	/// Instantiates the gun and a bullet
	/// </summary>
	public virtual void Shoot()
	{
		InstantiateAndDestroyGun();

		if (BulletShootingDelay == 0)
		{
			InstantiateBullet();
		}
		else
		{
			Invoke("InstantiateBullet", BulletShootingDelay);
		}
	}

	public void InstantiateAndDestroyGun()
	{
		GameObject gunInstance = Instantiate(gunPrefab, gunSpawnPoint.position, gunSpawnPoint.rotation);
		gunInstance.transform.SetParent(gunSpawnPoint);

		Destroy(gunInstance, .5f);
	}

	#endregion

	#region Coroutines

	public void InstantiateBullet()
	{
		GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		BulletLogic bulletLogic = bulletInstance.GetComponent<BulletLogic>();
		bulletLogic.bulletDamage = (int)damageToGive.initialValue; // is this right
	}
	#endregion
}
