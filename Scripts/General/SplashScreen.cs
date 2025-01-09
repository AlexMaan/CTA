using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] ReactableAnimator reactAnimator;

    void OnEnable() => Loader.OnSceneLoaded += React;
    void OnDisable() => Loader.OnSceneLoaded -= React;

    void React(string scene)
    {
        if(scene != Loader.Instance.LoaderConfig.MenuScene) return;
        reactAnimator.Act();
    }
}
