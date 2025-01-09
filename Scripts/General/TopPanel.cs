using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    [SerializeField] GameObject exitPopup;
    [SerializeField] TextMeshProUGUI[] counts;

    PlayerProfileData playerData;

    void OnEnable()
    {
        GameController.OnLevelEnded += Init;
        Loader.OnSceneLoaded += Init;
        Loader.OnSceneUnloaded += Init;
    }
    void OnDisable()
    {
        GameController.OnLevelEnded -= Init;
        Loader.OnSceneLoaded -= Init;
        Loader.OnSceneUnloaded += Init;
    }

    void Start() => Init(false);

    void Init()
    {
        playerData = PlayerProfileManager.Instance.PlayerProfileData;
        counts[0].text = playerData.PlayerStats.Coins.ToString();
        counts[1].text = playerData.PlayerStats.Wheels.ToString();
        counts[2].text = playerData.PlayerStats.Score.ToString();
        counts[3].text = playerData.PlayerStats.Hearts.ToString();
    }

    void Init(string name) => Init();
    void Init(bool isWin) => Init();

    public void ShowExitPopup() => exitPopup.SetActive(true);
}
