using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour {
	
	public exLayer backgroundLayer;
	public exSprite backgroundPrefabs;
	
	private List<GameObject> backgrounds;
	
	// Use this for initialization
	void Start () {
		backgrounds = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if(backgrounds.Capacity < 3) {
			GameObject bg = GameObject.Instantiate(backgroundPrefabs, Vector3.zero, Quaternion.identity) as GameObject;
			//bg.transform.parent = backgroundLayer.transform;
			backgrounds.Add(bg);
			backgroundLayer.Add(bg);
		}
		
	}
}
