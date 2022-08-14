using System;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipLevelAdButton : MonoBehaviour
{
    IRewardedAd ad;
    string adUnitId = "Rewarded_Android";
    string gameId = "4877029";
    private Button showAdButton;
    private void Start()
    {
        InitServices();
        showAdButton = GetComponent<Button>();
        showAdButton.onClick.AddListener(() => ShowAd());
    }
    public async void InitServices()
    {
        try
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetGameId(gameId);
            await UnityServices.InitializeAsync(initializationOptions);

            InitializationComplete();
        }
        catch (Exception e)
        {
            InitializationFailed(e);
        }
    }

    public void SetupAd()
    {
        //Create
        ad = MediationService.Instance.CreateRewardedAd(adUnitId);

        //Subscribe to events
        ad.OnClosed += AdClosed;
        ad.OnClicked += AdClicked;
        ad.OnLoaded += AdLoaded;
        ad.OnFailedLoad += AdFailedLoad;
        ad.OnUserRewarded += UserRewarded;

        // Impression Event
        MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
    }

    public async void ShowAd()
    {
        if (ad.AdState == AdState.Loaded)
        {
            try
            {
                RewardedAdShowOptions showOptions = new RewardedAdShowOptions();
                showOptions.AutoReload = true;
                await ad.ShowAsync(showOptions);
                AdShown();
            }
            catch (ShowFailedException e)
            {
                AdFailedShow(e);
            }
        }
    }

    void InitializationComplete()
    {
        SetupAd();
        LoadAd();
    }

    async void LoadAd()
    {
        try
        {
            await ad.LoadAsync();
        }
        catch (LoadFailedException)
        {
            // We will handle the failure in the OnFailedLoad callback
        }
    }

    void InitializationFailed(Exception e)
    {
        Debug.Log("Initialization Failed: " + e.Message);
    }

    void AdLoaded(object sender, EventArgs e)
    {
        Debug.Log("Ad loaded");
    }

    void AdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.Log("Failed to load ad");
        Debug.Log(e.Message);
    }

    void AdShown()
    {
        Debug.Log("Ad shown!");
    }

    void AdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.
    }

    void AdClicked(object sender, EventArgs e)
    {
        Debug.Log("Ad has been clicked");
        // Execute logic after an ad has been clicked.
    }

    void AdFailedShow(ShowFailedException e)
    {
        Debug.Log(e.Message);
    }

    void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }

    void UserRewarded(object sender, RewardEventArgs e)
    {
        Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");
        DataManager.Instance.levelIndex++;
        DataManager.Instance.levelNumber++;
        DataManager.Instance.Save();
        if (DataManager.Instance.levelIndex < 10)
        {
            SceneManager.LoadScene(DataManager.Instance.levelIndex);
        }
        else if (DataManager.Instance.levelIndex == 10)
        {
            DataManager.Instance.levelIndex = 1;
            DataManager.Instance.Save();
            SceneManager.LoadScene(DataManager.Instance.levelIndex);
        }
    }

}