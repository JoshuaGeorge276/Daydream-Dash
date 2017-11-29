using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativePickup : MonoBehaviour {

    private int randomEffectNumber;

	// Use this for initialization
	void Start () {
		randomEffectNumber = (Random.Range(0, 3)) % 3;
	}

    private void FixedUpdate() {
        transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.CompareTag("Player")) {
			switch (randomEffectNumber) {
			case 0: 
				HideUI ();
				break;
			case 1: 
				ChangePlayerColour (other.GetComponent<Player> ());
				break;
			case 2:
				PixelateScreen ();
				break;
			}
        }
        Destroy(gameObject);
    }

    void HideUI() {
		// Uses an animation to decide what length the UI will be hidden for.
        FindObjectOfType<UIManager>().HideUIAfterPickup();
    }

    void ChangePlayerColour(Player player) {
		// Changes the players hat to the next colour by calling a method on the player.
        if(player != null) {
            player.ChangeColourNoCheck();
        }
    }

	void PixelateScreen(){
		GameObject.FindObjectOfType<ScreenEffectScript> ().PixelateScreen ();
	}
}
