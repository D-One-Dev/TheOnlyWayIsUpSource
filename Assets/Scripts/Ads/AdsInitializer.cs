using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    //TEST BUILD
    private readonly string _androidGameId = "0";

    [SerializeField] bool _testMode = true;
    [SerializeField] RewardedAd rewardedAd;
    [SerializeField] InterstitialAd interstitialAd;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidGameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        if(rewardedAd != null) rewardedAd.LoadAd();
        if(interstitialAd != null) interstitialAd.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}