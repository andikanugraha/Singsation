using UnityEngine;
using System.Collections;

[RequireComponent (typeof (exSprite))]
public class MovingBackground : MonoBehaviour {

	public float speed = 30;
	public float leftScreenOffset = -240;
	
	private BackgroundManager backgroundManager;
	private exSprite sprite;
	
	void Awake() {
		backgroundManager = GameObject.Find(ConstantVariable.BackgroundManager).GetComponent<BackgroundManager>();
		sprite = this.GetComponent<exSprite>();
		leftScreenOffset = -(Screen.width / 2);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GamePlayManager.state == GamePlayManager.State.Play) {
			//jalankan background ke kiri
			Vector3 currentPos = transform.localPosition;
			currentPos.x -= speed * Time.deltaTime;
			transform.localPosition = currentPos;
			
			//hapus apabila hilang dari layar
			float posX = currentPos.x + sprite.width;
			if(posX < leftScreenOffset) {
				backgroundManager.backgroundCount--;
				Destroy(this.gameObject);	
			}
		}
		
		
	}
}
