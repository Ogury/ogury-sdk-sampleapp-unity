using UnityEngine;
using OgurySdk;
using static OgurySdk.OguryChoiceManager;
using System;

public class Main : MonoBehaviour
{
    //used just for displaying messages
    Logger logger;
    
	public static string ANDROID_ASSET_KEY = "OGY-E16CBF165165";
	public static string IOS_ASSET_KEY = "OGY-5575CC173955";

	public static string ANDROID_INTERSTITIAL_AD_UNIT_ID = "18efd430-bab1-0138-ceb4-0242ac120004_test";
    public static string IOS_INTERSTITIAL_AD_UNIT_ID = "cdab8440-4a9d-0138-0f05-0242ac120004_test";

    public OguryInterstitialAd _interstitial;
    public OguryThumbnailAd oguryThumbnailAd;

   

    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(ANDROID_ASSET_KEY, IOS_ASSET_KEY);

        _interstitial = new OguryInterstitialAd(ANDROID_INTERSTITIAL_AD_UNIT_ID,
            IOS_INTERSTITIAL_AD_UNIT_ID);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        _interstitial.OnAdLoaded += ad =>
        {
            // ...
            logger.LogAdLoadedMessage();
        };


        _interstitial.OnAdNotAvailable += ad =>
        {
            // ...
            logger.LogAdNotAvailableMessage();
        };

        _interstitial.OnAdDisplayed += ad =>
        {
            // ...
            logger.LogOnAdDisplayedMessage();
        };

        _interstitial.OnAdClosed += ad =>
        {
            // ...
            logger.LogOnAdClosedMessage();
        };



        _interstitial.OnAdError += OnAdError;

        logger.LogOnAdDisplayedMessage();
    }


    private void OnCMComplete(Answer answer)
    {
        logger.LogConsent(String.Format("Consent {0}", answer.ToString()));
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats 
        //LoadAdFormats();
    }

    private void OnCMError(OguryError error)
    {
        logger.LogConsent(String.Format("Consent {0} - {1}", error.ErrorCode, error.Description));
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats
        //LoadAdFormats();  
    }


    private void StartSdks()
    {
        Ogury.StartAds();
        // start vendors' SDKs
    }

    private void PassConsentToOtherSdks()
    {
        // pass user consent to vendors' SDKs
    }

    public void LoadAdFormats()
    {
        logger.LogLoadingAdMessage();
        // load the intertitial ad
        _interstitial.Load();
    }

    public void ShowInterstitial()
    {
        if (_interstitial.Loaded)
        {
            _interstitial.Show();
        }
    }


    
    void OnAdError(OguryInterstitialAd interstitialAd, OguryError error)
    {
        logger.LogOnAdErrordMessage(String.Format("Ad Error {0} - {1}", error.ErrorCode, error.Description));
    }


    private void OnDestroy()
    {
        OguryChoiceManager.OnAskComplete -= OnCMComplete;
        OguryChoiceManager.OnAskError -= OnCMError;
    }

}
