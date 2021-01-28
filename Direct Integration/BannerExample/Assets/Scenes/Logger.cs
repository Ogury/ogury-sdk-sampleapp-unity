using UnityEngine;
using System.Collections;
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

    public void LogLoadingAdMessage(string adType)
    {
        DisplayText(String.Format("Loading {0}",adType));
    }

    public void LogAdLoadedMessage(string adType)
    {
        DisplayText(String.Format("{0} Ad Loaded",adType));
    }


    public void LogAdNotAvailableMessage()
    {
        DisplayText("Ad not Available");
    }

    public void LogOnAdDisplayedMessage(string adType)
    {
        DisplayText(String.Format("{0} Ad Displayed",adType));
    }

    public void LogOnAdClosedMessage()
    {
        DisplayText("Ad Closed");
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
        var str = labelText.text;
        labelText.text = str + CustomFormat(text);
    }

    private string CustomFormat(string message)
    {
        return String.Format(String.Format("{0} {1}", message, "\n"));
    }
}
