using UnityEngine;
using System.Collections;

public class SafetyNet : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collider) {
		Utility.TeleportPlayerToStart();
	}

	void OnCollisionEnter2D (Collision2D collision) {
		Utility.TeleportPlayerToStart();
	}
}
