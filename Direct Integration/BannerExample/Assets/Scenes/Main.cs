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

    public static string AndroidSmallBannerAdUnitId = "3b6dd5e0-bab1-0138-76e8-0242ac120004_test";
    public static string IosSmallBannerAdUnitId = "c3a1a5e0-4f39-0138-42f4-0242ac120004_test";

    public static string AndroidMpuBannerAdUnitId = "3530b2c0-bab1-0138-ceb6-0242ac120004_test";
    public static string IosMpuBannerAdUnitId = "73eb6620-b234-0138-8e13-0242ac120004_test";

    private OguryBannerAd smallBanner;
    private OguryBannerAd mpuBanner;



    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(AndroidAssetKey, IosAssetKey);



        smallBanner = new OguryBannerAd(AndroidSmallBannerAdUnitId,
            IosSmallBannerAdUnitId, OguryBannerAdSize.SmallBanner320X50);

        mpuBanner = new OguryBannerAd(AndroidMpuBannerAdUnitId,
            IosMpuBannerAdUnitId, OguryBannerAdSize.Mpu300X250);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        smallBanner.OnAdLoaded += ad =>
        {
            logger.LogAdLoadedMessage("Small Banner");
        };

        smallBanner.OnAdNotLoaded += ad =>
        {
            logger.LogAdNotLoadedMessage("Small Banner");
        };

        mpuBanner.OnAdLoaded += ad =>
        {
            logger.LogAdLoadedMessage("MPU");
        };

        mpuBanner.OnAdNotLoaded += ad =>
        {
            logger.LogAdNotLoadedMessage("MPU");
        };


        smallBanner.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage("Small Banner");
        };

        mpuBanner.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage("MPU");
        };


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


    public void LoadMPUBannerAd()
    {
        logger.LogLoadingAdMessage("MPU");
        mpuBanner.Load();
    }


    public void LoadSmallBannerAd()
    {
        logger.LogLoadingAdMessage("Small Banner");
        smallBanner.Load();
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
        // load the intertitial ad
        smallBanner.Load();
        mpuBanner.Load();
    }

    public void ShowSmallBannerAd()
    {
        smallBanner.Show(OguryAdGravity.Bottom, 0, 0);
    }

    public void ShowMPUBannerAd()
    {
        mpuBanner.Show(OguryAdGravity.Bottom, 0, 250);
    }


    private void OnDestroy()
    {
        OguryChoiceManager.OnAskComplete -= OnCMComplete;
        OguryChoiceManager.OnAskError -= OnCMError;
    }

}
