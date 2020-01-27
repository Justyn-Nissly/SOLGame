using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
	#region Enums
	#endregion

	#region Public Variables
	public GameObject passwordText;
	public Dropdown   firstNameField,
					  middleNameField,
					  lastNameField;
	List<string> names = new List<string> { "option1", "option2", "option3" };

	#endregion

	#region Private Variables
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
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	#region Utility Methods
	/// <summary> Generate new profile data </summary>
	public void GenerateNewProfile()
	{
		firstNameField.AddOptions(names);
		middleNameField.AddOptions(names);
		lastNameField.AddOptions(names);
		CreateRandomName();
	}

	/// <summary> Create a random name for the user </summary>
	public void CreateRandomName()
	{
		// Get a random option from the list of names
		firstNameIndex  = Random.Range(0, 3);
		middleNameIndex = Random.Range(0, 3);
		lastNameIndex   = Random.Range(0, 3);

		// Set the dropdown boxes to the new random index
		firstNameField.value  = firstNameIndex;
		middleNameField.value = middleNameIndex;
		lastNameField.value   = lastNameIndex;

		// Make the name values global
		Globals.firstName  = names[firstNameIndex];
		Globals.middleName = names[middleNameIndex];
		Globals.lastName   = names[lastNameIndex];
	}

	/// <summary> Set the profile name to the option the user has chosen </summary>
	public void SetProfileName()
	{
		// Set the name fields value into the name index
		firstNameIndex  = firstNameField.value;
		middleNameIndex = middleNameField.value;
		lastNameIndex   = lastNameField.value;

		// Make the new name values global
		Globals.firstName  = names[firstNameIndex];
		Globals.middleName = names[middleNameIndex];
		Globals.lastName   = names[lastNameIndex];
	}

	/// <summary> Create profile and start a new game </summary>
	public void CreateProfile()
	{
		//FindObjectOfType<AudioManager>().StartBackground();
		m_Path = Application.dataPath;
		Globals.password = passwordText.GetComponent<TMPro.TextMeshProUGUI>().text;
		Debug.Log(Globals.firstName + " " + Globals.middleName + " " + Globals.lastName);
		SaveSystem.SaveGame();
		//Output the Game data path to the console

		//Debug.Log(Globals.firstName);
		//GameData.password = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<TMPro.TextMeshProUGUI>().text;
		/*Debug.Log("dataPath : " + m_Path);
		GameData.middleName = names[index];
		GameData.lastName   = names[index];
		GameData.password   = password.GetComponent<PasswordGenerator>().GetRandomPassword();
		SceneManager.LoadScene("Hub");*/
	}
	#endregion

	#region Coroutines
	#endregion
}
