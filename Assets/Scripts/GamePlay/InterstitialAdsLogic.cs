using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class GameOverInterstitialADS : MonoBehaviour
{
    public static GameOverInterstitialADS instance;

    private InterstitialAd interstitialAd;
    private Action onAdClosed;

    // TEST ID Android
    private string adUnitId = "ca-app-pub-3940256099942544/1033173712";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        Debug.Log("🔄 Loading Interstitial Ad...");

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("❌ Interstitial load failed: " + error);
                return;
            }

            Debug.Log("✅ Interstitial Loaded");

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("📌 Interstitial Closed -> Reload");

                onAdClosed?.Invoke();
                onAdClosed = null;

                LoadAd();
            };

            interstitialAd.OnAdFullScreenContentFailed += (AdError err) =>
            {
                Debug.Log("❌ Interstitial Show Failed: " + err);

                onAdClosed?.Invoke();
                onAdClosed = null;

                LoadAd();
            };
        });
    }

    // =========================
    // SHOW ADS
    // =========================
    public void ShowAd(Action onClosed = null)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("🎬 Show Interstitial Ad");
            onAdClosed = onClosed;
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("❌ Interstitial chưa sẵn sàng");
            LoadAd();

            onClosed?.Invoke();
        }
    }
}