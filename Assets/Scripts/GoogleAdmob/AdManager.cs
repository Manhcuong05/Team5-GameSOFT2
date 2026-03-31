using GoogleMobileAds.Api;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    private bool hasShownInterstitialOnMenu = false;
    private bool bannerLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
            LoadInterstitial();
        });

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // ✅ CHỈ load banner khi vào menu lần đầu
        if (scene.name == "MainMenu")
        {
            if (!bannerLoaded)
            {
                bannerLoaded = true;
                LoadBanner();
            }
            else
            {
                ShowBanner(); // đã có thì show lại
            }

            if (!hasShownInterstitialOnMenu)
            {
                hasShownInterstitialOnMenu = true;
                StartCoroutine(ShowAdWhenReady());
            }
        }
    }

    // =========================
    // BANNER (LOAD ĐÚNG THỜI ĐIỂM)
    // =========================
    void LoadBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);

        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("✅ Banner Loaded");
            bannerView.Show();
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("❌ Banner Failed: " + error);
        };
    }

    void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
        }
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
                Invoke(nameof(LoadInterstitial), 2f);
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                interstitialAd = null;
                LoadInterstitial();
            };
        });
    }

    IEnumerator ShowAdWhenReady()
    {
        float timeout = 5f;
        float timer = 0f;

        while ((interstitialAd == null || !interstitialAd.CanShowAd()) && timer < timeout)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
    }

    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}