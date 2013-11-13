using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Player : MonoBehaviour {
	
	public enum Gender {
		Male,
		Female
	};
	
	public enum Title {
		Beginner,
		Rookie,
		Intermediate,
		Professional,
		Superstar
	}
	
	public string username;
	public Gender gender;
	public int yearOfBirth;
	public Title title;
	
	public int level;
	public float exp;
	public float expNextLevel;
	public float totalExp;
	public int songPlayed;
	public float averageAccuracy;
	public SongContainer favoriteSong;
	
	public float coin;
	public float collectibleItem;
	
	public List<SongPlayed> songsPlayed;
	
	public int lessonProgress; 
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
