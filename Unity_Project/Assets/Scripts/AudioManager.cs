using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioSource Music;
	public AudioSource SFX;

	public AudioClip IdMusic;

	public AudioManager Instance; 
	// Use this for initialization
	void Awake () {
		// Singleton Implementation 
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		SetMusic(Global.Scenes.Id);
	}

	void OnLevelLoad (int level) {
		SetMusic((Global.Scenes) level);
	}

	void SetMusic (Global.Scenes currentScene) {
		if (currentScene == Global.Scenes.Id) {
			Music.clip = IdMusic;
			Music.Play();
		}
	}
}
