using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : SceneMusic
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public bool
		isGuardian;
	#endregion

	#region Private Variables
	private bool
		playerIsPresent,
		enemyIsPresent;
	private AudioSource
		bossTheme;
	private float
		guardianFade,
		songDelay;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected override void Awake()
	{
		base.Awake();
		songDelay = 0.6f;
		playThisSong = false;
		bossTheme = songObject.GetComponent<AudioSource>();
		bossTheme.volume = 1.0f - (guardianFade = (isGuardian) ? 1.0f : 0.0f);
	}

	protected override void FixedUpdate()
	{
		enemyIsPresent = (GameObject.FindWithTag("Enemy") != null);

		if (enemyIsPresent && playerIsPresent)
		{
			playThisSong = true;
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, sceneCamera.transform.position,
			                                             Time.deltaTime * 0.75f);
		}
		else if (bossTheme.isPlaying && songDelay <= 0.0f)
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.25f);
		}

		if (playThisSong)
		{
			if (songDelay > 0.0f)
			{
				songDelay -= Time.deltaTime;
			}
			else
			{
				PlayBossSong();
				if (bossTheme.volume < 1.0f && guardianFade > 0.0f)
				{
					bossTheme.volume += Time.deltaTime * 2.5f;
					guardianFade -= Time.deltaTime * 2.5f;
				}
			}
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