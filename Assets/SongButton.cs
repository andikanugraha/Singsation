using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dUIItem))]
public class SongButton : MonoBehaviour {
	
	public SongContainer songContainer;
	public AudioClip audioClip;
	public Texture coverImage;
	public tk2dSpriteFromTexture tk2dTexture;
	public GameObject pivotPlayButton;
	private tk2dSprite sprite;
	private tk2dUIItem uiItem;
	private SongSelection songSelection;
	
	void Awake () {
		uiItem = GetComponent<tk2dUIItem>();
		tk2dTexture = GetComponentInChildren<tk2dSpriteFromTexture>();
		sprite = GetComponentInChildren<tk2dSprite>();
		songSelection = GameObject.FindGameObjectWithTag (ConstantVariable.TagSongSelection).GetComponent<SongSelection> ();
	}
	
	// Use this for initialization
	void Start () {
		uiItem.OnClick += ButtonClick;
		tk2dTexture.texture = coverImage;
		sprite.GetCurrentSpriteDef().material.mainTexture = coverImage;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void ButtonClick() {
		songSelection.SelectMusic(songContainer, audioClip, pivotPlayButton);
		Debug.Log("Clicked! " + songContainer.title);	
	}
}
