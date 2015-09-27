using UnityEngine;
using System.Collections;

public class Utility {

	public static void TeleportPlayerToStart () {
		Global.Player.transform.position = Global.StartPos;
	}
}
