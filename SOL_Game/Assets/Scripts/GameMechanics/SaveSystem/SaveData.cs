using System;

[Serializable]
public class SaveData
{
	public GameData gameData     { get; set; }

	public SaveData()
	{

	}
}

[Serializable]
public class GameData
{
	public string currentLevel      { get; set; }
	public bool  beginingPosition   { get; set; }
	public bool   sword             { get; set; }
	public bool   hammer            { get; set; }
	public bool   blaster           { get; set; }
	public bool   shield            { get; set; }
	public float  currentHealth     { get; set; }
	public float  maxHealth         { get; set; }
	public int    bossesDefeated    { get; set; }
	public int    guardiansDefeated { get; set; }

	public GameData(bool   startInBeginingPosition, bool swordUnlocked,  bool  hammerUnlocked,
		            bool   blasterUnlocked,         bool shieldUnlocked, float currentHealth,
					float  maxHealth,               int  bossesDefeated, int   guardiansDefeated,
					string currentLevel)
	{
		this.beginingPosition  = startInBeginingPosition;
		this.sword             = swordUnlocked;
		this.hammer            = hammerUnlocked;
		this.blaster           = blasterUnlocked;
		this.shield            = shieldUnlocked;
		this.currentHealth     = currentHealth;
		this.maxHealth         = maxHealth;
		this.bossesDefeated    = bossesDefeated;
		this.guardiansDefeated = guardiansDefeated;
		this.currentLevel      = currentLevel;
	}
}