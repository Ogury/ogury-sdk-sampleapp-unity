using System;
using System.Runtime.InteropServices;
using UnityEngine;
// ReSharper disable UseNullPropagation
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable 618

namespace OgurySdk.ConsentManager
{
    public class OguryConsentManager
    {
#if UNITY_ANDROID
        private static AndroidJavaObject _consentManager = null;
        private static AndroidJavaObject ConsentManager =>
            _consentManager ?? (_consentManager =
                new AndroidJavaObject("com.ogury.unity.consentmanager.OguryConsentManager"));
#endif

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void ogury_consentManager_ask();

        [DllImport("__Internal")]
        private static extern void ogury_consentManager_edit();

        [DllImport("__Internal")]
        private static extern string ogury_consentManager_iabString();

        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_gdprApplies();

        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_isAccepted(string vendorSlug);

        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_isPurposeAccepted(string purpose);

        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_isSpecialFeatureAccepted(int specialFeatures);

        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_editAvailable();
        
        [DllImport("__Internal")]
        private static extern bool ogury_consentManager_hasPaid();
#endif

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static event ConsentManagerCompletedEventHandler OnAskComplete;
        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static event ConsentManagerFailedEventHandler OnAskError;
        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static event ConsentManagerCompletedEventHandler OnEditComplete;
        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static event ConsentManagerFailedEventHandler OnEditError;

        public delegate void ConsentManagerCompletedEventHandler();

        public delegate void ConsentManagerFailedEventHandler(OguryError error);

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static void Ask()
        {
            SetupCallbacks();
#if UNITY_EDITOR
            if (OnAskComplete != null)
            {
                OnAskComplete();
            }
#elif UNITY_ANDROID
            ConsentManager.Call("ask");
#elif UNITY_IOS
            ogury_consentManager_ask();
#endif
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static void Edit()
        {
            SetupCallbacks();
#if UNITY_EDITOR
            if (OnEditComplete != null)
            {
                OnEditComplete();
            }
#elif UNITY_ANDROID
            ConsentManager.Call("edit");
#elif UNITY_IOS
            ogury_consentManager_edit();
#endif
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static string IabString
        {
            get
            {
#if UNITY_EDITOR
                return "";
#elif UNITY_ANDROID
                return ConsentManager.Call<string>("getIABString");
#elif UNITY_IOS
                return ogury_consentManager_iabString();
#endif
            }
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool IsAccepted(string vendorSlug)
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
            return ConsentManager.Call<bool>("isAccepted", vendorSlug);
#elif UNITY_IOS
            return ogury_consentManager_isAccepted(vendorSlug);
#endif
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool IsPurposeAccepted(OguryPurpose purpose)
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
            return ConsentManager.Call<bool>("isPurposeAccepted",purpose.ToString());
#elif UNITY_IOS
            return ogury_consentManager_isPurposeAccepted(purpose.ToString());
#endif
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool IsSpecialFeatureAccepted(int specialFeatures)
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
            return ConsentManager.Call<bool>("isSpecialFeatureAccepted", specialFeatures);
#elif UNITY_IOS
            return ogury_consentManager_isSpecialFeatureAccepted(specialFeatures);
#endif
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool GdprApplies
        {
            get
            {
#if UNITY_EDITOR
                return true;
#elif UNITY_ANDROID
                return ConsentManager.Call<bool>("gdprApplies");
#elif UNITY_IOS
                return ogury_consentManager_gdprApplies();
#endif
            }
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool EditAvailable
        {
            get
            {
#if UNITY_EDITOR
                return true;
#elif UNITY_ANDROID
                return ConsentManager.Call<bool>("isEditAvailable");
#elif UNITY_IOS
                return ogury_consentManager_editAvailable();
#endif
            }
        }

        [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
        public static bool HasPaid
        {
            get
            {
#if UNITY_EDITOR
                return false;
#elif UNITY_ANDROID
                return ConsentManager.Call<bool>("hasPaid");
#elif UNITY_IOS
                return ogury_consentManager_hasPaid();
#endif
            }
        }

        private static bool _areCallbacksSetup = false;

        private static void SetupCallbacks()
        {
            if (_areCallbacksSetup || !OguryCallbacks.CheckIfPresent())
            {
                return;
            }

            _areCallbacksSetup = true;

            OguryCallbacks.Instance.LegacyOnAskComplete += () =>
            {
                if (OnAskComplete != null)
                {
                    OnAskComplete.Invoke();
                }
            };
            OguryCallbacks.Instance.LegacyOnAskError += (instanceId, error) =>
            {
                if (OnAskError != null)
                {
                    OnAskError.Invoke(error);
                }
            };
            OguryCallbacks.Instance.LegacyOnEditComplete += () =>
            {
                if (OnEditComplete != null)
                {
                    OnEditComplete.Invoke();
                }
            };
            OguryCallbacks.Instance.LegacyOnEditError += (instanceId, error) =>
            {
                if (OnEditError != null)
                {
                    OnEditError.Invoke(error);
                }
            };
        }
    }

    [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
    public enum OguryPurpose
    {
        Information,
        Personalisation,
        Ad,
        Content,
        Measurement
    }

    [ObsoleteAttribute("OguryConsentManager has been deprecated in favor of OguryChoiceManager that support both TCFv1 and TCFv2.", false)]
    public class OgurySpecialFeature
    {
        public static readonly int PreciseGeolocation = 1;
    }
}