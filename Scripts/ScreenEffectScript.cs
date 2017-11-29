using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectScript : MonoBehaviour {

	private GameObject pixelPanel;
	
	public void PixelateScreen(){
		// Sets active a pixelated Panel in the input canvas which will remain active for 10-20 seconds.
		if(!pixelPanel){
			pixelPanel = transform.GetChild(0).gameObject;
		}
		pixelPanel.SetActive (true);
		Invoke ("DisablePixelPanel", Random.Range(10, 20));
	}

	void DisablePixelPanel(){
		pixelPanel.SetActive (false);
	}
}
