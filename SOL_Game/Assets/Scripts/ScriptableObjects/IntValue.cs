using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject
{


    //Health variables
    public int startingPlayerHP;
    public int startingEnemyHP;


    //Player attack variables
    public int playerHeavyDamage;
    public int playerLightDamage;
    public int playerRangedDamage;

    //Enemy attack variable
    public int enemyHeavyDamage;
    public int enemyLightDamage;
    public int enemyRangedDamage;
    public int enemyShieldDamage;
}
