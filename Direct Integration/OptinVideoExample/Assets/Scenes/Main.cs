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

    public static string AndroidOptinVideoAdUnitId = "20a99c20-bab1-0138-ceb5-0242ac120004_test";
    public static string IosOptinVideoAdUnitId = "b1773ac0-4a9d-0138-d91c-0242ac120004_test";

    private OguryOptinVideoAd optinVideoAd;



    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(AndroidAssetKey, IosAssetKey);

        optinVideoAd = new OguryOptinVideoAd(AndroidOptinVideoAdUnitId,
            IosOptinVideoAdUnitId);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        optinVideoAd.OnAdLoaded += ad =>
        {
            logger.LogAdLoadedMessage();
        };

        optinVideoAd.OnAdNotLoaded += ad =>
        {
            logger.LogAdNotLoadedMessage();
        };

        optinVideoAd.OnAdRewarded += (ad, rewardItem) =>
        {
            // reward the user here
            logger.LogUserReward(String.Format("User has received reward {0} with value: {1}",
                rewardItem.Name, rewardItem.Value));
        };


        optinVideoAd.OnAdNotAvailable += ad =>
        {
            logger.LogAdNotAvailableMessage();
        };

        optinVideoAd.OnAdDisplayed += ad =>
        {
            logger.LogOnAdDisplayedMessage();
        };

        optinVideoAd.OnAdClosed += ad =>
        {
            logger.LogOnAdClosedMessage();
        };

        optinVideoAd.OnAdError += OnAdError;

    }


    private void OnCMComplete(Answer answer)
    {
        logger.LogConsent(String.Format("Consent {0}", answer.ToString()));
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats
    }

    private void OnCMError(OguryError error)
    {
        logger.LogConsent(String.Format("Consent {0} - {1}", error.ErrorCode, error.Description));
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats
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

        optinVideoAd.Load();
    }

    public void ShowOptinVideo()
    {
        if (optinVideoAd.Loaded)
        {
            optinVideoAd.Show();
        }
    }


    void OnAdError(OguryOptinVideoAd oguryOptinVideoAd, OguryError error)
    {
        logger.LogOnAdErrordMessage(String.Format("Ad Error {0} - {1}", error.ErrorCode, error.Description));
    }


    private void OnDestroy()
    {
        OguryChoiceManager.OnAskComplete -= OnCMComplete;
        OguryChoiceManager.OnAskError -= OnCMError;
    }

}
