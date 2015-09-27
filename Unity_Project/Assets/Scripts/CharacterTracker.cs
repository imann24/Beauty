using UnityEngine;
using System.Collections;

public class CharacterTracker : MonoBehaviour {
	public GameObject Player;

	float zOffset = -10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = FollowPlayer(Player.transform.position, false);
	}


	Vector3 FollowPlayer (Vector3 playerPos, bool trackHorizontalMovement ) {
		return new Vector3(trackHorizontalMovement?playerPos.x:transform.position.x, playerPos.y, playerPos.z + zOffset);
	}
}
