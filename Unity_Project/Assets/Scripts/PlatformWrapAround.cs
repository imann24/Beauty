using UnityEngine;
using System.Collections;

public class PlatformWrapAround : MonoBehaviour {
	public delegate void PlayerCollideAction(Global.Direction platformSide);
	public event PlayerCollideAction OnPlayerCollide;

	public Global.Direction PlatformSide;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D (Collider2D collision) {
		if (OnPlayerCollide != null) {
			OnPlayerCollide(PlatformSide);
		}
	}

}
