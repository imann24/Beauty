using UnityEngine;
using System.Collections;

public class CharacterTracker : MonoBehaviour {
	public GameObject Player;

	float zOffset = -10f;
	float yOffset = 0.05f;
	// Use this for initialization
	void Start () {
		transform.position = Utility.TranslatePosition(transform.position, yTranslate:yOffset);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = FollowPlayer(Player.transform.position, true, false);
	}


	Vector3 FollowPlayer (Vector3 playerPos, bool trackHorizontalMovement = true, bool trackVerticalMovement = true) {
		return new Vector3(trackHorizontalMovement?playerPos.x:transform.position.x, 
		                   trackVerticalMovement?playerPos.y:transform.position.y,
		                   playerPos.z + zOffset);
	}


}
