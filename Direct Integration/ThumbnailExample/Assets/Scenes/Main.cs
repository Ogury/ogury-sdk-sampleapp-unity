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

	public static string ANDROID_THUMBNAIL_AD_UNIT_ID = "2c137ab0-bab1-0138-db2a-0242ac120004_test";
    public static string IOS_THUMBNAIL_AD_UNIT_ID = "7fe46720-4a9f-0138-0f06-0242ac120004_test";

    public OguryThumbnailAd _thumbnailAd;
   

   

    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;

    
        Ogury.Start(ANDROID_ASSET_KEY, IOS_ASSET_KEY);
        
        
        
   

        _thumbnailAd = new OguryThumbnailAd(ANDROID_THUMBNAIL_AD_UNIT_ID,
            IOS_THUMBNAIL_AD_UNIT_ID);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        _thumbnailAd.OnAdLoaded += ad =>
        {
            // ...
            logger.LogAdLoadedMessage();
        };


        _thumbnailAd.OnAdNotAvailable += ad =>
        {
            // ...
            logger.LogAdNotAvailableMessage();
        };

        _thumbnailAd.OnAdDisplayed += ad =>
        {
            // ...
            logger.LogOnAdDisplayedMessage();
        };

        _thumbnailAd.OnAdClosed += ad =>
        {
            // ...
            logger.LogOnAdClosedMessage();
        };



        _thumbnailAd.OnAdError += OnAdError;

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
        _thumbnailAd.Load(180,180);
    }

    public void ShowThumbnailAd()
    {
        if (_thumbnailAd.Loaded)
        {
            _thumbnailAd.Show(OguryThumbnailAdGravity.BottomRight,0, 0);
        }

    }


    void OnAdError(OguryThumbnailAd oguryThumbnailAd, OguryError error)
    {
        logger.LogOnAdErrordMessage(String.Format("Ad Error {0} - {1}", error.ErrorCode, error.Description));
    }


    private void OnDestroy()
    {
        OguryChoiceManager.OnAskComplete -= OnCMComplete;
        OguryChoiceManager.OnAskError -= OnCMError;
    }

}
