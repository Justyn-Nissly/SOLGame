using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : SceneMusic
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	#endregion

	#region Private Variables
	private bool
		playerIsPresent,
		enemyIsPresent;
	private AudioSource
		bossTheme;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected override void Awake()
	{
		base.Awake();
		playThisSong = false;
		bossTheme = songObject.GetComponent<AudioSource>();
	}

	protected override void FixedUpdate()
	{
		enemyIsPresent = (GameObject.FindWithTag("Enemy") != null);

		if (enemyIsPresent && playerIsPresent)
		{
			playThisSong = true;
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, sceneCamera.transform.position,
			                                             Time.deltaTime * 4.0f);
		}
		else
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.25f);
		}

		if (playThisSong)
		{
			PlayBossSong();
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			playerIsPresent = true;
		}
	}

	protected override void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			playerIsPresent = false;
		}
	}
	#endregion

	#region Utility Methods
	void PlayBossSong()
	{
		if (bossTheme.isPlaying == false)
		{
			bossTheme.loop = true;
			bossTheme.Play();
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}