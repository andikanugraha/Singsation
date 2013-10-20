using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class NotesManager : MonoBehaviour
{
	public string notesFileName;
	public FileInfo notes;
	public AudioSource song;
	public GUIText notesText;
	public GUIText debugText;
	public Dictionary<float, float> notesContainer;	// hold the notes
	public Dictionary<float, float> timeContainer; 	// hold the time / duration
	public Dictionary<float, string> lyricsContainer; 	// hold the lyrics
	public List<float> lineBreakContainer; //menyimpan posisi baris lirik
	public float position; //posisi lagu disetel
	public int currentLine; //posisi baris lirik saat ini
	public float bpm; //beat per minute
	public float beatTime; //lama ketukan dalam detik
	
	/*
	 * Menggunakan Composer hasil file notes.txt akan menghasilkan BPM standar: 180 BPM
	 * Ultrastar menggunakan beat 4 kali BPM
	 * Jadi lama satu ketukan sama dengan 60 detik / 180 BPM / 4 
	 *
	 */
	
	private string[] notesArray;
	private List<string> lineContainer; 				//hold text per line
	private float deltaTime;
	private float lastPosition;
	private ObjectSend dataSend;
	private GlobalSetting globalSetting;

	void Awake ()
	{
		globalSetting = GameObject.Find (ConstantVariable.GlobalSetting).GetComponent<GlobalSetting> ();
		if (!globalSetting.debugMode) {
			dataSend = GameObject.Find (ConstantVariable.DataSend).GetComponent<ObjectSend> ();
		}
		
		notesContainer = new Dictionary<float, float> ();
		timeContainer = new Dictionary<float, float> ();
		lyricsContainer = new Dictionary<float, string> ();
		lineBreakContainer = new List<float> ();
		lineContainer = new List<string> ();
		deltaTime = 0;
		position = 0;
		currentLine = 0;
		bpm = 180;
		beatTime = 0;
	}

	void Start ()
	{
		if (dataSend != null) {
			//Debug.Log ("Data send exist");
			notesFileName = dataSend.song.notes;
		}
		ReadFile (notesFileName);
		beatTime = 60 / bpm / 4;
		//Debug.Log("Beat time: " + beatTime);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (song.isPlaying) {
			
			//menambah posisi lagu berjalan berdasarkan waktu ketukan / beat time
			if(deltaTime >= beatTime){
				while(deltaTime >= beatTime) {
					deltaTime -= beatTime;
					position++;	
				}
			}
			deltaTime += Time.deltaTime;
			
			if(position >= lineBreakContainer[currentLine]){
				currentLine++;	
			}
			
			if (position < lastPosition) {
				if (notesContainer.ContainsKey (position)) {
					string noteTxt = System.Convert.ToString (notesContainer [position]);
					noteTxt += " (" + NoteFrequencyManager.NoteToChromatic (notesContainer [position]) + ")";
					string timeTxt = System.Convert.ToString (timeContainer [position]);
					string lyricsTxt = System.Convert.ToString (lyricsContainer [position]);
					notesText.text = "Notes: " + noteTxt + "\nTime: " + timeTxt + "\nLyrics: " + lyricsTxt; 
					notesText.text += "\nLine: " + currentLine;
				}
			} else {
				notesText.text = "FINISHED";
			}
			debugText.text = System.Convert.ToString (position);
		}
	}
	
	private void ReadFile (string notesFileName)
	{
		string targetFile = "Assets/Resources/Notes/" + notesFileName;
		notes = new FileInfo (targetFile);
		StreamReader reader = notes.OpenText ();
		position = 0;
		string text = ""; 
		while(text != null) {
			text = reader.ReadLine ();
			lineContainer.Add (text);
			
			if(text != null && text.StartsWith("#BPM")){
				bpm = float.Parse(text.Substring(text.IndexOf(':') + 1));
				//Debug.Log(text + "\nBPM: " + bpm);
			}
			
			
			if (ValidateNotesLine (text)) {
				string [] split = text.Split (new char []{' '});
				//Debug.Log("split 0: " + split[1]);
				if (split [0].Equals (":")) {
					float pos = float.Parse (split [1]);
					float time = float.Parse (split [2]);
					float note = float.Parse (split [3]);
					string lyrics = split [4];
					if (lastPosition < pos) {
						lastPosition = pos;	
					}
					//Debug.Log("pos: " + pos);
					notesContainer.Add (pos, note);
					timeContainer.Add (pos, time);
					lyricsContainer.Add (pos, lyrics);
				} else if(split [0].Equals ("-")){
					
					float pos = float.Parse (split [1]);
					//float newPos = float.Parse (split [2]);
					lineBreakContainer.Add (pos);
					//Debug.Log("LINE BREAK: " + pos);
				}
			}
			//Debug.Log(text);
		}          
	}

	private bool ValidateNotesLine (string text)
	{
		bool valid = false;
		if (text != null) {
			if (text [0].Equals (':') || text [0].Equals ('-')) {
				valid = true;	
				//Debug.Log("TRUE: " + text);	
			} else {
				//Debug.Log("FAILED: " + text);
			}
		}
		
		return valid;
	}

}
