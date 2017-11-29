using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text mainText, scoreText, distanceText;
	public Image nextColour, prevColour, tiltIcon;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
        mainText.text = "Swipe to GO!";
		scoreText.text = "0";
		FindObjectOfType<Player>().SetUIManager (this);
	}
		
    public void AnimateMainText(){
		mainText.text = "Go!";
		anim.SetTrigger("popText");

	}

	public void UpdateScoreText(int totalScore){
		scoreText.text = totalScore.ToString ();
		anim.SetTrigger ("scoreUpdate");
	}

	public void UpdateColourIndicator(Color next, Color prev){
		nextColour.color = next;
		prevColour.color = prev;
	}

    public void TriggerTiltIcon() {
        Debug.Log("Tilt ICON");
        anim.SetTrigger("isTiltPlatform");
    }

    public void UpdateDistanceTravelledText(float distance) {
        distanceText.text = distance.ToString() + "m";
    }

    public void HideUIAfterPickup() {
        anim.SetTrigger("hideUI");
    }
}
