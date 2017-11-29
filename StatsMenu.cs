using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Collections;

public class StatsMenu : SimpleMenu<StatsMenu> {

    public Text bestScoreText, shareScoreText;
    public GameObject statsCanvas, shareCanvas;

    private bool isProcessing = false;
    private bool isFocus = false;
    private string bestScoreString;

    public void ShareBtnPress() {
        if (!isProcessing) {
            statsCanvas.SetActive(false);
            shareCanvas.SetActive(true);
            StartCoroutine(ShareScreenshot());
        }
    }

    IEnumerator ShareScreenshot() {
        isProcessing = true;
        AdManager.Instance.BannerAd().Hide();

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot("screenshot.png", 2);
        string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.3f);

        if (!Application.isEditor) {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share your new score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }

        yield return new WaitUntil(() => isFocus);
        shareCanvas.SetActive(false);
        statsCanvas.SetActive(true);
        isProcessing = false;
    }

    private void OnApplicationFocus(bool focus) {
        isFocus = focus;
    }

    private void OnEnable() {
        bestScoreString = PlayerPrefs.GetInt("DISTANCE").ToString() + "m";
        bestScoreText.text = bestScoreString;
        shareScoreText.text = "I ran " + bestScoreString + " in Daydream Dash!";
    }

    public override void BackToMainMenu() {
        base.BackToMainMenu();
    }
}
