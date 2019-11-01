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
	protected float thrust = 10;
	protected float knockTime = .2f;
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


				ApplyKnockBack(collider.gameObject);

			}
		}

		GameObject weaponInstance = Instantiate(weapon, attackPosition.transform);
		Destroy(weaponInstance, .3f);
	}

	private void ApplyKnockBack(GameObject characterBeingAtacked)
	{
		Rigidbody2D rigidbody2D = characterBeingAtacked.GetComponent<Rigidbody2D>();

		// add knock back
		if(rigidbody2D != null && characterBeingAtacked.CompareTag("Player"))
		{
			rigidbody2D.isKinematic = false;
			Vector2 difference = characterBeingAtacked.transform.position - transform.position;
			difference = difference.normalized * thrust;

			rigidbody2D.AddForce(difference, ForceMode2D.Impulse);
			StartCoroutine(KnockCo(rigidbody2D));
		}
		else if (rigidbody2D != null && characterBeingAtacked.CompareTag("Enemy"))
		{
			//rigidbody2D.isKinematic = false;
			Vector2 difference = characterBeingAtacked.transform.position - transform.position;
			difference = difference.normalized * thrust;

			rigidbody2D.AddForce(difference, ForceMode2D.Impulse);
			StartCoroutine(KnockCoE(rigidbody2D));
		}
	}

	private IEnumerator KnockCo(Rigidbody2D rigidbody2D)
	{
		if(rigidbody2D != null)
		{
			yield return new WaitForSeconds(knockTime);
			if (rigidbody2D != null)
			{
				rigidbody2D.velocity = Vector2.zero;
				rigidbody2D.isKinematic = true;
			}
		}
	}

	private IEnumerator KnockCoE(Rigidbody2D rigidbody2D)
	{
		if (rigidbody2D != null)
		{
			yield return new WaitForSeconds(knockTime);
			if (rigidbody2D != null)
			{
				rigidbody2D.velocity = Vector2.zero;
				//rigidbody2D.isKinematic = true;
			}
		}
	}
	#endregion

	#region Coroutines
	#endregion
}
