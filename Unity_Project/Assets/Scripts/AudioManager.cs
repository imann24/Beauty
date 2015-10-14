using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioSource Music;
	public AudioSource SFX;
	public AudioSource WalkingSFX;

	public AudioClip IdMusic;

	public AudioClip ObjectInteraction;
	public AudioClip BugsCrawling;
	public AudioClip CreekingLoop;

	public AudioManager Instance; 

	private bool suppressOnLevelLoad;

	const string DAD_CRYING_KEY = "DadCrying";

	PlayerController.State currentPlayerMovement = PlayerController.State.Stoppped;
	Global.Rooms currentRoom = Global.Rooms.BedroomLeft;

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
		PlayerController.OnWalkChange += HandleOnWalkChange;
	}

	void UnsubscribeEvents () {
		PlayAnimationOnCollide.OnAnimationPlay -= HandleAnimationTriggered;
		MessageComponent.OnMessageRead -= HandleItemInteraction;
		TeleportArea.OnTeleportToRoom -= HandleRoomChange;
		PlayerController.OnWalkChange += HandleOnWalkChange;
	}

	void HandleOnWalkChange (PlayerController.State newState) {
		if (!RoomWithWalkingSFX(currentRoom)) {
			return;
		}

		if (currentPlayerMovement == PlayerController.State.Stoppped &&
		    currentPlayerMovement != newState) {
			WalkingSFX.mute = false;
		} else if (currentPlayerMovement != PlayerController.State.Stoppped &&
		           newState == PlayerController.State.Stoppped) {
			WalkingSFX.mute = true;
		}

		currentPlayerMovement = newState;
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

		WalkingSFX.mute = !(RoomWithWalkingSFX(NewRoom));
		currentRoom = NewRoom;
	}

	bool IsBugAnimation (string animationTrigger) {
		return animationTrigger.Contains("Bug");
	}	

	bool RoomWithWalkingSFX (Global.Rooms roomToCheck) {
		return roomToCheck == Global.Rooms.BedroomLeft ||
				roomToCheck ==	Global.Rooms.BedroomRight ||
				roomToCheck == Global.Rooms.HallwayLeft ||
				roomToCheck == Global.Rooms.HallwayRight;
	}
}
