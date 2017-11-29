using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceCanvas : MonoBehaviour {

    private Animator animator;
	private Text attemptsText;

	// Use this for initialization
	void Start () {
		attemptsText = GetComponentInChildren<Text> ();
        animator = GetComponent<Animator>();

		if (GameManager.attempts > 0) {
			attemptsText.text = "Attempts " + GameManager.attempts;
		}
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetBool("gameStarted", GameManager.gameStarted);

		if (GameManager.gameStarted) {
			SelfDestruct ();
		}
	}

    private void SelfDestruct() {
        Destroy(gameObject, 30);
    }
}
