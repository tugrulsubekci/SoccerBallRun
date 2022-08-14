using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;

public class FrameRate : MonoBehaviour
{
    private int frameRate = 60;
    private void Awake()
    {
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
