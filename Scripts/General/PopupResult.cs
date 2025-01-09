using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupResult : Popup
{
    [SerializeField] bool isWin;
    [SerializeField] GameObject[] prizes;
    [SerializeField] TextMeshProUGUI[] counts;

    LevelData levelData;

    private void Start()
    {
        if (MenuController.CurrentLevelData != null) levelData = MenuController.CurrentLevelData;
        if (levelData != null) Init();
    }

    void Init()
    {
        for (int i = 0; i < 3; i++)
            prizes[i].SetActive(false);

        if (levelData.Prizes.Coins > 0)
        {
            prizes[0].SetActive(true);
            counts[0].text = ((int)(levelData.Prizes.Coins * Mock())).ToString();
        }
        if (levelData.Prizes.Wheels > 0)
        {
            prizes[1].SetActive(true);
            counts[1].text = ((int)(levelData.Prizes.Wheels )).ToString();
        }
        if (levelData.Prizes.Score > 0)
        {
            prizes[2].SetActive(true);
            counts[2].text = ((int)(levelData.Prizes.Score * Mock())).ToString();
        }
    }

    float Mock()
    {
        float win = isWin ? 1 : 0.2f;
        return Random.Range(0.5f, 1.5f) * win;
    }
}
