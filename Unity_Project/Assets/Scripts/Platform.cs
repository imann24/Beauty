using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	Vector2 leftMostPoint;
	Vector2 rightMostPoint;

	string leftPointKey = "Left";
	string rightPointKey = "Right";

	float offset = 2.0f;

	// Use this for initialization
	void Start () {
		SubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SubscribeEvents () {

		PlatformWrapAround [] playerCollideChecks = transform.GetComponentsInChildren<PlatformWrapAround>();
		for (int i = 0; i < playerCollideChecks.Length; i++) {
			playerCollideChecks[i].OnPlayerCollide += HandleOnPlayerCollide;
			AssignEndPoints(playerCollideChecks[i].gameObject);
		}
	}

	void HandleOnPlayerCollide (Global.Direction platformSide)
	{
		Vector2 newPlayerPos;
		if (platformSide == Global.Direction.Left) {
			newPlayerPos = rightMostPoint;
			newPlayerPos.y = Global.Player.transform.position.y;
			Global.Player.transform.position = newPlayerPos;
			Global.Player.transform.Translate(offset * Vector3.left);
		} else if (platformSide == Global.Direction.Right) {
			newPlayerPos = leftMostPoint;
			newPlayerPos.y = Global.Player.transform.position.y;
			Global.Player.transform.position = newPlayerPos;
			Global.Player.transform.Translate(offset * Vector3.right);
		}
	}

	void UnsubscribeEvents () {

	}

	void TeleportPlayer () {

	}

	void AssignEndPoints (GameObject point) {
		if (point.name.Contains(leftPointKey)) {
			leftMostPoint = point.transform.position;
		} else if (point.name.Contains(rightPointKey)) {
			rightMostPoint = point.transform.position;
		}
	}


}
