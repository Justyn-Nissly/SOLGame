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
	public bool  beginingPosition            { get; set; }
	public bool   sword                      { get; set; }
	public bool   hammer                     { get; set; }
	public bool   blaster                    { get; set; }
	public bool   shield                     { get; set; }
	public float  currentHealth              { get; set; }
	public float  maxHealth                  { get; set; }
	public string currentLevel               { get; set; }
	public int    bossesDefeated             { get; set; }
	public int    guardiansDefeated          { get; set; }
	public int    hubCheckPoint              { get; set; }
	public int    biolabCheckPoint           { get; set; }
	public int    atlantisCheckPoint         { get; set; }
	public int    factoryCheckPoint          { get; set; }
	public int    factoryLevel2CheckPoint    { get; set; }
	public int    geothermalCheckPoint       { get; set; }
	public int    geothermalLevel2CheckPoint { get; set; }
	public int    spacebaseCheckPoint        { get; set; }
	public int    spacebaseLevel2CheckPoint  { get; set; }
	public int    spacebaseLevel3CheckPoint  { get; set; }
	public int    finalWyrmFightCheckPoint   { get; set; }

	public GameData(bool  startInBeginingPosition,           bool   swordUnlocked,
					bool  hammerUnlocked,                    bool   blasterUnlocked,
					bool  shieldUnlocked,                    float  currentHealth,
					float maxHealth,                         string currentPlayerLevel,
					int   bossesDefeated,                    int    guardiansDefeated,
					int   hubCheckPointReached,              int    biolabCheckPointReached,
					int   atlantisCheckPointReached,         int    factoryCheckPointReached,
					int   factoryLevel2CheckPointReached,    int    geothermalCheckPointReached,
					int   geothermalLevel2CheckPointReached, int    spacebaseCheckPointReached,
					int   spacebaseLevel2CheckPointReached,  int    spacebaseLevel3CheckPointReached,
					int   finalWyrmFightCheckPointReached)
	{
		this.beginingPosition           = startInBeginingPosition;
		this.sword                      = swordUnlocked;
		this.hammer                     = hammerUnlocked;
		this.blaster                    = blasterUnlocked;
		this.shield                     = shieldUnlocked;
		this.currentHealth              = currentHealth;
		this.maxHealth                  = maxHealth;
		this.currentLevel               = currentPlayerLevel;
		this.bossesDefeated             = bossesDefeated;
		this.guardiansDefeated          = guardiansDefeated;
		this.hubCheckPoint              = hubCheckPointReached;
		this.biolabCheckPoint           = biolabCheckPointReached;
		this.atlantisCheckPoint         = atlantisCheckPointReached;
		this.factoryCheckPoint          = factoryCheckPointReached;
		this.factoryLevel2CheckPoint    = factoryLevel2CheckPointReached;
		this.geothermalCheckPoint       = geothermalCheckPointReached;
		this.geothermalLevel2CheckPoint = geothermalLevel2CheckPointReached;
		this.spacebaseCheckPoint        = spacebaseCheckPointReached;
		this.spacebaseLevel2CheckPoint  = spacebaseLevel2CheckPointReached;
		this.spacebaseLevel3CheckPoint  = spacebaseLevel3CheckPointReached;
		this.finalWyrmFightCheckPoint   = finalWyrmFightCheckPointReached;
	}
}