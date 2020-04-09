using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(N13GL))]
[CanEditMultipleObjects]
public class InspectorEditor : Editor
{
	/*override public void OnInspectorGUI()
	{
		var myScript = target as N13GL;

		myScript.isShield = EditorGUILayout.Toggle("Shield Guardian", myScript.isShield);

		using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isShield)))
		{
			if (group.visible == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Toggle("Is Charging", myScript.isHittingPlayer);
				EditorGUILayout.Toggle("Is Hitting Player", myScript.isHittingPlayer);
				EditorGUILayout.Toggle("Is Can Do Quarter HealthEvent", myScript.canDoQuarterHealthEvent);
				EditorGUILayout.Toggle("Is Charging", myScript.isCharging);
				
				EditorGUI.indentLevel--;
			}
		}

		myScript.isGun = EditorGUILayout.Toggle("Gun Guardian", myScript.isGun);

		using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(myScript.isGun)))
		{
			if (group.visible == true)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Toggle("Is Charging", myScript.isCharging);
				EditorGUILayout.Toggle("Is Hitting Player", myScript.isCharging);
				EditorGUILayout.Toggle("Is Can Do Quarter HealthEvent", myScript.isCharging);
				EditorGUILayout.Toggle("Is Charging", myScript.isCharging);

				EditorGUI.indentLevel--;
			
			}
		}
	}*/
}

/*isCharging                   = false, // Flag for if the enemy is charging at the player
		isStunned                    = false, // Flag for if the enemy is stunned
		isHittingPlayer              = false, // Flag for is the enemy is colliding with the player right now
		canDoHalfHealthEvent         = true,  // Flag so that this health event only happens once
		canDoQuarterHealthEvent      = true,  // Flag so that this health event only happens once
		canDoThreeQuarterHealthEvent = true,  // Flag so that this health event only happens once
		enemyIsShacking              = false, // For making the enemy look "mad"
		canShoot                     = true;  // Can the guardian shoot*/