public class GameMenu : SimpleMenu<GameMenu>
{
    private void OnEnable() {
        AdManager.Instance.RequestIntertitialAds();
    }
    public override void OnBackPressed()
	{
		PauseMenu.Show();
	}
}
