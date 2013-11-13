using UnityEngine;
using System.Collections;

[RequireComponent (typeof (tk2dSprite))]
public class BackgroundMove : MonoBehaviour {

	public float speed = 30;
	public float leftScreenOffset = -300;
	
	private GameObject parent;
	private tk2dSprite sprite;
	private BackgroundManager backgroundManager;
	
	void Awake() {
		backgroundManager = GameObject.Find(ConstantVariable.BackgroundManager).GetComponent<BackgroundManager>();
		sprite = this.GetComponent<tk2dSprite>();
		leftScreenOffset = -(Screen.width / 2);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Game sedang berjalan
		if(GamePlayManager.state == GamePlayManager.State.Play) {
			//jalankan background ke kiri
			Vector3 currentPos = transform.localPosition;
			currentPos.x -= speed * Time.deltaTime;
			transform.localPosition = currentPos;
			
			//hapus apabila hilang dari layar
			float posX = currentPos.x + sprite.scale.x;
			if(posX < leftScreenOffset) {
				backgroundManager.backgroundCount--;
				Destroy(this.gameObject);	
			}
		}
	}
}
