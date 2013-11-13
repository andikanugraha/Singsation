using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dUIItem))]
public class SongItem : MonoBehaviour {
	
	public SongContainer songContainer;
	public AudioClip audioClip;
	public Texture coverImage;
	public tk2dSpriteFromTexture spriteFromTexture;
	public GameObject pivotPlayButton;
	private tk2dSprite sprite;
	private tk2dUIItem uiItem;
	private SongSelection songSelection;
	
	void Awake () {
		
		uiItem = GetComponent<tk2dUIItem>();
		spriteFromTexture = GetComponentInChildren<tk2dSpriteFromTexture>();
		sprite = GetComponentInChildren<tk2dSprite>();
		songSelection = GameObject.FindGameObjectWithTag (ConstantVariable.TagSongSelection).GetComponent<SongSelection> ();
	}
	
	// Use this for initialization
	void Start () {
		uiItem.OnClick += ButtonClick;
		spriteFromTexture.texture = coverImage;
		spriteFromTexture.spriteCollectionSize.type = tk2dSpriteCollectionSize.Type.Explicit;
		float x = spriteFromTexture.texture.width;
		float y = spriteFromTexture.texture.height;
		float targetHeight = spriteFromTexture.spriteCollectionSize.height = GlobalSetting.minResolutionY;
		float orthoSize = x / ((( x / y ) * 2 ) * targetHeight );
		spriteFromTexture.spriteCollectionSize.orthoSize = orthoSize;
		Debug.Log("Ortho size: " + orthoSize);
		sprite.GetCurrentSpriteDef().material.mainTexture = coverImage;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void ButtonClick() {
		songSelection.SelectMusic(songContainer, audioClip, this.gameObject);
		Debug.Log("Clicked! " + songContainer.title);	
	}
}
