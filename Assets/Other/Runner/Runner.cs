﻿using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {
	
	public static float distanceTraveled;
	public float acceleration;
	public Vector3 boostVelocity, jumpVelocity;
	public float gameOverY;
	
	
	private bool touchingPlatform;
	private Vector3 startPosition;
	
	private static int boosts;
	
	void Start () {
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = transform.localPosition;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetButtonDown( "Jump" ) ) {
			if ( touchingPlatform ) {
				rigidbody.AddForce( jumpVelocity, ForceMode.VelocityChange );	
				touchingPlatform = false;
			} else if ( boosts > 0 ) {
				rigidbody.AddForce ( boostVelocity, ForceMode.VelocityChange );
				boosts -= 1;
				GUIManager.SetBoosts(boosts);
			}
		}
		
		distanceTraveled = transform.localPosition.x;
		GUIManager.SetDistance(distanceTraveled);
		
		if ( transform.localPosition.y < gameOverY ) {
			GameEventManager.TriggerGameOver();	
		}
		
	}
	
	void FixedUpdate () {
		if ( touchingPlatform ) {
			rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
		}
	}
	
	void OnCollisionEnter () {
		touchingPlatform = true;	
	}
	
	void OnCollisionExit () {
		touchingPlatform = false;	
	}
	
	private void GameStart () {
		boosts = 0;
		GUIManager.SetBoosts(boosts);
		distanceTraveled = 0;
		GUIManager.SetDistance(distanceTraveled);
		transform.localPosition = startPosition;
		rigidbody.isKinematic = false;
		gameObject.SetActive(true);
		enabled = true;
	}
	
	private void GameOver () {
		rigidbody.isKinematic = true;
		enabled = false;
	}
	
	public static void AddBoost () {
		boosts += 1;	
		GUIManager.SetBoosts(boosts);
	}
	
}
