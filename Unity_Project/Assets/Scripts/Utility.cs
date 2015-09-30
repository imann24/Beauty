using UnityEngine;
using System.Collections;

public class Utility {

	public static void TeleportPlayerToStart () {
		Global.Player.transform.position = Global.StartPos;
	}
	
	public static Vector3 TranslatePosition (Vector3 currentPos, float xTranslate = 0, float yTranslate = 0, float zTranslate = 0) {
		currentPos.x += xTranslate;
		currentPos.y += yTranslate;
		currentPos.z += zTranslate;
		return currentPos;
	}
}
