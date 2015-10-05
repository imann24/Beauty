using UnityEngine;
using System.Collections;

public class TeleportArea : MonoBehaviour {
	public static bool Teleporting;
	public Global.Rooms TargetRoom;
	public delegate void TeleportToRoomAction (Global.Rooms TargetRoom);
	public static event TeleportToRoomAction OnTeleportToRoom;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Debug.Log("Calling the event from " + gameObject.name);
		if (!Teleporting && OnTeleportToRoom != null) {
			OnTeleportToRoom(TargetRoom);
			Teleporting = true;
		}
	}
}
