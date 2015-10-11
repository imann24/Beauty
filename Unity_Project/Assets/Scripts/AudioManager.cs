using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioSource Music;
	public AudioSource SFX;

	public AudioClip IdMusic;

	public AudioClip ObjectInteraction;
	public AudioClip BugsCrawling;

	public AudioManager Instance; 

	private bool suppressOnLevelLoad;

	const string DAD_CRYING_KEY = "DadCrying";

	// Use this for initialization
	void Awake () {
		// Singleton Implementation 
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			suppressOnLevelLoad = true;
			Destroy(gameObject);
		}
	}
	
	void Start () {
		SubscribeEvents();
		SetMusic(Global.Scenes.Id);
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	void OnLevelWasLoaded (int level) {
		if (suppressOnLevelLoad) {
			return;
		}

		SetMusic((Global.Scenes) level);
	}

	void SetMusic (Global.Scenes currentScene) {
		if (currentScene == Global.Scenes.Id) {
			Music.clip = IdMusic;
			Music.Play();
		}
	}

	void PlayObjectInteractionSFX () {
		SFX.clip = ObjectInteraction;
		SFX.Play();
	}

	void PlayBugSFX () {
		SFX.clip = BugsCrawling;
		SFX.Play();
	}

	void SubscribeEvents () {
		PlayAnimationOnCollide.OnAnimationPlay += HandleAnimationTriggered;
		MessageComponent.OnMessageRead += HandleItemInteraction;
		TeleportArea.OnTeleportToRoom += HandleRoomChange;
	}

	void UnsubscribeEvents () {
		PlayAnimationOnCollide.OnAnimationPlay -= HandleAnimationTriggered;
		MessageComponent.OnMessageRead -= HandleItemInteraction;
		TeleportArea.OnTeleportToRoom -= HandleRoomChange;
	}

	void HandleAnimationTriggered (string animationTrigger) {
		if (IsBugAnimation(animationTrigger)) {
			PlayBugSFX();
		}
	}

	void HandleItemInteraction (bool interactionDone) {
		if (!interactionDone) {
			PlayObjectInteractionSFX();
		}
	}

	void HandleRoomChange (Global.Rooms NewRoom) {
		ProximityAudioController.Controllers[DAD_CRYING_KEY].ToggleAudio(NewRoom == Global.Rooms.HallwayLeft || 
		                                                                 NewRoom == Global.Rooms.HallwayRight);
	}

	bool IsBugAnimation (string animationTrigger) {
		return animationTrigger.Contains("Bug");
	}
}
