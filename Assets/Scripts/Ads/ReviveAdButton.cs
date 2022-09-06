using System;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReviveAdButton : MonoBehaviour
{
    IRewardedAd ad;
    string adUnitId = "Rewarded_Android";
    string gameId = "4877029";
    private Button _showAdButton;
    [SerializeField] GameObject player;
    private GameManager gameManager;
    private FBAnalyticss FBAnalyticss;
    private void Awake()
    {
        FBAnalyticss = FindObjectOfType<FBAnalyticss>();
    }
    private void Start()
    {
        InitServices();
        _showAdButton = GetComponent<Button>();
        _showAdButton.onClick.AddListener(() => ShowAd());
        gameManager = FindObjectOfType<GameManager>();
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
        // Debug.Log("Initialization Failed: " + e.Message);
    }

    void AdLoaded(object sender, EventArgs e)
    {
        // Debug.Log("Ad loaded");
    }

    void AdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        //Debug.Log("Failed to load ad");
        // Debug.Log(e.Message);
    }

    void AdShown()
    {
        // Debug.Log("Ad shown!");
    }

    void AdClosed(object sender, EventArgs e)
    {
        // Debug.Log("Ad has closed");
        // Execute logic after an ad has been closed.
    }

    void AdClicked(object sender, EventArgs e)
    {
        // Debug.Log("Ad has been clicked");
        // Execute logic after an ad has been clicked.
    }

    void AdFailedShow(ShowFailedException e)
    {
        // Debug.Log(e.Message);
    }

    void ImpressionEvent(object sender, ImpressionEventArgs args)
    {
        var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
        // Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
    }

    void UserRewarded(object sender, RewardEventArgs e)
    {
        // Debug.Log($"Received reward: type:{e.Type}; amount:{e.Amount}");
        FBAnalyticss.LogRevivedLevelEvent("Revived Level: " + (DataManager.Instance.levelNumber + 1).ToString());
        OldAdManager.Instance.playerPos = new Vector3(0, 0.75f, player.transform.position.z);
        OldAdManager.Instance.isRevived = true;
        OldAdManager.Instance.reviveCoins = gameManager._coins;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}