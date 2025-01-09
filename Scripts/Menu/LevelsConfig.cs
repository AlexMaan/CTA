using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] int levelNumber;
    [SerializeField] int goal;
    [SerializeField] int limit;
    [SerializeField] bool isTutorial;
    [SerializeField] PlayerStats prizes;

    public int LevelNumber => levelNumber;
    public int Goal => goal;
    public int Limitation => limit;
    public bool IsTutorial => isTutorial;
    public PlayerStats Prizes => prizes;
}

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Scriptables/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    [SerializeField] List<LevelData> levels;

    public List<LevelData> Levels => levels;
}
