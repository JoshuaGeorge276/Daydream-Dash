public class ShopMenu : SimpleMenu<ShopMenu> {

    private void OnEnable() {
        Invoke("ShowAdDelayed", 2);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        AdManager.Instance.BannerAd().Destroy();
    }

    public void OpenPurchaseMenu() {
        PurchaseMenu.Open();
    }

    void ShowAdDelayed() {
        AdManager.Instance.ShowBanner(GoogleMobileAds.Api.AdPosition.Top);
    }
}
