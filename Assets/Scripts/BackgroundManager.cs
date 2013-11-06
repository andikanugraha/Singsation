using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {
	
	public float pivotX = -240;
	public float pivotY = 80;
	public tk2dSprite background;
	
	public exLayer backgroundLayer;
	public exSprite backgroundPrefabs;
	public int backgroundCount;
	
	private List<exSprite> backgrounds;
	
	
	void Awake () {
		backgrounds = new List<exSprite>();
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
		exSprite bg = GameObject.Instantiate(backgroundPrefabs, Vector3.zero, Quaternion.identity) as exSprite;
		bg.transform.parent = backgroundLayer.transform;
		bg.transform.localPosition = new Vector3( (bg.width) * backgroundCount , pivotY, 0);
		backgroundLayer.Add(bg);
		backgroundCount++;	
	}
	
}
