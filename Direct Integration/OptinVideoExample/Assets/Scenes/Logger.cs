using UnityEngine;
using UnityEngine.UI;
using System;

public class Logger : MonoBehaviour
{

    Text labelText;



    // Use this for initialization
    void Start()
    {
        labelText = GetComponent<Text>();
    }

    public void LogConsent(string consentInfo)
    {
        DisplayText(consentInfo);
    }

    public void LogLoadingAdMessage()
    {
        DisplayText("Loading Ad");
    }

    public void LogAdLoadedMessage()
    {
        DisplayText("Ad Loaded");
    }

    public void LogUserReward(string reward)
    {
        DisplayText(reward);
    }


    public void LogAdNotAvailableMessage()
    {
        DisplayText("Ad not Available");
    }

    public void LogOnAdDisplayedMessage()
    {
        DisplayText("Ad Displayed");
    }

    public void LogOnAdClosedMessage()
    {
        DisplayText("Ad Closed");
    }

    public void LogAdNotLoadedMessage()
    {
        DisplayText("Ad Not Loaded");
    }

    public void LogOnAdErrordMessage(string error)
    {
        DisplayText(error);
    }

    public void LogNoMessage()
    {
        labelText.enabled = false;
    }


    public void DisplayText(string text)
    {
        labelText.text = CustomFormat(text);
    }

    private string CustomFormat(string message)
    {
        return String.Format(String.Format("Status: {0}", message));
    }
}
