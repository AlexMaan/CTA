using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] string sceneName;
    void OnEnable() => Loader.OnSceneLoaded += HideOnLoad;
    void OnDisable() => Loader.OnSceneLoaded -= HideOnLoad;

    void HideOnLoad(string scene)
    {
        if (scene == sceneName) HidePopup();
    }

    public void HidePopup()
    {
        gameObject.SetActive(false);
    }
}
