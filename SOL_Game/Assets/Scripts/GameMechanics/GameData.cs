using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int level;
	public int health;
	public float[] position; // This is used to replace Vector3 variabls because Vector3 variabls are not serializable

	public GameData(Player player)
	{
			/*level = player.level;
			helath = player.health;*/
		position = new float[3];
		position[0] = player.transform.position.x;
		position[0] = player.transform.position.y;
		position[0] = player.transform.position.z;
	}
}