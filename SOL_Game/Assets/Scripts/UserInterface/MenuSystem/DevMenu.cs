using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevMenu : MonoBehaviour
{
	#region Enums

	#endregion

	#region Public Variables

	#endregion

	#region Private Variables

	#endregion

	// Unity Named Methods
	private void Start()
	{
		// select the hub be default (needed to work right)
		GameObject.Find("TheHub").GetComponent<Button>().Select();

		// freeze player moment because we are in a menu
		GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
		if(playerGO != null)
		{
			playerGO.GetComponent<Player>().FreezePlayer();
		}
	}

	#region Main Methods
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Delete))
		//{
		//	SceneManager.LoadScene("DevMenu");
		//}
	}
	#endregion

	#region Utility Methods
	/// Load the Hub scene
	public void LoadHub()
	{
		SceneManager.LoadScene("Hub");
	}

	/// Load the Bio-Lab scene
	public void LoadBioLab()
	{
		SceneManager.LoadScene("Biolab (First facility)");
	}

	/// Load the Atlantis scene
	public void LoadAtlantis()
	{
		SceneManager.LoadScene("Atlantis (second facility)");
	}

	/// Load the Factory level 1 scene
	public void LoadFactory1()
	{
		SceneManager.LoadScene("Factory (third facility level 1)");
	}

	/// Load the Factory level 2 scene
	public void LoadFactory2()
	{
		SceneManager.LoadScene("Factory (third facility level 2)");
	}

	/// Load the Geo-Thermal Plant level 1 scene
	public void LoadGeoThermalPlant1()
	{
		SceneManager.LoadScene("Geo-thermal plant (fourth facility level 1)");
	}

	/// Load the Geo-Thermal Plant level 2 scene
	public void LoadGeoThermalPlant2()
	{
		SceneManager.LoadScene("Geo-thermal plant (fourth facility level 2)");
	}

	/// Load the Adrics Lab level 1 scene
	public void LoadAdricsLab1()
	{
		SceneManager.LoadScene("Adrics lab (fifth facility level 1)");
	}

	/// Load the Adrics Lab level 2 scene
	public void LoadAdricsLab2()
	{
		SceneManager.LoadScene("Adrics lab (fifth facility level 2)");
	}

	/// Load the Adrics Lab level 3 scene
	public void LoadAdricsLab3()
	{
		SceneManager.LoadScene("Adrics lab (fifth facility level 3)");
	}
	/// Load the Dev Room scene
	public void LoadDevRoom()
	{
		SceneManager.LoadScene("DevRoom");
	}
	#endregion

	#region Coroutines

	#endregion
}