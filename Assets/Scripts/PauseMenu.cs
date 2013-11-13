using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
//	public float widthBox = 200;
//	public float heightBox = 200;
//	
//	public float widthButton = 100;
//	public float heightButton = 30;
//	public float spaceButton = 10;
	
	public tk2dUIItem resumeButton;
	public tk2dUIItem optionButton;
	public tk2dUIItem exitButton;
	
	void Start () {
		resumeButton.OnClick += ResumeButtonClick;
		optionButton.OnClick += OptionButtonClick;
		exitButton.OnClick += ExitButtonClick;
	}
	
	void Update() {
		if(GamePlayManager.state == GamePlayManager.State.Pause) {
			if(!gameObject.activeSelf) gameObject.SetActive(true);	
		} else if(GamePlayManager.state == GamePlayManager.State.Play) {
			if(gameObject.activeSelf) gameObject.SetActive(false);	
		}
	}
	
	private void ResumeButtonClick() {
		GamePlayManager.state = GamePlayManager.State.Play;	
	}
	
	private void OptionButtonClick() {
		
	}
	
	private void ExitButtonClick() {
		Application.LoadLevel(0);
	}
	
//	void OnGUI () {
//		
//		float xPos = (Screen.width - widthBox) / 2;
//		float yPos = (Screen.height - heightBox) / 2 + 40;
//		
//		// Make a background box
//		GUI.Box(new Rect(xPos, yPos, widthBox, heightBox), "Pause Game");
//		
//		xPos = (Screen.width - widthButton) / 2;
//		yPos = (Screen.height - heightButton) / 2;
//		
//		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
//		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Resume")) {
//			GamePlayManager.state = GamePlayManager.State.Play;
//		}
//		
//		yPos += heightButton + spaceButton;
//
//		// Make the second button.
//		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Restart")) {
//			Application.LoadLevel(1);
//		}
//		
//		yPos += heightButton + spaceButton;
//
//		// Make the second button.
//		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Main Menu")) {
//			Application.LoadLevel(0);
//		}
//		
//	}
	
}
