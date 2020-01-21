using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PasswordGenerator : MonoBehaviour
{
	public const int LETTERS_LENGTH = 4,
		             NUMBERS_LENGTH = 4;
	public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(GetRandomPassword());
		text.GetComponent<TMPro.TextMeshProUGUI>().text = GetRandomPassword();
	}
	public String GetRandomPassword()
	{
		Random
			random = new Random(); // Random numbers
		String
			password = ""; // The random string

		// The password starts with a sequence of random numbers
		for (int digit = 1; digit <= NUMBERS_LENGTH; digit++)
		{
			password += (char)random.Next((int)'0', (int)':');
		}

		password += '-';

		// The password ends with a sequence of random letters K-O
		for (int digit = 1; digit <= LETTERS_LENGTH; digit++)

		{
			password += (char)random.Next((int)'K', (int)'P');
		}
		return password;
	}
}