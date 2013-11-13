using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class NoteVisualizerManager : MonoBehaviour
{

	public float noteHeight = 20;
	public float backgroundWidth = 480;
	public float backgroundHeight = 400;
	public float pivotX = 0;
	public float pivotY = 0;
	public Texture texture;
	
	public tk2dUIScrollbar notesProgress;
	public tk2dSlicedSprite noteBarBackground;
	public tk2dSprite noteIndicator;
	public GameObject noteBarParent;
	//Prefabs
	public tk2dSlicedSprite noteBarPrefabs;
	public tk2dSlicedSprite micNoteBarPrefabs;
	public tk2dTextMesh lyricsBarPrefabs;
	
	private tk2dSlicedSprite micNoteBar;
	private NotesManager notesManager;
	private NoteFrequencyManager noteFrequencyManager;
	private bool newLine;
	private int currentLine;
	private float keyPrevious;
	private float micNoteStart;
	
	private List<tk2dSlicedSprite> notesCollection;
	private List<tk2dSlicedSprite> micNotesCollection;
	private List<tk2dTextMesh> lyricsCollection;
	
	void Awake() {
		notesManager = GameObject.Find(ConstantVariable.NotesManager).GetComponent<NotesManager>();
		noteFrequencyManager = GameObject.Find (ConstantVariable.MicrophoneManager).GetComponent<NoteFrequencyManager>();
		
		//Initiate
		notesCollection = new List<tk2dSlicedSprite>();
		micNotesCollection = new List<tk2dSlicedSprite>();
		lyricsCollection = new List<tk2dTextMesh>();
		
		newLine = true; //untuk masuk ke lyrics berikut nya
		currentLine = 0;
		keyPrevious = 0;
		
	}
	
	// Use this for initialization
	void Start ()
	{	
		if(noteBarBackground) {
			backgroundWidth = noteBarBackground.dimensions.x;
			backgroundHeight = noteBarBackground.dimensions.y;
			pivotX = noteBarBackground.transform.localPosition.x;
			pivotY = noteBarBackground.transform.localPosition.y;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		//hitung posisi indikator bar saat ini dalam satu line lyrics
		float percent = 0;
		if(notesManager.currentLine <= 0) {
			percent = notesManager.position / notesManager.lineBreakContainer[notesManager.currentLine];	
		} else {
			float before = notesManager.lineBreakContainer[notesManager.currentLine - 1];
			percent = (notesManager.position - before  ) / (notesManager.lineBreakContainer[notesManager.currentLine] - before);		
		}
		
		//Note visualizer
		notesProgress.Value = percent;
		
		//Apabila memasuki line baru maka proses untuk line berikutnya
		if(newLine) {
			
			foreach(tk2dSlicedSprite sprite in notesCollection) {
				Destroy(sprite.gameObject);	
			}
			
			foreach(tk2dSlicedSprite sprite in micNotesCollection) {
				Destroy(sprite.gameObject);	
			}
			
			foreach(tk2dTextMesh label in lyricsCollection) {
				Destroy(label.gameObject);	
			}
			
			notesCollection = new List<tk2dSlicedSprite>();
			micNotesCollection = new List<tk2dSlicedSprite>();
			lyricsCollection = new List<tk2dTextMesh>();
			
			float linePos = 0;
			if(notesManager.currentLine > 0) {
				linePos = notesManager.lineBreakContainer[notesManager.currentLine - 1];
			}
			
			//create note
			SongNoteVisualizer(linePos);
			
			newLine = false;
		}
		
		//Cek apakah sudah masuk ke line berikutnya
		if(currentLine != notesManager.currentLine) {
			newLine = true;
			currentLine = notesManager.currentLine;
		}
		
		//buat note untuk visualisasi microphone
		MicrophoneNoteVisualizer(noteFrequencyManager.key, percent);
		
		
		//Note indicator
		float x = pivotX + percent * backgroundWidth;
		float y = NotePositionPanel(noteFrequencyManager.key);
		noteIndicator.transform.localPosition = new Vector3 (x, y, 0);
		
	}
	
	//Create note from song for one line
	void SongNoteVisualizer(float linePos){
		
		//Hitung jarak dari line sekarang ke line berikutnya
		float lengthUntilBreak = 0;
		lengthUntilBreak = Mathf.Abs(notesManager.lineBreakContainer[notesManager.currentLine] - linePos);
		
		//start making note
		for ( float i = linePos; i < notesManager.lineBreakContainer[notesManager.currentLine]; i++) {
			if(notesManager.notesContainer.ContainsKey(i)){
				float noteWidth = backgroundWidth / lengthUntilBreak;
				float key = notesManager.notesContainer[i];
				float distance = notesManager.timeContainer[i];
				string lyrics = notesManager.lyricsContainer[i];
				
				float posX = pivotX + (i - linePos) * noteWidth;
				float posY = NotePositionPanel(key);
				
//				UISprite note = (UISprite) Instantiate(notePrefabs, Vector3.zero, Quaternion.identity);
//				note.pivot = UIWidget.Pivot.BottomLeft;
//				note.transform.parent = panel.transform;
//				note.transform.localRotation = Quaternion.identity;
//				note.transform.localPosition = new Vector3(posX, posY, 0);
//				note.transform.localScale = new Vector3(distance * noteWidth, noteHeight, 1);
//				note.depth = ConstantVariable.NoteDepth;
//				notesCollection.Add(note);
				
				tk2dSlicedSprite noteBar = GameObject.Instantiate(noteBarPrefabs, Vector3.zero, Quaternion.identity) as tk2dSlicedSprite;
				noteBar.transform.parent = noteBarParent.transform;
				noteBar.transform.localPosition = new Vector3(posX, posY, 0);
				noteBar.dimensions = new Vector2(distance * noteWidth, noteHeight);
				notesCollection.Add(noteBar);
				
				tk2dTextMesh lyricsBar = GameObject.Instantiate(lyricsBarPrefabs, Vector3.zero, Quaternion.identity) as tk2dTextMesh;
				lyricsBar.transform.parent = noteBarParent.transform;
				lyricsBar.transform.localPosition = new Vector3(posX, posY, 0);
				lyricsBar.text = lyrics;
				lyricsCollection.Add(lyricsBar);
				
				//Add lyrics
//				UILabel labels = (UILabel) Instantiate(lyricsPrefabs, Vector3.zero, Quaternion.identity);
//				labels.pivot = UIWidget.Pivot.BottomLeft;
//				labels.transform.parent = panel.transform;
//				labels.transform.localPosition = note.transform.localPosition;
//				labels.transform.localRotation = Quaternion.identity;
//				
//				//labels.transform.localPosition = new Vector3( 0.5f , 0.5f, 0);
//				labels.transform.localScale = new Vector3(18f, 18f, 0.5f);
//				labels.text = lyricsBar;
//				labels.depth = ConstantVariable.LyricsDepth;
//				lyricsCollection.Add(labels);
			} 
		}
	}
	
	//Create note from microphone for one line
	void MicrophoneNoteVisualizer (float key, float percent) {
		float micKey = Mathf.Round(key);
		if(micKey > 0) {
			if(keyPrevious != micKey) {
				
//				micNote = (UISprite) Instantiate(micNotePrefabs, Vector3.zero, Quaternion.identity);
//				micNote.pivot = UIWidget.Pivot.BottomLeft;
//				micNote.transform.parent = panel.transform;
//				micNote.transform.localRotation = Quaternion.identity;
//				micNote.transform.localPosition = new Vector3(posX, posY, 0);
//				micNote.depth = ConstantVariable.MicNoteDepth;
//				micNotesCollection.Add(micNote);
				
				
				micNoteStart = percent;
				float posX = pivotX + percent * backgroundWidth;
				float posY = NotePositionPanel(micKey); //pivotY + (micKey % 12) * noteSize;
				micNoteBar = GameObject.Instantiate(micNoteBarPrefabs, Vector3.zero, Quaternion.identity) as tk2dSlicedSprite;
				micNoteBar.transform.parent = noteBarParent.transform;
				micNoteBar.transform.localPosition = new Vector3(posX, posY, 0);
				micNotesCollection.Add(micNoteBar);
				
			} else {
				
			}
			
			keyPrevious = micKey;
			float length = ( percent - micNoteStart ) * backgroundWidth;
			//micNote.transform.localScale = new Vector3(length , noteHeight, 1);
			micNoteBar.dimensions = new Vector2(length, noteHeight);
		}
	}
	
	
	float NotePosition (float key)
	{
		float pos = key % 12;
		pos = (Screen.height / 24 * pos);		
		return pos;
	}
	
	float NotePositionPanel(float key) {
		float pos = key % 12;
		pos =  pivotY + backgroundHeight / 12 * pos;		
		return pos;	
	}
	
	

}
