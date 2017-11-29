using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    [HideInInspector]
	public static bool playPressed;	// Whether the player has pressed play in the main menu screen.
    public static bool gameStarted; // Whether the player has touched the screen in the game menu.
	public static int attempts;

	public Canvas inputCanvas;

    private bool isGamePaused = false;
    private Transform initialGamePanel;

    private void Start() {
        gameStarted = false;
        initialGamePanel = inputCanvas.transform.Find("Initialise Game");
    }

    public void PauseGame() {
		SoundManager.PlayPauseMenuSound ();
        Time.timeScale = 0;
		inputCanvas.enabled = false;
        isGamePaused = true;
    }

    public void ResumeGame() {
		SoundManager.PlayPauseMenuSound ();
        Time.timeScale = 1;
		inputCanvas.enabled = true;
        isGamePaused = false;
    }

	public static void ReloadScene() {
        attempts++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public static void BackToMainMenu(){
		playPressed = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

    public void InitGame() {
        // Used to when the player drags the screen to start the actual game
        if (!gameStarted) {
            gameStarted = true;
            inputCanvas.enabled = true;
            MusicPlayer.PlayGameMusic();
            FindObjectOfType<UIManager>().AnimateMainText();
            initialGamePanel.gameObject.SetActive(false);
        }
        return;
    }
}
