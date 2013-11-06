using UnityEngine;
using System.Collections;

public class GamePlayManager : MonoBehaviour
{
	
	public enum State
	{
		Play,
		Pause,
	}

	public static State state;
	public tk2dUIItem pauseButton;
	public AudioSource song;
	public GUIText timeLapse;
	public PauseMenu pauseMenu;
	public MicrophoneInput microphoneInput;
	public tk2dTextMesh songTitle;
	public tk2dTextMesh scoreText;
	public GUIText scoreTextDebug;
	public tk2dUIProgressBar songProgressBar;
	
	private float score;
	private float matchKey;
	private ObjectSend dataSend;
	private NoteFrequencyManager noteFrequencyManager;
	private NotesManager notesManager;
	private GlobalSetting globalSetting;
	private tk2dSprite pauseButtonSprite;
	
	void Awake ()
	{
		noteFrequencyManager = GameObject.Find (ConstantVariable.MicrophoneManager).GetComponent<NoteFrequencyManager> ();
		notesManager = GameObject.Find (ConstantVariable.NotesManager).GetComponent<NotesManager> ();
		globalSetting = GameObject.Find(ConstantVariable.GlobalSetting).GetComponent<GlobalSetting> ();
		pauseButtonSprite = pauseButton.GetComponentInChildren<tk2dSprite>();
		
		if(!globalSetting.debugMode) {
			dataSend = GameObject.Find (ConstantVariable.DataSend).GetComponent<ObjectSend> ();
		}
		
	}
	
	void Start ()
	{
		if (dataSend != null) {
			song.clip = dataSend.clip;
			songTitle.text = dataSend.song.singer + "\n" + dataSend.song.title;
			Destroy (dataSend);
		} else {
			songTitle.text = "Artist\nUnknown";
		}
		
		state = State.Play;
		score = 0;
		matchKey = 0;	
		
		//Add event
		pauseButton.OnClick += PauseButtonClick;
	}
	
	void Update ()
	{
		//Debug.Log("Current state: " + state);
				
		if (state == State.Play) {
			PlayState();
			
		} else if (state == State.Pause) {
			PauseState();
		}
		
	}
	
	private void PauseButtonClick() {
		if (state == State.Play) {
			state = State.Pause;
		} else if (state == State.Pause) {
			state = State.Play;
		}
	}

	private string ConvertToTime (float time)
	{
		string display = "";
		float min = Mathf.Floor ((time / 60f));
		float sec = (time % 60f);
		float fraction = ((time * 100) % 100);
		display = string.Format ("{0:00}:{1:00}:{2:00}", min, sec, fraction);	
		return display;
	}
	
	//State Play, ketika sedang bermain
	private void PlayState ()
	{
		if (!song.isPlaying) {
			song.Play ();	
		}
		
		if (pauseMenu.enabled) {
			pauseMenu.enabled = false;	
		}
		
		if (!microphoneInput.enabled) {
			microphoneInput.enabled = true;
		}
		
		if(!pauseButtonSprite.CurrentSprite.name.Equals(ConstantVariable.PauseIcon)) {
			pauseButtonSprite.SetSprite(ConstantVariable.PauseIcon);
		}
		
		float key = Mathf.Round(noteFrequencyManager.key);
		if(notesManager.notesContainer.ContainsKey(notesManager.position)){
			matchKey = notesManager.notesContainer[notesManager.position];
		}
		float modKey = key % 12;
		float modMatchKey = matchKey % 12;
		if( key != 0 && modKey == modMatchKey ) {
			score += Mathf.Round(Time.deltaTime * 100);	
			
		}
		
		scoreTextDebug.text = "Score: " + score + " \n key: " + key + " (" + modKey + ")\n matchkey: " + matchKey + " (" + modMatchKey + ")";
		scoreText.text = "Score: " + score;
		timeLapse.text = ConvertToTime (song.time) + " / " + ConvertToTime (song.clip.length);
		
		//Song progress bar
		float songPercent = song.time / song.clip.length;
		songProgressBar.Value = songPercent;
	}
	
	//State Pause, ketika sedang berhenti
	private void PauseState ()
	{
		if (song.isPlaying) {
			song.Pause ();	
		}
		
		if (!pauseMenu.enabled) {
			pauseMenu.enabled = true;	
		}
		
		if (microphoneInput.enabled) {
			microphoneInput.enabled = false;
		}
		
		if(!pauseButtonSprite.CurrentSprite.name.Equals(ConstantVariable.PlayIcon)) {
			pauseButtonSprite.SetSprite(ConstantVariable.PlayIcon);
		}
	}
}
