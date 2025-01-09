using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Loader : MonoBehaviour
{
    [SerializeField] LoaderConfig loaderConfig;
    [SerializeField] PlayerProfileManager playerProfileManager;
    [SerializeField] bool testMode;

    Loader() { }
    static Loader instance;
    public static Loader Instance
    {
        get
        {
            if (instance == null) instance = FindAnyObjectByType<Loader>();
            if (instance == null) instance = new GameObject("Loader").AddComponent<Loader>();
            return instance;
        }
    }

    public PlayerProfileManager PlayerConfigManager => playerProfileManager;
    public LoaderConfig LoaderConfig => loaderConfig;

    public static Action<string> OnSceneLoaded;
    public static Action<string> OnSceneUnloaded;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        if (playerProfileManager == null)
        {
            playerProfileManager = new GameObject("PlayerConfigManager")
                                      .AddComponent<PlayerProfileManager>();
            playerProfileManager.transform.SetParent(transform);    
        }

        if (testMode) return;
        await Task.Delay(TimeSpan.FromSeconds(loaderConfig.DisplaySplashTime));
        await LoadSceneAsync(loaderConfig.MenuScene);
    }

    public async Task LoadSceneAsync(string sceneName, bool additive = false)
    {
        var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncLoad.isDone) await Task.Yield();
        OnSceneLoaded?.Invoke(sceneName);
    }

    public async Task UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone) await Task.Yield();
        OnSceneUnloaded?.Invoke(sceneName);
    }
}
