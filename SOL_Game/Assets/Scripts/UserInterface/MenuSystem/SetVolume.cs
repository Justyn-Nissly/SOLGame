using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
	#region Enums (Empty)

	#endregion

	#region Public Variables
	public AudioMixer
		mixer; // Reference to the AudioMixer

	public Slider
		slider; // Reference to the volume Slider
	#endregion

	#region Private Variables
	private float
		sliderValue; // The volume Slider's value
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		// Set the volumes starting value
		slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
	}
	#endregion

	#region Utility Methods
	/// <summary> This changes the volume to the current volume slider value </summary>
	public void SetLevel()
	{
		// Get the volume slider value
		sliderValue = slider.value;

		// Set the volume based on that new value
		mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

		// This saves the volume setting even after stopping and starting the game
		PlayerPrefs.SetFloat("MusicVolume", sliderValue);
	}
	#endregion

	#region Coroutines (Empty)

	#endregion
}
