using System.Runtime.InteropServices;
using UnityEngine;

namespace OgurySdk.ConsentManager
{
    public class OguryConsentManagerExternal
    {
#if UNITY_ANDROID
        private static AndroidJavaObject _consentManagerExternal = null;
        private static AndroidJavaObject ConsentManagerExternal =>
            _consentManagerExternal ?? (_consentManagerExternal =
                new AndroidJavaObject("com.ogury.unity.consentmanager.OguryConsentManagerExternal"));
#endif

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void ogury_consentManager_setConsent(string iabString, int nonIabVendorsAcceptedCount,
            string[] nonIabVendorsAccepted);
#endif

        public static void SetConsent(string iabString, string[] nonIabVendorsAccepted)
        {
#if UNITY_ANDROID
            ConsentManagerExternal.Call("setConsent", iabString, nonIabVendorsAccepted);
#elif UNITY_IOS
            var nonIabVendorsAcceptedCount = 0;
            if (nonIabVendorsAccepted != null)
            {
                nonIabVendorsAcceptedCount = nonIabVendorsAccepted.Length;
            }

            ogury_consentManager_setConsent(iabString, nonIabVendorsAcceptedCount, nonIabVendorsAccepted);
#endif
        }
    }
}