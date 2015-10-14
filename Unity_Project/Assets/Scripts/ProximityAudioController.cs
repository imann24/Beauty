using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class ProximityAudioController : MonoBehaviour {
	public static Dictionary<string, ProximityAudioController> Controllers = new Dictionary<string, ProximityAudioController>();
	public Transform ProximityObject;
	public float SizeOfSpace;
	public float MaxVolume;

	AudioSource audioSource;
	bool audioActive;


	// Use this for initialization
	void Start () {
		Controllers.Add(gameObject.name, this);
		AssignReferences();
	}

	void OnDestroy () {
		Controllers.Remove(gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		if (audioActive) {
			audioSource.volume = Mathf.Clamp(1.0f - Mathf.Clamp01(Vector2.Distance(transform.position, ProximityObject.position)/SizeOfSpace), 0, MaxVolume);
		}
	}

	public void ToggleAudio (bool active) {
		audioActive = active;

		if (active) {
			audioSource.Play();
		} else {
			audioSource.Stop();
		}
	}

	void AssignReferences () {
		audioSource = GetComponent<AudioSource>();
		audioActive = audioSource.playOnAwake;
	}
}
