using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LyricsManager : MonoBehaviour
{
	
	public string lyricsFileName;
	public FileInfo lyrics;
	public AudioSource song;
	public GUIText lyricsText;
	public GUIText lyricsTextNext;
	public GUITexture lyricsElapsed;
	public GUIText debugText;
	public UILabel lyricsNGUI;
	public UILabel lyricsNextNGUI;
	public UISlider lyricsElapsedNGUI;
	private string[] lyricsArray;
	private List<string> lyricsContainer;
	private List<float> timeContainer;
	private string sPattern;
	private float time;
	private int count;
	private ObjectSend dataSend;
	private GlobalSetting globalSetting;

	void Awake ()
	{
		globalSetting = GameObject.Find (ConstantVariable.GlobalSetting).GetComponent<GlobalSetting> ();
		if(!globalSetting.debugMode) {
			dataSend = GameObject.Find (ConstantVariable.DataSend).GetComponent<ObjectSend> ();
		}
		
		lyricsContainer = new List<string> ();
		timeContainer = new List<float> ();
		sPattern = @"\[([\d{2}:\d{2}:\d{2}\]])";
	}
	// Use this for initialization
	void Start ()
	{
		if (dataSend != null) {
			//Debug.Log ("Data send ada");
			lyricsFileName = dataSend.song.lyrics;
		}
		lyrics = new FileInfo ("Assets/Resources/Lyrics/" + lyricsFileName);
		StreamReader reader = lyrics.OpenText ();
		count = 0;
		string text; 
		do {
			text = reader.ReadLine ();
			if (ValidateLyricsLine (text)) {
				lyricsContainer.Add (text);
				timeContainer.Add (ConvertToTime (text));
			}
			//Debug.Log(text);
		} while (text != null);          
		
		
	}
	// Update is called once per frame
	void Update ()
	{
		if (song.isPlaying) {
			if (song.time > timeContainer [count + 1]) {
				count++;	
			}
			
			if (count < lyricsContainer.Count - 1) {
				lyricsNGUI.text = TrimLyrics (lyricsContainer [count]);
				lyricsText.text = TrimLyrics (lyricsContainer [count]);
				if (lyricsContainer.Count - count > 1) {
					lyricsTextNext.text = TrimLyrics (lyricsContainer [count + 1]);
					lyricsNextNGUI.text = TrimLyrics (lyricsContainer [count + 1]);
				} else {
					lyricsTextNext.text = "FINISHED";
				}
				
				float interlude = song.time - timeContainer [count];
				float distance = timeContainer [count + 1] - timeContainer [count];
				
				float percent = interlude / distance;
				float xPos = lyricsText.transform.position.x;
				float yPos = lyricsText.transform.position.y;

				lyricsElapsedNGUI.sliderValue = percent;
			} else {
				lyricsText.text = "FINISHED";
			}
			
			
			
		}
	}

	private bool ValidateLyricsLine (string text)
	{
		bool valid = false;
		if (text != null) {
			if (System.Text.RegularExpressions.Regex.IsMatch (text, sPattern)) {
				//Debug.Log("SUCCESS: " + text);
				valid = true;
			} else {
				//Debug.Log("FAILED: " + text);
			}
		}
		
		return valid;
	}

	private string TrimLyrics (string text)
	{
		string display = "";
		int idx = text.IndexOf (']');
		display = text.Substring (idx + 1);
		return display;
	}

	private float ConvertToTime (string text)
	{
		float time = 0;
		if (System.Text.RegularExpressions.Regex.IsMatch (text, sPattern)) {
			string container = text.Substring (1, text.IndexOf (']') - 1);
			string[] containers = container.Split (':');
//			foreach(string s in containers){
//				Debug.Log(s);	
//			}
			
			float minute = float.Parse (containers [0]);
			minute *= 60;
			float second = float.Parse (containers [1]);
			time = minute + second;
			//Debug.Log ("Time: " + time);
		} else {
			time = -1;	
		}
		
		return time;
	}
}
