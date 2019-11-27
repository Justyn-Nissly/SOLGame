using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public AudioClip
		enemyDeath;    // The enemy death sound
	public AudioSource
		enemyAudioSRC; // The audio source to control enemy souds
	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		enemyAudioSRC = GetComponent<AudioSource>();

		enemyAudioSRC.clip = enemyDeath;

	}
	#endregion

	#region Utility Methods
	/*public void StartBackground()
	{
		backgroudSRC.Play();
	}
	public void StopBackground()
	{
		backgroudSRC.Stop();
	}*/
	public void PlaySound()
	{
		enemyAudioSRC.Play();
	}
	#endregion

	#region Coroutines

	#endregion
}