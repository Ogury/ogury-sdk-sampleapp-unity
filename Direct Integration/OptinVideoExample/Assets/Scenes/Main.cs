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

	public static string ANDROID_OPTINVIDEO_AD_UNIT_ID = "20a99c20-bab1-0138-ceb5-0242ac120004_test";
    public static string IOS_OPTINVIDEO_AD_UNIT_ID = "b1773ac0-4a9d-0138-d91c-0242ac120004_test";

    public OguryOptinVideoAd _optinVideo;

   

    // Use this for initialization
    void Start()
    {
        logger = FindObjectOfType(typeof(Logger)) as Logger;


        Ogury.Start(ANDROID_ASSET_KEY, IOS_ASSET_KEY);

        _optinVideo = new OguryOptinVideoAd(ANDROID_OPTINVIDEO_AD_UNIT_ID,
            IOS_OPTINVIDEO_AD_UNIT_ID);

        // get user consent
        OguryChoiceManager.OnAskComplete += OnCMComplete;
        OguryChoiceManager.OnAskError += OnCMError;
        OguryChoiceManager.Ask();


        _optinVideo.OnAdLoaded += ad =>
        {
            // ...
            logger.LogAdLoadedMessage();
        };

        _optinVideo.OnAdRewarded += (ad, rewardItem) =>
        {
            // reward the user here
            logger.LogUserReward(String.Format("User has received reward {0} with value: {1}",
                rewardItem.Name,rewardItem.Value));
        };


        _optinVideo.OnAdNotAvailable += ad =>
        {
            // ...
            logger.LogAdNotAvailableMessage();
        };

        _optinVideo.OnAdDisplayed += ad =>
        {
            // ...
            logger.LogOnAdDisplayedMessage();
        };

        _optinVideo.OnAdClosed += ad =>
        {
            // ...
            logger.LogOnAdClosedMessage();
        };

        _optinVideo.OnAdError += OnAdError;


        logger.LogLoadingAdMessage();
    }


    private void OnCMComplete(Answer answer)
    {
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats
        //LoadAdFormats();
    }

    private void OnCMError(OguryError error)
    {
        PassConsentToOtherSdks();
        StartSdks();
        // load ad formats
        LoadAdFormats();  
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
        _optinVideo.Load();
    }

    public void ShowOptinVideo()
    {
        if (_optinVideo.Loaded)
        {
            _optinVideo.Show();
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
