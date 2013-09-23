using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SongSelection : MonoBehaviour
{

		public GUIText guiSongSelected;
		public List<SongContainer> listSong;
		public float startYPos = 200;
		public float minHeight = 250;
		public AudioSource music;
		public ObjectSend dataSend;
		private GUIStyle guiStyle = new GUIStyle ();
		private LoadXmlData data;
		private List<GUIText> texts;
		private List<Texture> listImage;
		private List<AudioClip> listClip;
		private SongContainer songSelected;
		private float hScrollbarValue = 0;

		void Awake ()
		{
				//DontDestroyOnLoad (this);
				data = GameObject.Find ("xml_manager").GetComponent<LoadXmlData> ();
				listSong = new List<SongContainer> ();
				listImage = new List<Texture> ();
				listClip = new List<AudioClip> ();
				texts = new List<GUIText> ();
		}

		void Start ()
		{
				songSelected = null;
				List<Dictionary<string,string>> rawData = data.retrieveData ();
				foreach (Dictionary<string,string> obj in rawData) {
						SongContainer song = ConvertToSongContainer (obj); 
						listSong.Add (song);
						string targetFile = "file://" + Application.dataPath + "/Resources/Images/Cover Album/" + song.coverAlbum;
						Texture texture = LoadImage (targetFile);
						if (!texture) {
								targetFile = "file://" + Application.dataPath + "/Resources/Images/Cover Album/default.jpg";
								texture = LoadImage (targetFile);
						}

						listImage.Add (texture);
						
						AudioClip clip = LoadMusic (song.file);
						listClip.Add (clip);
						
						Debug.Log ("ada !" + song.title);
						
				}
				//music.clip = (AudioClip)Resources.Load ("Musics/002_big bang_love song");
		}

		void Update()
		{
				if (songSelected != null) {
						guiSongSelected.text = "Song Selected: " + songSelected.singer + " - " + songSelected.title;
						
						if (!music.isPlaying) {
								music.Play ();
						}
				}
		}

		void OnGUI ()
		{
				float width = 500;
				float height = 500;
				hScrollbarValue = GUI.HorizontalScrollbar (new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height), hScrollbarValue, 1.0f, 0.0f, 10.0f);
				float pos = startYPos;
				for (int i = 0; i < listSong.Count; i++) {
						CreateSongGUI (listSong [i], listImage [i], listClip[i], pos);
						pos += minHeight + 10;
				}
				
				width = 150;
				height = 150;
				if (GUI.Button (new Rect( Screen.width - width, Screen.height - height, width, height ), "SING")) 
				{
						
						Application.LoadLevel ("Play");
				}
						
				

		}

		public void CreateSongGUI (SongContainer song, Texture image, AudioClip clip, float pos)
		{
				
				float height = minHeight;
				float width = 0.4f * Screen.width;
				float xPos = (Screen.width - width) / 2;
				float yPos = pos;
				int imgWidth = 125;
				int imgHeight = 125;
//				Texture2D image = new Texture2D (imgWidth, imgHeight);
//				string fileName = "Assets/Images/Cover Album/" + song.coverAlbum;
//				if (File.Exists (fileName)) {
//						byte[] imageData = File.ReadAllBytes (fileName);
//						image.LoadImage (imageData);
//						//Debug.Log ("Image loaded!");
//				} else {
//						fileName = "Assets/Images/Cover Album/default.jpg";
//						//Debug.Log ("No Image exist!");
//				}
				
				Rect rect = new Rect (xPos, yPos, width, height);
				GUI.BeginGroup (rect);
				GUI.Box (new Rect(0, 0, width, height), song.title);
				GUI.Box(new Rect(25, 25, imgWidth, imgHeight), image);
				GUI.Label (new Rect (170, 50, 300, 20), ("Title: " + song.singer + " - " + song.title) );
				GUI.Label (new Rect (170, 75, 300, 20), ("Categories:" + song.category ) );
				GUI.Label (new Rect (170, 100, 300, 20), ("Duration:" + song.duration));
				GUI.EndGroup ();
				
				if (rect.Contains (Event.current.mousePosition)) 
				{
								
						if (Input.GetMouseButtonUp (0)) {
								dataSend.song = songSelected = song;
								dataSend.clip = music.clip = clip;
								
								Debug.Log ("Clicked: " + song.title);
						}
				}
				
		}

		public Texture LoadImage(string targetFile)
		{
				Texture2D image = null;
				WWW www = null;

				//Debug.Log("Beginning load at time: " + Time.time);
				www = new WWW(targetFile);
				
				//Debug.Log("File has finished being loaded at " + Time.time);
				image = new Texture2D(64, 64);
				//Debug.Log("Preparing to load PNG into Texture");
				www.LoadImageIntoTexture(image);

				
				//Debug.Log("Loaded image into texture");
				return image;
		}

		public AudioClip LoadMusic(string targetFile)
		{
				AudioClip clip;
				clip = (AudioClip)Resources.Load ("Musics/" + targetFile);
				return clip;
		}

		public SongContainer ConvertToSongContainer (Dictionary<string,string> obj)
		{
				SongContainer songContainer = new SongContainer ();
				obj.TryGetValue ("trackno", out songContainer.trackNo);
				obj.TryGetValue ("title", out songContainer.title);
				obj.TryGetValue ("singer", out songContainer.singer);
				obj.TryGetValue ("category", out songContainer.category);
				obj.TryGetValue ("duration", out songContainer.duration);
				obj.TryGetValue ("file", out songContainer.file);
				obj.TryGetValue ("lyrics", out songContainer.lyrics);
				obj.TryGetValue ("coveralbum", out songContainer.coverAlbum);
				return songContainer;
				
		}
}
