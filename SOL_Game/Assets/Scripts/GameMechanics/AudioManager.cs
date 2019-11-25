using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip enemyDeath;
	public AudioSource enemyAudioSRC;

	void Start()
	{
		enemyAudioSRC = GetComponent<AudioSource>();
		
		enemyAudioSRC.clip = enemyDeath;
		
	}
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
}
