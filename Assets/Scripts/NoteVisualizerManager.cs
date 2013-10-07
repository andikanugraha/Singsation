using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class NoteVisualizerManager : MonoBehaviour
{

	public float noteHeight = 20;
	public float noteWidth = 10;
	public float panelWidth = 800;
	public float panelHeight = 400;
	public float pivotX = -400;
	public float pivotY = -400;
	public Texture texture;
	public GUITexture noteIndicator;
	public UISlider progressNotes;
	public UISprite noteIndicatorNGUI;
	public UISprite notePrefabs;
	public UISprite micNotePrefabs;
	public UILabel lyricsPrefabs;
	
	private UIPanel panel;
	private NotesManager notesManager;
	private NoteFrequencyManager noteFrequencyManager;
	private bool newLine;
	private int currentLine;
	private float keyPrevious;
	private float micNoteStart;
	private UISprite micNote;
	private List<UISprite> notesCollection;
	private List<UISprite> micNotesCollection;
	private List<UILabel> lyricsCollection;
	
	void Awake() {
		panel = GameObject.FindGameObjectWithTag(ConstantVariable.TagPanel).GetComponent<UIPanel>();
		notesManager = GameObject.Find(ConstantVariable.NotesManager).GetComponent<NotesManager>();
		noteFrequencyManager = GameObject.Find (ConstantVariable.MicrophoneManager).GetComponent<NoteFrequencyManager>();
		
		notesCollection = new List<UISprite>();
		micNotesCollection = new List<UISprite>();
		lyricsCollection = new List<UILabel>();
		newLine = true;
		currentLine = 0;
		keyPrevious = 0;
	}
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		//float x = noteIndicator.guiTexture.pixelInset.x;
		float y = NotePosition (noteFrequencyManager.key);
		float x = 100;			

		float width = noteIndicator.pixelInset.width;
		float height = noteIndicator.pixelInset.height;
		noteIndicator.pixelInset = new Rect (x, y, width, height);
		
		
		//NGUI 
		
		float percent = 0;
		if(notesManager.currentLine <= 0) {
			percent = notesManager.position / notesManager.lineBreakContainer[notesManager.currentLine];	
		} else {
			float before = notesManager.lineBreakContainer[notesManager.currentLine - 1];
			percent = (notesManager.position - before  ) / (notesManager.lineBreakContainer[notesManager.currentLine] - before);		
		}
		
		//Note visualizer
		progressNotes.sliderValue = percent;
		if(newLine) {
			foreach(UISprite sprite in notesCollection) {
				Destroy(sprite.gameObject);	
			}
			
			foreach(UISprite sprite in micNotesCollection) {
				Destroy(sprite.gameObject);	
			}
			
			foreach(UILabel label in lyricsCollection) {
				Destroy(label.gameObject);	
			}
			
			notesCollection = new List<UISprite>();
			micNotesCollection = new List<UISprite>();
			lyricsCollection = new List<UILabel>();
			
			float linePos = 0;
			if(notesManager.currentLine > 0) {
				linePos = notesManager.lineBreakContainer[notesManager.currentLine - 1];
			}
			for ( float i = linePos; i < notesManager.lineBreakContainer[notesManager.currentLine]; i++) {
				if(notesManager.notesContainer.ContainsKey(i)){
					float key = notesManager.notesContainer[i];
					float distance = notesManager.timeContainer[i];
					string lyrics = notesManager.lyricsContainer[i];
					
					float posX = pivotX + (i - linePos) * noteWidth;
					float posY = NotePositionPanel(key);
					
					UISprite note = (UISprite) Instantiate(notePrefabs, Vector3.zero, Quaternion.identity);
					note.pivot = UIWidget.Pivot.BottomLeft;
					note.transform.parent = panel.transform;
					note.transform.localRotation = Quaternion.identity;
					note.transform.localPosition = new Vector3(posX, posY, 0);
					note.transform.localScale = new Vector3(distance * noteWidth, noteHeight, 1);
					note.depth = ConstantVariable.NoteDepth;
					notesCollection.Add(note);
					
					//Add lyrics
					UILabel labels = (UILabel) Instantiate(lyricsPrefabs, Vector3.zero, Quaternion.identity);
					labels.pivot = UIWidget.Pivot.Center;
					labels.transform.parent = note.transform;
					labels.transform.localRotation = Quaternion.identity;
					
					labels.transform.localPosition = new Vector3( 0.5f , 0.5f, 0);
					labels.transform.localScale = new Vector3(0.2f, 0.6f, 0.5f);
					labels.text = lyrics;
					labels.depth = ConstantVariable.LyricsDepth;
					lyricsCollection.Add(labels);
				} else {
					
				}
				
			}
			newLine = false;
		}
		
		if(currentLine != notesManager.currentLine) {
			newLine = true;
			currentLine = notesManager.currentLine;
		}
		
		float micKey = Mathf.Round(noteFrequencyManager.key);
		if(micKey > 0) {
			if(keyPrevious != micKey) {
				micNote = (UISprite) Instantiate(micNotePrefabs, Vector3.zero, Quaternion.identity);
				micNote.pivot = UIWidget.Pivot.BottomLeft;
				micNote.transform.parent = panel.transform;
				micNote.transform.localRotation = Quaternion.identity;
				
				micNoteStart = percent;
				float posX = pivotX + percent * panelWidth;
				float posY = NotePositionPanel(micKey); //pivotY + (micKey % 12) * noteSize;
				micNote.transform.localPosition = new Vector3(posX, posY, 0);
				micNote.depth = ConstantVariable.MicNoteDepth;
				micNotesCollection.Add(micNote);
			} else {
				
			}
			
			keyPrevious = micKey;
			float length = ( percent - micNoteStart ) * panelWidth;
			micNote.transform.localScale = new Vector3(length , noteHeight, 1);
			
		}
		
		//Note indicator
		y = NotePositionPanel(noteFrequencyManager.key);
		noteIndicatorNGUI.transform.localPosition = new Vector3 (pivotX + percent * panelWidth, y, 0);
		
	}

	void OnGUI ()
	{

	}

	float NotePosition (float key)
	{
		float pos = key % 12;
		pos = (Screen.height / 24 * pos);		
		return pos;
	}
	
	float NotePositionPanel(float key) {
		float pos = key % 12;
		pos =  pivotY + panelHeight / 12 * pos;		
		return pos;	
	}
	
	void MicrophoneVisualizer (float key, float percent) {
		float micKey = Mathf.Round(key);
		if(micKey > 0) {
			if(keyPrevious != micKey) {
				micNote = (UISprite) Instantiate(micNotePrefabs, Vector3.zero, Quaternion.identity);
				micNote.pivot = UIWidget.Pivot.BottomLeft;
				micNote.transform.parent = panel.transform;
				micNote.transform.localRotation = Quaternion.identity;
				
				micNoteStart = percent;
				float posX = pivotX + percent * panelWidth;
				float posY = NotePositionPanel(micKey); //pivotY + (micKey % 12) * noteSize;
				micNote.transform.localPosition = new Vector3(posX, posY, 0);
				micNote.depth = ConstantVariable.MicNoteDepth;
				micNotesCollection.Add(micNote);
			} else {
				
			}
			
			keyPrevious = micKey;
			float length = ( percent - micNoteStart ) * panelWidth;
			micNote.transform.localScale = new Vector3(length , noteHeight, 1);
			
		}
	}

}
