using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] MenuController _menuController;
    int adType;

    void Awake()
    {
        if (Advertisement.isInitialized) LoadAd();
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _androidAdUnitId);
        Advertisement.Load(_androidAdUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd(int type)
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        adType = type;

        if(PlayerPrefs.GetInt("AdCounter", 0) == 4)
        {
            Debug.Log("Showing Ad: " + _androidAdUnitId);
            PlayerPrefs.SetInt("AdCounter", 0);
            Advertisement.Show(_androidAdUnitId, this);
        }
        else
        {
            PlayerPrefs.SetInt("AdCounter", PlayerPrefs.GetInt("AdCounter", 0) + 1);
            ChangeScene();
        }
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAd();
        ChangeScene();
    }

    private void ChangeScene()
    {
        switch (adType)
        {
            case 0:
                _menuController.StartGame();
                break;
            case 1:
                _menuController.ExitToMenu();
                break;
        }
    }
}