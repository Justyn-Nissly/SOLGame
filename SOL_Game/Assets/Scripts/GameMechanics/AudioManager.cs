using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioClip enemyDeath;

	public static AudioSource enemyAudioSRC;

	void Start()
	{
		enemyAudioSRC = GetComponent<AudioSource>();
		enemyAudioSRC.clip = enemyDeath;
	}
	public void PlaySound()
	{
		enemyAudioSRC.Play();
	}
}
