using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public float widthBox = 200;
	public float heightBox = 200;
	
	public float widthButton = 100;
	public float heightButton = 30;
	public float spaceButton = 10;
	
	void OnGUI () {
		
		float xPos = (Screen.width - widthBox) / 2;
		float yPos = (Screen.height - heightBox) / 2 + 40;
		
		// Make a background box
		GUI.Box(new Rect(xPos, yPos, widthBox, heightBox), "Main Menu");
		
		xPos = (Screen.width - widthButton) / 2;
		yPos = (Screen.height - heightButton) / 2;
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Start Sing")) {
			Application.LoadLevel(1);
		}
		
		yPos += heightButton + spaceButton;

		// Make the second button.
		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Option")) {
			Application.LoadLevel(2);
		}
		
		yPos += heightButton + spaceButton;

		// Make the second button.
		if(GUI.Button(new Rect( xPos, yPos, widthButton, heightButton), "Quit")) {
			Application.Quit();
		}
		
	}
	
	
	
}
