using System;

[Serializable]
public class SaveData
{
	public PlayerData myPlayerData { get; set; }
	public string currentLevel     { get; set; }

	public SaveData()
	{

	}
}

[Serializable]
public class PlayerData
{
	public bool   beginingPosition  { get; set; }
	public bool   sword             { get; set; }
	public bool   hammer            { get; set; }
	public bool   blaster           { get; set; }
	public bool   shield            { get; set; }
	public float  playerHealth      { get; set; }
	public int    bossesDefeated    { get; set; }
	public int    guardiansDefeated { get; set; }
	public float health = 3.0f;

	public PlayerData(bool startInBeginingPosition, bool swordUnlocked,  bool  hammerUnlocked,
		              bool blasterUnlocked,         bool shieldUnlocked, float playerHealth,
					  int  bossesDefeated,          int  guardiansDefeated)
	{
		this.beginingPosition  = startInBeginingPosition;
		this.sword             = swordUnlocked;
		this.hammer            = hammerUnlocked;
		this.blaster           = blasterUnlocked;
		this.shield            = shieldUnlocked;
		this.playerHealth      = health;
		this.bossesDefeated    = bossesDefeated;
		this.guardiansDefeated = guardiansDefeated;
	}
}