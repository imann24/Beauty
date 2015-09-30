using UnityEngine;
using System.Collections;

public class TeleportArea : MonoBehaviour {
	public Global.Scenes TargetLevel;
	public delegate void TeleportToLevelAction (Global.Scenes sceneToLoad);
	public static event TeleportToLevelAction OnTeleportToLevel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (OnTeleportToLevel != null) {
			OnTeleportToLevel(TargetLevel);
		}
	}
}
