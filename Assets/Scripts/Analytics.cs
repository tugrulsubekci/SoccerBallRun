using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;
using System.Collections.Generic;

public class Analytics : MonoBehaviour
{
    public static Analytics Instance;
    private int frameRate = 60;
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = frameRate;
        GameAnalytics.Initialize();
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
            {
                FB.ActivateApp();
            });
        }
    }
}
