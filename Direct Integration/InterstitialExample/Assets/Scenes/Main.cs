using UnityEngine;
using OgurySdk;
using static OgurySdk.OguryChoiceManager;
using System;

public class Main : MonoBehaviour
{
    //used just for displaying messages
    Logger logger;

    public static string AndroidAssetKey = "OGY-E16CBF165165";
    public static string IosAssetKey = "OGY-5575CC173955";

    public static string AndroidInterstitialAdUnitId = "18efd430-bab1-0138-ceb4-0242ac120004_test";
    public static string IosInterstitialAdUnitId = "cdab8440-4a9d-0138-0f05-0242ac120004_test";

    private OguryInterstitialAd interstitialAd;



    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(AndroidAssetKey, IosAssetKey);

        interstitialAd = new OguryInterstitialAd(AndroidInterstitialAdUnitId,
            IosInterstitialAdUnitId);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        interstitialAd.OnAdLoaded += ad =>
        {
            logger.LogAdLoadedMessage();
        };

        interstitialAd.OnAdNotLoaded += ad =>
        {
            logger.LogAdNotLoadedMessage();
        };


        interstitialAd.OnAdNotAvailable += ad =>
        {
            logger.LogAdNotAvailableMessage();
        };

        interstitialAd.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage();
        };

        interstitialAd.OnAdClosed += ad =>
        {
            logger.LogOnAdClosedMessage();
        };



        interstitialAd.OnAdError += OnAdError;

    }


    private void OnCMComplete(Answer answer)
    {
        logger.LogConsent(String.Format("Consent {0}", answer.ToString()));
        PassConsentToOtherSdks();
        StartSdks();
        //Load Ad formats
    }

    private void OnCMError(OguryError error)
    {
        logger.LogConsent(String.Format("Consent {0} - {1}", error.ErrorCode, error.Description));
        PassConsentToOtherSdks();
        StartSdks();
        //Load Ad formats
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

        interstitialAd.Load();
    }

    public void ShowInterstitial()
    {
        if (interstitialAd.Loaded)
        {
            interstitialAd.Show();
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
