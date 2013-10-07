using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
	
	//A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;  
	
	//The maximum and minimum available recording frequencies  
    private int minFreq;  
    private int maxFreq;
	
	//A handle to the attached AudioSource  
    private AudioSource goAudioSource;  
	
	private string m_micSelected = null;
    private AudioClip m_clip;
	private float clipTime;
	private GamePlayManager gamePlayManager;
	
	void Awake(){
		gamePlayManager = GameObject.Find("Gameplay_manager").GetComponent<GamePlayManager>();
	}
 
    
	
	// Use this for initialization
	void Start () {
		
		clipTime = gamePlayManager.song.clip.length;
		
		//Check if there is at least one microphone connected  
        if(Microphone.devices.Length <= 0){  
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");  
        } 
		else //At least one microphone is present  
        {  
            //Set 'micConnected' to true  
            micConnected = true;  
  
            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);  
  
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if(minFreq == 0 && maxFreq == 0)  
            {  
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = ConstantVariable.maxFrequency;  
            }  
  
            //Get the attached AudioSource component  
            goAudioSource = this.GetComponent<AudioSource>();  
			
			
			if(!Microphone.IsRecording(null))  
            {  
				StartCoroutine(RecordPlayAnalyse());
                //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                //goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);  
            }  
        }  
		
	}
	
	void Update() {
		
		if(Microphone.IsRecording(null)){
			
		}
		
        float[] spectrum = audio.GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);
        int i = 1;
        while (i < 1023) {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
            i++;
        }
    }
	
	void OnGUI()   
    {  
        //If there is a microphone  
        if(micConnected)  
        {  
            //If the audio from any microphone isn't being captured  
            if(!Microphone.IsRecording(null))  
            {  
                //Case the 'Record' button gets pressed  
                if(GUI.Button(new Rect(100, 25, 200, 50), "Record"))  
                {  
					StartCoroutine(RecordPlayAnalyse());
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                    //goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);  
                }  
            }  
            else //Recording is in progress  
            {  
                //Case the 'Stop and Play' button gets pressed  
                if(GUI.Button(new Rect(100, 25, 200, 50), "Stop and Play!"))  
                {  
                    Microphone.End(null); //Stop the audio recording  
                    goAudioSource.Play(); //Playback the recorded audio  
                }  
  
                GUI.Label(new Rect(Screen.width/2-100, Screen.height/2+25, 200, 50), "Recording in progress...");  
            }  
        }  
		else // No microphone  
		{  
            //Print a red "Microphone not connected!" message at the center of the screen  
            GUI.contentColor = Color.red;  
            GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-25, 200, 50), "Microphone not connected!");  
        }  
  
    }  
	
	IEnumerator RecordPlayAnalyse() {
		// record a two-second clip
		m_clip = Microphone.Start(m_micSelected, false, (int)clipTime + 1, AudioSettings.outputSampleRate);
		yield return null;
		
		while (Microphone.GetPosition(m_micSelected) < 10) {
		 	yield return null;
		}
		
		audio.clip = m_clip;
		audio.Play();
 
    }

}
