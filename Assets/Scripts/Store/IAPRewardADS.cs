using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class IAPRewardADS : MonoBehaviour
{
    public static IAPRewardADS instance;

    private RewardedAd rewardedAd;
    private Action onRewardSuccess;

    private string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // TEST ID

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadAd();
    }

    // =========================
    // LOAD ADS
    // =========================
    public void LoadAd()
    {
        Debug.Log("🔄 Loading Rewarded Ad...");

        AdRequest request = new AdRequest();

        RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("❌ Load failed: " + error);
                return;
            }

            Debug.Log("✅ Rewarded Ad Loaded");

            rewardedAd = ad;

            // Event khi đóng ads
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Ad Closed → Reload");
                LoadAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError err) =>
            {
                Debug.Log("Ad Show Failed: " + err);
                LoadAd();
            };
        });
    }

    // =========================
    // SHOW ADS
    // =========================
    public void ShowAd(Action onSuccess)
    {
        if (rewardedAd != null)
        {
            Debug.Log("🎬 Show Rewarded Ad");

            onRewardSuccess = onSuccess;

            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("🎁 Reward Received: " + reward.Amount);
                onRewardSuccess?.Invoke();
            });
        }
        else
        {
            Debug.Log("❌ Ad chưa sẵn sàng");
            LoadAd();
        }
    }
}