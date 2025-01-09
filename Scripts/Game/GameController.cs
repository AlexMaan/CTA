using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Field field;
    [SerializeField] Race race;
    [SerializeField] GameObject winPopup, losePopup;
    [SerializeField] int[] products;
    [SerializeField] LevelData levelData;

    bool isTutorial;

    public LevelData LevelData => levelData;
    public static Action<bool> OnLevelEnded;

    void Start()
    {
        products = new int[Loader.Instance.PlayerConfigManager.PlayerProfileData.FieldConfig.ColorsCount];
        if (MenuController.CurrentLevelData != null) levelData = MenuController.CurrentLevelData;

        if (!field) field = FindObjectOfType<Field>();
        if (!race) race = FindObjectOfType<Race>();
        field.Init(this);
        race.Init(this);
        isTutorial = levelData.IsTutorial;
    }

    public async void ReturnToMenu()
    {
        
        await Loader.Instance.UnloadSceneAsync(Loader.Instance.LoaderConfig.GameScene);
    }

    public void ConvertToProduct(int count, int colorId)
    {
        products[colorId] += count;
        int sum = products.Aggregate(products[0], (all, x) => all + x);
        BucketsController.Instance.FillBucket(colorId, count);
        if (isTutorial && CheckCompletion()) EndCondition(true);
    }


    public void EndCondition(bool win)
    {
        if (win)
        {
            winPopup.SetActive(true);
            PlayerProfileManager.Instance.UpdateLevel(levelData.LevelNumber + 1);
            PlayerProfileManager.Instance.UpdatePlayerStats(levelData.Prizes);
        }
        else losePopup.SetActive(true);
    }

    public void EndLevel(bool win)
    {
        OnLevelEnded?.Invoke(win);
        ReturnToMenu();
    }

    bool CheckCompletion()
    {
        bool result = true;
        foreach (var product in products)
        {
            if (product < 5)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public void ManualExit()
    {
        PlayerProfileManager.Instance.UpdatePlayerStats(new PlayerStats(0, 0, -1));
        EndLevel(false);
    }
}
