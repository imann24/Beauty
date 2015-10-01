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

	public static string [] SplitString (string targetString, char targetChar) {
		string [] untrimmedArray = targetString.Split(targetChar);
		string [] finalArray;
		int finalArrayLength = untrimmedArray.Length;
		int indexInFinalArray = 0;
		for (int i = 0; i < untrimmedArray.Length; i++) {
			// NOTE: there is some difference based on text encoding between Mac and PC:
			// The "or" operator here is intended to account for either OS ind order to detect empty strings
			if (string.IsNullOrEmpty(untrimmedArray[i]) || (int)untrimmedArray[i][0] == 13) {
				finalArrayLength--;
			}
		}
		
		finalArray = new string[finalArrayLength];
		
		for (int i = 0; i < untrimmedArray.Length; i++) {
			//skips the empty strings
			if (string.IsNullOrEmpty(untrimmedArray[i]) || (int)untrimmedArray[i][0] == 13) {
				continue;
			}
			
			finalArray[indexInFinalArray++] = untrimmedArray[i];
		}
		
		return finalArray;
	}
}
