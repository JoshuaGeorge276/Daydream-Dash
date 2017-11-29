using UnityEngine;

public class MainMenu : SimpleMenu<MainMenu>
{

    protected override void Awake() {
        base.Awake();
        AdManager.Instance.ShowBanner(GoogleMobileAds.Api.AdPosition.BottomLeft);
    }
    public void OnPlayPressed()
	{
        DestroyAD();
        GameManager.playPressed = true;
		MusicPlayer.StopMusic ();
		SoundManager.PlayStartGameSound ();
		GameMenu.Show();
	}
	public override void OnBackPressed()
	{
		Application.Quit();
	}

	public void OnShopPressed(){
        DestroyAD();
		MusicPlayer.PlayShopMusic ();
		ShopMenu.Show ();
	}

    public void OnStatsPressed() {
        StatsMenu.Show();
    }

    void DestroyAD() {
        AdManager.Instance.BannerAd().Hide();
        AdManager.Instance.BannerAd().Destroy();
    }
}
