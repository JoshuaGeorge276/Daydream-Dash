using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    public static AdManager Instance { set; get; }

    private BannerView bannerAd;
    private InterstitialAd videoAd;

    private void Awake() { 
        if (Instance && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RequestIntertitialAds();
    }

    public void ShowBanner(AdPosition pos) {
        string bannerId = "ca-app-pub-7475177449573756/8730725046";
        

        AdRequest request = new AdRequest.Builder().Build();
        bannerAd = new BannerView(bannerId, AdSize.Banner, pos);
        bannerAd.LoadAd(request);
        bannerAd.Show();
    }

    public void ShowInterstitialAd() {
        if (videoAd.IsLoaded()) {
            videoAd.Show();
        } else {
            Debug.Log("Not yet loaded");
        }
    }

    public void RequestIntertitialAds() {
        string videoID = "ca-app-pub-7475177449573756/1622391216";
        

        videoAd = new InterstitialAd(videoID);

        AdRequest request = new AdRequest.Builder().Build();

        videoAd.OnAdOpening += HandleOnAdOpening;
        videoAd.OnAdClosed += HandleOnAdClosed;

        videoAd.LoadAd(request);
    }

    private void HandleOnAdClosed(object sender, System.EventArgs e) {
        videoAd.Destroy();
        RequestIntertitialAds();
        MusicPlayer.SetVolume(1);
    }

    private void HandleOnAdOpening(object sender, System.EventArgs e) {
        MusicPlayer.SetVolume(0);
        Time.timeScale = 0;
    }

    public BannerView BannerAd() {
        return bannerAd;
    }
}
