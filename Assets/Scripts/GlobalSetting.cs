using UnityEngine;
using System.Collections;

public class GlobalSetting : MonoBehaviour {
	
	public bool debugMode = true;
	public float volume = 100;
	
	public GUITexture screenBorder;
	
	void Start() {
		
//		float x = Screen.width / 2;
//		float y = Screen.height / 2;
//		screenBorder.pixelInset = new Rect(0, 0, 100, 100);	
	}
}
