using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {
	
	public float pivotY = 60;
	public GameObject parentBackground;
	public GameObject backgroundPrefabs;
	
	public int backgroundCount;
	
	private List<GameObject> backgrounds;
	
	void Awake () {
		backgrounds = new List<GameObject>();
		backgroundCount = 0;
	}
	
	// Use this for initialization
	void Start () {
		backgroundCount = GameObject.FindGameObjectsWithTag(ConstantVariable.TagBackground).Length;
		
		while(backgroundCount < 3) {
			AddBackground();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(backgroundCount < 3) {
			AddBackground();
		}
		
	}
	
	void AddBackground(){
		GameObject bg = GameObject.Instantiate(backgroundPrefabs, Vector3.zero, Quaternion.identity) as GameObject;
		
		tk2dSprite sprite = bg.GetComponent<tk2dSprite>();
		
		//exSprite bg = GameObject.Instantiate(backgroundPrefabs, Vector3.zero, Quaternion.identity) as exSprite;
		bg.transform.parent = parentBackground.transform;
		bg.transform.localPosition = new Vector3( sprite.scale.x * backgroundCount , pivotY, 0);
		backgrounds.Add(bg);
		backgroundCount++;	
	}
	
}
