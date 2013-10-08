using UnityEngine;
using System.Collections;

public class SpectrumManager : MonoBehaviour {

//	public Texture texture;
//	public GUIText debugText;
//	public int sample = 128;
//	
//	void OnGUI(){
//		float[] spectrum = audio.GetSpectrumData(sample, 0, FFTWindow.BlackmanHarris);
//		for(int i = 0; i < sample; i++){
//			float width = Screen.width / sample;
//			float xLeft = i * width;
//			float yTop = Screen.height;
//			float height = spectrum[i] * Screen.height;
//			
//			GUI.DrawTexture( new Rect( xLeft, yTop, width, -height), texture);
//		}
//		debugText.text = spectrum[0] + " - " + spectrum[1];
//		
//	}
	
	
	public GUIText display; // drag a GUIText here to show results
	public UISlider decibelBar;
	
	int qSamples = 1024;  // array size
	float refValue = 0.1f; // RMS value for 0 dB
	float threshold = 0.02f;      // minimum amplitude to extract pitch
	float rmsValue;   // sound level - RMS
	float dbValue;    // sound level - dB
	public float pitchValue; // sound pitch - Hz
	public float maxDecibel = 13; //maximum decibel

	private float[] samples; // audio samples
	private float[] spectrum; // audio spectrum

	void Start () {
		samples = new float[qSamples];
		spectrum = new float[qSamples];
	}

	public void AnalyzeSound(){
		audio.GetOutputData(samples, 0); // fill array with samples
		int i;
		float sum = 0;
		for (i = 0; i < qSamples; i++){
			sum += samples[i] * samples[i]; // sum squared samples
		}
		rmsValue = Mathf.Sqrt(sum/qSamples); // rms = square root of average
		dbValue = 20 * Mathf.Log10(rmsValue/refValue); // calculate dB
		if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
		// get sound spectrum
		audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
		float maxV = 0;
		int maxN = 0;
		for (i = 0; i < qSamples; i++){ // find max 
			if (spectrum[i] > maxV && spectrum[i] > threshold){
				maxV = spectrum[i];
				maxN = i; // maxN is the index of max
			}
		}
		float freqN = maxN; // pass the index to a float variable
		if (maxN > 0 && maxN < qSamples-1){ // interpolate index using neighbours
			float dL = spectrum[maxN-1]/spectrum[maxN];
			float dR = spectrum[maxN+1]/spectrum[maxN];
			freqN += 0.5f * (dR*dR - dL*dL);
		}
		pitchValue = freqN * 24000/qSamples; // convert index to frequency
	}

	void Update () {
		if (Input.GetKeyDown("p")){
			audio.Play();
		}
		AnalyzeSound();
		if (display){ 
			display.text = "RMS: "+rmsValue.ToString("F2")+
				" ("+dbValue.ToString("F1")+" dB)\n"+
					"Pitch: "+pitchValue.ToString("F0")+" Hz";
		}
		
		if(decibelBar) {
			decibelBar.sliderValue = dbValue / maxDecibel;
		}	
		
		//decibelBar.sliderValue = 0.8f;
	}


}
