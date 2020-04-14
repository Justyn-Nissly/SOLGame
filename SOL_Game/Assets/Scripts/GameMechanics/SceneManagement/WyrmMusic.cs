using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyrmMusic : SceneMusic
{
	#region Enums (Empty)
	#endregion

	#region Public Variables
	public GameObject
		intensity;
	#endregion

	#region Private Variables
	private bool
		bossIsPresent;
	private AudioSource
		wyrmTheme;
	private Wyrm
		wyrm;
	private float
		songDelay;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected override void Awake()
	{
		songDelay = 3.0f;
		playThisSong = false;
		wyrmTheme = songObject.GetComponent<AudioSource>();
	}

	protected override void FixedUpdate()
	{
		if ((wyrm = FindObjectOfType<Wyrm>()) != null)
		{
			bossIsPresent = wyrm.enabled;
		}
		else
		{
			bossIsPresent = false;
			player = FindObjectOfType<Player>();
		}

		PlayWyrmTheme();
	}
	#endregion

	#region Utility Methods
	private void PlayWyrmTheme()
	{
		if (bossIsPresent)
		{
			if (wyrmTheme.isPlaying == false && (songDelay -= Time.deltaTime) <= 0.0f)
			{
				wyrmTheme.Play();
				intensity.GetComponent<AudioSource>().loop = true;
				intensity.GetComponent<AudioSource>().Play();
			}

			songObject.transform.position = Vector2.Lerp(songObject.transform.position, player.transform.position,
			                                             Time.deltaTime * 0.75f);
			intensity.transform.position = Vector2.Lerp(intensity.transform.position,
			                                            player.transform.position + (Vector3) (Vector2.down *
			                                           (wyrm.maxHealth.runTimeValue / wyrm.maxHealth.initialValue) * 40.0f),
			                                            Time.deltaTime * 0.75f);
		}
		else
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.25f);
			intensity.transform.position = Vector2.Lerp(intensity.transform.position, muteLocation.transform.position,
			                                            Time.deltaTime * 0.25f);
		}
	}
	#endregion

	#region Coroutines (Empty)
	#endregion
}