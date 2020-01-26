using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables
	public GameObject passwordText;
	public Dropdown firstNameField,
					middleNameField,
					lastNameField;
	List<string> names = new List<string> { "option1", "option2", "option3" };
	#endregion

	#region Private Variables
	private GameObject pauseMenu;
	private string password;
	private int firstNameIndex,
				middleNameIndex,
				lastNameIndex;
	public static string passwordString;
	public string m_Path;//////////////////
	private PasswordGenerator passwordGenerator;
	#endregion

	// Unity Named Methods
	#region Main Methods
	void Start()
	{
		firstNameField.AddOptions(names);
		middleNameField.AddOptions(names);
		lastNameField.AddOptions(names);
		CreateRandomName();
	}
	/// Check every frame if the user has hit the "end" key to open the developer menu
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.End))
		{
			SceneManager.LoadScene("DevMenu");
		}
	}
	#endregion

	#region Utility Methods
	/// Create a new Game
	public void CreateNewGame()
	{
		int index = 2;
		//FindObjectOfType<AudioManager>().StartBackground();
		m_Path = Application.dataPath;
		Globals.password = passwordText.GetComponent<TMPro.TextMeshProUGUI>().text;
		Globals.firstName  = names[firstNameIndex];
		Globals.middleName = names[middleNameIndex];
		Globals.lastName   = names[lastNameIndex];
		Debug.Log(names[firstNameIndex] + " " + names[middleNameIndex] + " " + names[lastNameIndex]);
		//Output the Game data path to the console

		//Debug.Log(Globals.firstName);
		//GameData.password = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<TMPro.TextMeshProUGUI>().text;
		/*Debug.Log("dataPath : " + m_Path);
		GameData.middleName = names[index];
		GameData.lastName   = names[index];
		GameData.password   = password.GetComponent<PasswordGenerator>().GetRandomPassword();
		SceneManager.LoadScene("Hub");*/
	}
	
	/// Create a random name for the user
	public void CreateRandomName()
	{
		// Get a random option from the list of names
		firstNameIndex	= Random.Range(0, 3);
		middleNameIndex = Random.Range(0, 3);
		lastNameIndex   = Random.Range(0, 3);

		// Set the dropdown boxes to the new random index
		firstNameField.value  = firstNameIndex;
		Debug.Log(names[firstNameIndex] + " " + names[middleNameIndex] + " " + names[lastNameIndex]);
		Debug.Log("yay");
		//Globals.firstName = names[firstNameIndex];
		middleNameField.value = middleNameIndex;
		lastNameField.value   = lastNameIndex;
	}

	/// Set the profile name to the option the user has chosen
	public void SetProfileName()
	{
		firstNameIndex	= firstNameField.value;
		middleNameIndex = middleNameField.value;
		lastNameIndex	= lastNameField.value;
	}

	/// Load a game save
	public void LoadGame()
	{

	}

	/// Quits the game
	public void QuitGame()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
	#endregion
 
	#region Coroutines

	#endregion
}