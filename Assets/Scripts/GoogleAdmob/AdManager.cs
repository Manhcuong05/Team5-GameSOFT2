using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    //chỉ show 1 lần toàn game
    private static bool hasShownInterstitial = false;

    void Start()
    {
        Debug.Log("🚀 Initializing AdMob...");

        MobileAds.Initialize(initStatus =>
        {
            LoadBanner();
            LoadInterstitial();
        });
    }

    // BANNER
    void LoadBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();

        bannerView.OnBannerAdLoaded += () =>
        {
            bannerView.Show();
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            
        };

        bannerView.LoadAd(request);
    }

    // INTERSTITIAL
    void LoadInterstitial()
    {
        //nếu đã show rồi thì không load nữa
        if (hasShownInterstitial) return;

        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        AdRequest request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                return;
            }

            interstitialAd = ad;

            //chỉ show nếu chưa từng show
            if (!hasShownInterstitial)
            {
                hasShownInterstitial = true;

                Debug.Log("🎬 Show Interstitial (ONLY ONCE)");
                interstitialAd.Show();
            }
        });
    }

    // CLEANUP
    void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}