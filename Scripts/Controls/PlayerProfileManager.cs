using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance { get; private set; }
    string configPath;
    [SerializeField] PlayerProfileData playerProfileData;
    [SerializeField] bool testMode;

    public PlayerProfileData PlayerProfileData => playerProfileData;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        configPath = Application.persistentDataPath + $"/{Loader.Instance.LoaderConfig.PlayerProfileName}.profile";
        playerProfileData = LoadConfig();
    }

    PlayerProfileData LoadConfig()
    {
        if (File.Exists(configPath) && !testMode)
        {
            var bf = new BinaryFormatter();
            using (var filestream = new FileStream(configPath, FileMode.Open))
            {
                return (PlayerProfileData)bf.Deserialize(filestream);
            }
        }
        else
        {
            return Loader.Instance.LoaderConfig.DefaultPlayerProfile.data;
        }
    }

    public void SaveConfig(PlayerProfileData playerConfig)
    {
        var bf = new BinaryFormatter();
        using (var filestream = new FileStream(configPath, FileMode.Create))
        {
            bf.Serialize(filestream, playerConfig);
        }
    }

    public void UpdateLevel(int level)
    {

        playerProfileData = new PlayerProfileData(level, playerProfileData.PlayerStats,
                                                  playerProfileData.FieldConfig);
        SaveConfig(playerProfileData);
    }

    public void UpdatePlayerStats(PlayerStats playerStats)
    {

        playerProfileData = new PlayerProfileData(playerProfileData.CurrentLevel,
                                                  AddStats(playerStats), playerProfileData.FieldConfig);
        SaveConfig(playerProfileData);
    }

    PlayerStats AddStats(PlayerStats stats)
    {

        var baseStats = playerProfileData.PlayerStats;
        return new PlayerStats(Mathf.Max(0, stats.Coins + baseStats.Coins),
                               Mathf.Max(0, stats.Score + baseStats.Score),
                               Mathf.Max(0, stats.Hearts + baseStats.Hearts),
                               Mathf.Max(0, stats.Wheels + baseStats.Wheels));
    }

    public void ResetConfig()
    {
        File.Delete(configPath);
    }
}
