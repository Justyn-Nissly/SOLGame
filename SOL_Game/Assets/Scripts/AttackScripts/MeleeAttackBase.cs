using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBase : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public Transform attackPosition;
	public float attackRange;
	public LayerMask willDamageLayer;
	public GameObject weapon;
	public int damage;
	#endregion

	#region Private Variables
	private int force = 100;
	#endregion

	// Unity Named Methods
	#region Main Methods
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPosition.position, attackRange);
	}
	#endregion

	#region Utility Methods
	public void Attack()
	{
		Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, willDamageLayer);

		foreach (Collider2D collider in enemiesToDamage)
		{
			BaseCharacter characterBeingAtacked = collider.GetComponent<BaseCharacter>();
			if (characterBeingAtacked != null)
			{
				characterBeingAtacked.TakeDamage(damage);


				//ApplyKnockBack(collider.gameObject);

			}
		}

		GameObject weaponInstance = Instantiate(weapon, attackPosition.transform);
		Destroy(weaponInstance, .3f);
	}

	private void ApplyKnockBack(GameObject characterBeingAtacked)
	{
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();
		if (rigidbody2D != null)
		{
			Vector2 vector2 = DegreeToVector2(gameObject.transform.eulerAngles.z + 90) * force;
			rigidbody2D.AddForce(vector2, ForceMode2D.Impulse);
		}
	}

	public Vector2 DegreeToVector2(float degree)
	{
		return RadianToVector2(degree * Mathf.Deg2Rad);
	}

	public Vector2 RadianToVector2(float radian)
	{
		return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
	}
	#endregion

	#region Coroutines
	#endregion
}
