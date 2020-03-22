using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
	private Vector3 normalSize;
	private bool isShacking = false;

	private float
		shackSpeed = 50.0f,              // How fast it shakes
		shackAmount = .01f;              // How much it shakes

	private void Start()
	{
		normalSize = transform.localScale;
		StartCoroutine(GrowToNormalSize());
	}

	private void FixedUpdate()
	{
		// If the enemy should be shacking start shacking the enemy
		if (isShacking)
		{
			transform.position = new Vector2(transform.position.x + (Mathf.Sin(Time.time * shackSpeed) * shackAmount), transform.position.y + (Mathf.Sin(Time.time * shackSpeed) * shackAmount));
		}
	}

	private IEnumerator GrowToNormalSize()
	{
		float percentageComplete = 0;
		isShacking = true;

		// play the pixel die effect from start(0) to finish(1)
		while (percentageComplete < 1)
		{
			transform.localScale = new Vector3(Mathf.Lerp(0f, normalSize.x, percentageComplete), Mathf.Lerp(0f, normalSize.y, percentageComplete), transform.localScale.z);
			percentageComplete += Time.deltaTime / 8;
			yield return null;
		}

		transform.localScale = normalSize;
		isShacking = false;
	}
}
