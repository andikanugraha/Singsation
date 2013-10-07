using UnityEngine;
using System.Collections;

public class NoteFrequencyManager : MonoBehaviour {

	const float A440 = 440; // Hz

	//Note start at C0
	const float startKey = 58;
	
	public float frequency;
	public GUIText testing;

	public SpectrumManager spectrum;

	string[] chromaticSet = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
	public float key;
	public string chromatic;

	void Awake(){
				
	}
	
	// Update is called once per frame
	void Update () {
				if (spectrum) {
						frequency = spectrum.pitchValue;
				}
				if (frequency > 0) {
						key = FrequencyToNote (frequency);
						chromatic = NoteToChromatic (key);
				} else {
						key = 0;
						chromatic = "-";
				}
				testing.text = "Frequency of " + frequency + "\nNote: " + key + "\nchromatic: " + chromatic;
	}

	float FrequencyToNote(float freq) {
				float n = 12 * Mathf.Log ((freq / A440), 2) + startKey;
				//Debug.Log ("Frequency of " + freq + " is: " + n);
				return n;
	}

	public static string NoteToChromatic(float n) {
				string[] chromaticSet = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
				n = Mathf.Round (n);
				string chromatic = "";
				float octave = Mathf.Floor( n / chromaticSet.Length );
				float pos = (n - 1) % chromaticSet.Length;
				chromatic = chromaticSet [(int)pos] + octave;
				return chromatic;
	}
	
}
