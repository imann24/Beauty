using UnityEngine;
using System.Collections;

public class SpawnArea : MonoBehaviour {
	public Global.Rooms Room;

	// Use this for initialization
	void Start () {
		SubscribeEvents();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void SubscribeEvents () {
		TeleportArea.OnTeleportToRoom += SpawnPlayer;
	}

	void UnsubscribeEvents () {
		TeleportArea.OnTeleportToRoom -= SpawnPlayer;
	}

	void SpawnPlayer (Global.Rooms TargetRoom) {
		if (TargetRoom == Room) {
			Invoke("SpawnPlayer", 1.0f);
		}
	}

	void SpawnPlayer () {
		TeleportArea.Teleporting = false;
		Global.Player.transform.position = transform.position;
	}
}
