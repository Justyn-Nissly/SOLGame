using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyrmMusic : SceneMusic
{
	#region Enums (Empty)
	#endregion

	#region Public Variables (Empty)
	#endregion

	#region Private Variables
	private bool
		bossIsPresent;
	private AudioSource
		wyrmTheme;
	private Wyrm
		wyrm;
	#endregion

	// Unity Named Methods
	#region Main Methods
	protected override void Awake()
	{
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

		if (bossIsPresent)
		{
			if (wyrmTheme.isPlaying == false)
			{
				wyrmTheme.Play();
			}

			songObject.transform.position = Vector2.Lerp(songObject.transform.position, player.transform.position,
			                                             Time.deltaTime * 0.75f);
		}
		else
		{
			songObject.transform.position = Vector2.Lerp(songObject.transform.position, muteLocation.transform.position,
			                                             Time.deltaTime * 0.25f);
		}
	}
	#endregion

	#region Utility Methods (Empty)
	#endregion

	#region Coroutines (Empty)
	#endregion
}