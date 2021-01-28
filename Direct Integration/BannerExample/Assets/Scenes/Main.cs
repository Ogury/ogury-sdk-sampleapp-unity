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

	public static string ANDROID_SMALL_BANNER_AD_UNIT_ID = "3b6dd5e0-bab1-0138-76e8-0242ac120004_test";
    public static string IOS_SMALL_BANNER_AD_UNIT_ID = "c3a1a5e0-4f39-0138-42f4-0242ac120004_test";

    public static string ANDROID_MPU_BANNER_AD_UNIT_ID = "3530b2c0-bab1-0138-ceb6-0242ac120004_test";
    public static string IOS_MPU_BANNER_UNIT_ID = "73eb6620-b234-0138-8e13-0242ac120004_test";

    public OguryBannerAd _small_banner;
    public OguryBannerAd _mpu_banner;



    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(ANDROID_ASSET_KEY, IOS_ASSET_KEY);

      

        _small_banner = new OguryBannerAd(ANDROID_SMALL_BANNER_AD_UNIT_ID,
            IOS_SMALL_BANNER_AD_UNIT_ID,OguryBannerAdSize.SmallBanner320X50);

        _mpu_banner = new OguryBannerAd(ANDROID_MPU_BANNER_AD_UNIT_ID,
            IOS_MPU_BANNER_UNIT_ID, OguryBannerAdSize.Mpu300X250);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        _small_banner.OnAdLoaded += ad =>
        {
            // ...
            logger.LogAdLoadedMessage("Small Banner");
        };

        _mpu_banner.OnAdLoaded += ad =>
        {
            // ...
            logger.LogAdLoadedMessage("MPU");
        };



        _small_banner.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage("Small Banner");
        };

        _mpu_banner.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage("MPU");
        };

        
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


    public void LoadMPUBannerAd()
    {
        logger.LogLoadingAdMessage("MPU");
        _mpu_banner.Load();
    }


    public void LoadSmallBannerAd()
    {
        logger.LogLoadingAdMessage("Small Banner");
        _small_banner.Load();
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
        _small_banner.Load();
        _mpu_banner.Load();
    }

    public void ShowSmallBannerAd()
    {
        _small_banner.Show(OguryAdGravity.Bottom, 0, 0);
    }

    public void ShowMPUBannerAd()
    {
        _mpu_banner.Show(OguryAdGravity.Bottom, 0, 250);
    }


    private void OnDestroy()
    {
        OguryChoiceManager.OnAskComplete -= OnCMComplete;
        OguryChoiceManager.OnAskError -= OnCMError;
    }

}
