using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Camera menuCamera;
    [SerializeField] RoadMap roadMap;
    [SerializeField] LevelsConfig levelsConfig;

    string gamescene;

    public static LevelData CurrentLevelData { get; private set; }

    void Awake() => CurrentLevelData = null;

    void OnEnable()
    {
        GameController.OnLevelEnded += ProcessLevelResults;
        Loader.OnSceneUnloaded += OnSceneUnloaded;
    }
    void OnDisable()
    {
        GameController.OnLevelEnded -= ProcessLevelResults;
        Loader.OnSceneUnloaded -= OnSceneUnloaded;
    }

    void Start()
    {
        gamescene = Loader.Instance.LoaderConfig.GameScene;
        InitData();
    }

    public void InitData()
    {
        int levelN = Loader.Instance.PlayerConfigManager.PlayerProfileData.CurrentLevel;
        LevelData data = levelsConfig.Levels.FirstOrDefault(x => x.LevelNumber == levelN);
        if (data == null) data = levelsConfig.Levels.Last();
        CurrentLevelData = data;
    }

    public async void LoadGame()
    {
        await Loader.Instance.LoadSceneAsync(gamescene, true);
        menuCamera.gameObject.SetActive(false);
    }

    void OnSceneUnloaded(string scene)
    {
        if (scene == gamescene) menuCamera.gameObject.SetActive(true);
    }

    void ProcessLevelResults(bool iswin)
    {
        InitData();
        if (iswin)
            roadMap.MoveToNextPoint();
    }
}
