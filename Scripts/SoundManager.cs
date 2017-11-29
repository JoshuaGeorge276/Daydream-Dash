using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private static AudioClip pickupSound, pauseMenuSound; 

    private static AudioSource audioSource;
    private static float[] audioPitches;
    private static int timesPlayed;
	private static float defaultVolume;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
		pickupSound = audioSource.clip;
		defaultVolume = audioSource.volume;
        audioPitches = new float[] { 1, 1.05f, 1.1f, 1.16f, 1.22f, 1.27f, 1.33f, 1.38f, 1.44f };
        timesPlayed = 0;
    }
	
	public static void PlayPickupSound() {
		if (audioSource.clip != pickupSound) {
			audioSource.clip = pickupSound;
		}
        audioSource.pitch = audioPitches[timesPlayed % audioPitches.Length];
        audioSource.Play();
        timesPlayed++;
    }

	public static void PlayDeathSound(){
		audioSource.volume = 1f;
        AudioClip deathClip = Resources.Load("Player/death") as AudioClip;
        AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position);
		audioSource.volume = defaultVolume;
	}

	public static void PlayStartGameSound(){
        audioSource.PlayOneShot(Resources.Load("Sounds/playsound2") as AudioClip, 1);
        audioSource.Play ();
	}

	public static void PlayPauseMenuSound(){
		if (!pauseMenuSound) {
			pauseMenuSound = Resources.Load ("Sounds/pauseenter") as AudioClip;
		}
		audioSource.clip = pauseMenuSound;
		audioSource.Play ();
	}
}
