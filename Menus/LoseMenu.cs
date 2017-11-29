using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoseMenu : SimpleMenu<LoseMenu> {

    public Text distance, bestDistance, orbs, totalOrbs, highscore, bonusText;
    public AudioClip starAwarded, allStarsAchieved;
    public GameObject highscoreObjects;

    private Animator anim;
    private AudioSource audioSource;
    private Player player;
    private int currentOrbs, targetOrbs;

    private float duration = 0.5f, t;

    private void OnEnable() {
        AdManager.Instance.ShowInterstitialAd();
        Time.timeScale = 0.1f;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();

        currentOrbs = PlayerPrefs.GetInt("ORBS");
        targetOrbs = currentOrbs + player.GetScore();
        SetText(player.GetDistance().ToString(), player.GetScore().ToString());
        CheckHighscore();
        player.SavePrefs();
        bestDistance.text = PlayerPrefs.GetInt("DISTANCE").ToString();
        StartCoroutine("AwardStars");
    }

    private void Update() {
        if(currentOrbs < targetOrbs) {
            totalOrbs.text = Mathf.RoundToInt(Mathf.Lerp(currentOrbs, targetOrbs, t)).ToString();
            t += 0.5f * Time.unscaledDeltaTime;
        }
        
    }

    void SetText(string distance, string orbs) {
        this.distance.text = distance;
        this.orbs.text = orbs;
    }

    public void OnReplayPressed() {
        Time.timeScale = 1; // Have to set this to 1 because it gets set to 0.1 when the Lose Menu appears.
        GameManager.ReloadScene();
    }

    public override void BackToMainMenu() {
        Time.timeScale = 1; // Have to set this to 1 because it gets set to 0.1 when the Lose Menu appears.
        base.BackToMainMenu();
    }

    void CheckHighscore() {
        if (player.GetDistance() >= PlayerPrefs.GetInt("DISTANCE")){
            highscoreObjects.SetActive(true);
        }
    }

    IEnumerator AwardStars() {
        int requirementDistance = 500;
        int bonus = 0;
        audioSource.clip = starAwarded;
        for (int i = 0; i < 3; i++) {
            if(player.GetDistance() >= requirementDistance) {
                audioSource.Play();
                anim.SetBool("Star" + (i + 1), true);
                requirementDistance *= 2;
                targetOrbs += (250 * (i + 1));
                bonus++;
                yield return new WaitForSecondsRealtime(1);
            } else {
                break;
            }
        }
        bonusText.text = "+ " + (250 * bonus).ToString();
        if(requirementDistance >= 2000) {
            audioSource.PlayOneShot(allStarsAchieved, 1);
        }
    }

}
