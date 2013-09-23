using UnityEngine;
using System.Collections;

public class ObjectSend : MonoBehaviour {

	public SongContainer song;
	public AudioClip clip;

	void Awake()
	{
				DontDestroyOnLoad (this);
	}

}
