using GoogleMobileAds.Api;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdManager : MonoBehaviour
{
private BannerView bannerView;
private InterstitialAd interstitialAd;

// tránh spam quảng cáo
private bool hasShownAd = false;

void Start()
{
    MobileAds.SetRequestConfiguration(
        new RequestConfiguration
        {
            TestDeviceIds = new List<string>()
            {
                AdRequest.TestDeviceSimulator
            }
        });

    MobileAds.Initialize(initStatus =>
    {
        Debug.Log("AdMob Initialized");

        LoadBanner();
        LoadInterstitial();
    });
}

// =========================
// BANNER
// =========================

void LoadBanner()
{
    string adUnitId = "ca-app-pub-3940256099942544/6300978111";

    if (bannerView != null)
    {
        bannerView.Destroy();
        bannerView = null;
    }

    bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

    AdRequest request = new AdRequest();

    bannerView.LoadAd(request);

    bannerView.OnBannerAdLoaded += () =>
    {
        Debug.Log("Banner Loaded");
    };

    bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
    {
        Debug.Log("Banner Failed: " + error);
    };
}

// =========================
// INTERSTITIAL
// =========================

void LoadInterstitial()
{
    string adUnitId = "ca-app-pub-3940256099942544/1033173712";

    AdRequest request = new AdRequest();

    InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
    {
        if (error != null || ad == null)
        {
            Debug.Log("Interstitial load failed: " + error);
            return;
        }

        Debug.Log("Interstitial loaded");

        interstitialAd = ad;

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ad closed");

            // load interstitial mới
            LoadInterstitial();

            // reload banner để tránh banner bị mất
            ReloadBanner();
        };

        if (!hasShownAd)
        {
            StartCoroutine(ShowAdDelayed());
        }
    });
}

IEnumerator ShowAdDelayed()
{
    hasShownAd = true;

    yield return new WaitForSeconds(0.5f);

    if (interstitialAd != null && interstitialAd.CanShowAd())
    {
        interstitialAd.Show();
    }
}

// =========================
// SHOW WHEN NEEDED
// =========================

public void ShowInterstitial()
{
    if (interstitialAd != null && interstitialAd.CanShowAd())
    {
        interstitialAd.Show();
    }
}

// =========================
// RELOAD BANNER
// =========================

void ReloadBanner()
{
    if (bannerView != null)
    {
        bannerView.Destroy();
        bannerView = null;
    }

    LoadBanner();
}

void OnDestroy()
{
    if (bannerView != null)
    {
        bannerView.Destroy();
    }
}

}