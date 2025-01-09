using UnityEngine;

[System.Serializable]
public struct PlayerProfileData
{
    [SerializeField] int currentLevel;
    [SerializeField] FieldConfig fieldConfig;
    [SerializeField] PlayerStats playerStats;

    public int CurrentLevel => currentLevel;
    public PlayerStats PlayerStats => playerStats;
    public FieldConfig FieldConfig => fieldConfig;

    public PlayerProfileData(int currentLevel, PlayerStats playerStats, FieldConfig fieldConfig)
    {
        this.currentLevel = currentLevel;
        this.fieldConfig = fieldConfig;
        this.playerStats = playerStats;
    }
}

[System.Serializable]
public struct PlayerStats
{
    [SerializeField] int score;
    [SerializeField] int hearts;
    [SerializeField] int coins;
    [SerializeField] int wheels;

    public PlayerStats(int coins = 0, int score = 0, int hearts = 0,  int stars = 0)
    {
        this.score = score;
        this.hearts = hearts;
        this.coins = coins;
        this.wheels = stars;
    }

    public int Score => score;
    public int Hearts => hearts;
    public int Coins => coins;
    public int Wheels => wheels;
}
