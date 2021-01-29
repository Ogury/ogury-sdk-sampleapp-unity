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

    public static string AndroidThumbnailAdUnitId = "2c137ab0-bab1-0138-db2a-0242ac120004_test";
    public static string IosThumbnailAdUnitId = "7fe46720-4a9f-0138-0f06-0242ac120004_test";

    private OguryThumbnailAd thumbnailAd;




    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;

        Ogury.Start(AndroidAssetKey, IosAssetKey);

        thumbnailAd = new OguryThumbnailAd(AndroidThumbnailAdUnitId,
            IosThumbnailAdUnitId);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        thumbnailAd.OnAdLoaded += ad =>
        {
            logger.LogAdLoadedMessage();
        };

        thumbnailAd.OnAdNotAvailable += ad =>
        {
            logger.LogAdNotAvailableMessage();
        };

        thumbnailAd.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage();
        };

        thumbnailAd.OnAdClosed += ad =>
        {
            logger.LogOnAdClosedMessage();
        };

        thumbnailAd.OnAdNotLoaded += ad =>
        {
            logger.LogAdNotLoadedMessage();
        };

        thumbnailAd.OnAdError += OnAdError;

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

        thumbnailAd.Load(180, 180);
    }

    public void ShowThumbnailAd()
    {
        if (thumbnailAd.Loaded)
        {
            thumbnailAd.Show(OguryThumbnailAdGravity.BottomRight, 0, 0);
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
