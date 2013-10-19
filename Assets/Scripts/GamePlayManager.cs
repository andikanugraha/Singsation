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
	public GUITexture pauseButton;
	public Texture pauseIcon;
	public Texture playIcon;
	public AudioSource song;
	public GUIText timeLapse;
	public PauseMenu pauseMenu;
	public MicrophoneInput microphoneInput;
	public GUIText songTitle;
	public GUIText scoreText;
	public UILabel scoreLabel;
	public UISlider songProgressBar;
	
	private float score;
	private float matchKey;
	private ObjectSend dataSend;
	private NoteFrequencyManager noteFrequencyManager;
	private NotesManager notesManager;
	private GlobalSetting globalSetting;
	
	
	void Awake ()
	{
		noteFrequencyManager = GameObject.Find (ConstantVariable.MicrophoneManager).GetComponent<NoteFrequencyManager> ();
		notesManager = GameObject.Find (ConstantVariable.NotesManager).GetComponent<NotesManager> ();
		globalSetting = GameObject.Find(ConstantVariable.GlobalSetting).GetComponent<GlobalSetting> ();
		
		if(!globalSetting.debugMode) {
			dataSend = GameObject.Find (ConstantVariable.DataSend).GetComponent<ObjectSend> ();
		}
		
	}
	// Use this for initialization
	void Start ()
	{
		if (dataSend != null) {
			song.clip = dataSend.clip;
			songTitle.text = dataSend.song.singer + " - " + dataSend.song.title;
			Destroy (dataSend);
		}
		state = State.Play;
		score = 0;
		matchKey = 0;	
	}
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("Current state: " + state);
				
		if (state == State.Play) {
			PlayState();
			
		} else if (state == State.Pause) {
			PauseState();
		}
		
		if (Input.GetMouseButtonUp (0)) {
			bool hit = pauseButton.HitTest (Input.mousePosition);
			if (hit) {
				if (state == State.Play) {
					state = State.Pause;
					
				} else if (state == State.Pause) {
					state = State.Play;		
					
				}
			}
		}
	}

	void OnGUI ()
	{
		if (state == State.Pause) {
			
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
		
		if (pauseButton.texture != playIcon) {
			pauseButton.texture = playIcon;	
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
		
		scoreText.text = "Score: " + score + " \n key: " + key + " (" + modKey + ")\n matchkey: " + matchKey + " (" + modMatchKey + ")";
		scoreLabel.text = "Score: " + score;
		timeLapse.text = ConvertToTime (song.time) + " / " + ConvertToTime (song.clip.length);
		
		//Song progress bar
		float songPercent = song.time / song.clip.length;
		songProgressBar.sliderValue = songPercent;
	}
	
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
		
		if (pauseButton.texture != pauseIcon) {
			pauseButton.texture = pauseIcon;	
		}
	}
}
