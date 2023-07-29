using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IAdsService
    {
        private string _androidGameId="5348167";
        private string _iOSGameId="5348166";
        private bool _testMode = true;
        
        private string _androidAdUnitId = "Rewarded_Android";
        private string _iOSAdUnitId = "Rewarded_iOS";

        private string _adUnitId = null;
        private string _gameId;
        private Action _onVideoFineshed;

        public bool IsRewardedVideoReady { get; set; }
        public event Action RewardedVideoReady;

        public int Reward => 25;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = _androidGameId;
                    _adUnitId = _androidAdUnitId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = _iOSGameId;
                    _adUnitId = _iOSAdUnitId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = _androidGameId;
                    _adUnitId = _androidAdUnitId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }

            LoadAd();
        }

        public void LoadAd()
        {
            Advertisement.Load(_adUnitId, this);

            IsRewardedVideoReady = true;
        }

        public void ShowAd(Action onVideoFineshed)
        {
            Advertisement.Show(_adUnitId, this);

            _onVideoFineshed = onVideoFineshed;

            IsRewardedVideoReady = false;
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (placementId == _adUnitId)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.LogError($"OnUnityAdsDidFinish{showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFineshed?.Invoke();
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.LogError($"OnUnityAdsDidFinish{showCompletionState}");
                    break;
                default:
                    Debug.LogError($"OnUnityAdsDidFinish{showCompletionState}");
                    break;
            }

            _onVideoFineshed = null;
            LoadAd();
        }
    }
}