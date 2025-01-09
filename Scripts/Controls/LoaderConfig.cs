using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoaderConfig", menuName = "Scriptables/LoaderConfig")]
public class LoaderConfig : ScriptableObject
{
    [Header("Init Config")]
    [SerializeField] string playerProfileName;
    [SerializeField] PlayerProfile defaultPlayerProfile;
    [SerializeField] string menuScene;
    [SerializeField] string gameScene;
    [SerializeField] int displaySplashTime;

    [Header("Menu Config")]
    [SerializeField] int maxLevelCount;

    public string PlayerProfileName => playerProfileName;
    public PlayerProfile DefaultPlayerProfile => defaultPlayerProfile;
    public string MenuScene => menuScene;
    public string GameScene => gameScene;
    public int DisplaySplashTime => displaySplashTime;
    public int MaxLevelCount => maxLevelCount;


}
