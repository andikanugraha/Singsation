using UnityEngine;
using System.Collections;

public class SpectrumManager : MonoBehaviour {

	public Texture texture;
	public GUIText debugText;
	public int sample = 128;
	
	void Update(){
//		float[] spectrum = audio.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
//        int i = 1;
//        while (i < 1023) {
//            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
//            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
//            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
//            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
//            i++;
//        }	
	}
	
	void OnGUI(){
		float[] spectrum = audio.GetSpectrumData(sample, 0, FFTWindow.BlackmanHarris);
		for(int i = 0; i < sample; i++){
			float width = Screen.width / sample;
			float xLeft = i * width;
			float yTop = Screen.height;
			float height = spectrum[i] * Screen.height;
			
			GUI.DrawTexture( new Rect( xLeft, yTop, width, -height), texture);
		}
		debugText.text = spectrum[0] + " - " + spectrum[1];
		
	}
}
