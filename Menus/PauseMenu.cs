using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : SimpleMenu<PauseMenu>
{
	public Text unpauseText;

	private GameManager gameManager;

	void Start(){
		gameManager = FindObjectOfType<GameManager> ();
		gameManager.PauseGame ();
	}

	public new void OnQuitPressed()
	{
		Hide();
		Destroy(this.gameObject); // This menu does not automatically destroy itself
		GameMenu.Hide();
	}

	public void OnRestartPressed(){
		gameManager.ResumeGame ();
		GameManager.ReloadScene ();
	}

	public override void OnBackPressed ()
	{
		base.OnBackPressed ();
		gameManager.ResumeGame ();
	}

	public override void BackToMainMenu(){
		gameManager.ResumeGame ();
		base.BackToMainMenu ();
	}

	public void SetText(string text){
		unpauseText.text = text;
	}
}
