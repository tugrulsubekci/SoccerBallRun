using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;
using System.Collections.Generic;

public class FBAnalyticss : MonoBehaviour
{
    private int frameRate = 30;
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
    public void LogAchievedLevelEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(
            AppEventName.AchievedLevel,
            null,
            parameters
        );
    }
    public void LogSkippedLevelEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(
            "Skipped Level",
            null,
            parameters
        );
    }
    public void LogRevivedLevelEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(
            "Revived Level",
            null,
            parameters
        );
    }
}
