using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private static MusicPlayer instance;
	private static AudioSource audioSource;
	private static AudioClip mainMenuMusic, gameMusic, shopMusic;

	// Use this for initialization
	void Awake () {
		if (instance && instance != this) {
			Destroy(this.gameObject);
			return;
		} 

		instance = this;
	
		audioSource = GetComponent<AudioSource> ();
		mainMenuMusic = audioSource.clip;
		DontDestroyOnLoad (this.gameObject);
	}

	public static void PlayMainMenuMusic(){
        if (audioSource.clip == mainMenuMusic) {
            return;
        }
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
    }

	public static void PlayGameMusic(){
		if (!gameMusic) {
			gameMusic = Resources.Load ("Music/BackgroundMusic") as AudioClip;
		}
		if (audioSource.clip != gameMusic) {
			audioSource.clip = gameMusic;
			audioSource.Play ();
		}
	}

	public static void PlayShopMusic(){
		if (!shopMusic) {
			shopMusic = Resources.Load ("Music/happy") as AudioClip;
		}
		audioSource.clip = shopMusic;
		audioSource.Play ();
	}

	public static void StopMusic(){
		audioSource.Stop();
	}

    public static void SetVolume(int volume) {
        SetVolume(volume);
    }
}
