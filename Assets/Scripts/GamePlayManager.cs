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
		private ObjectSend dataSend;

		void Awake ()
		{
				//For debuging disable dataSend
				//dataSend = GameObject.Find ("data_send").GetComponent<ObjectSend> ();
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
				
		}
		// Update is called once per frame
		void Update ()
		{
				//Debug.Log("Current state: " + state);
		
				if (state == State.Play) {
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
			
						timeLapse.text = ConvertToTime (song.time) + " / " + ConvertToTime (song.clip.length);
			
				} else if (state == State.Pause) {
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
}
