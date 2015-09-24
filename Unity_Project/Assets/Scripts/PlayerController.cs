using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float PlayerAcceleration = 50;
	public float MaxPlayerSpeed = 200;
	public float JumpHeight = 1000;

	Rigidbody2D rigibody;
	bool touchingGround = false;
	bool touchingLadder = false;
	// Use this for initialization
	void Start () {
		EstablishReferences();
	}



	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
			Jump();
		}*/


		if (Input.GetKey(KeyCode.A)) {
			Movement(Global.Direction.Left);
		} else if (Input.GetKey(KeyCode.D)) {
			Movement(Global.Direction.Right);
		} else if (Input.GetKey(KeyCode.W)) {
			Movement(Global.Direction.Up);
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		touchingGround = true;
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.name.Contains(Global.LADDER)) {
			touchingLadder = true;
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		touchingGround = false;
	}

	void OnTriggerExit2D (Collider2D collision) {
		if (collision.gameObject.name.Contains(Global.LADDER)) {
			touchingLadder = false;
		}
	}

	void Movement (Global.Direction direction) {
		if (direction == Global.Direction.Left) {
			rigibody.AddForce(new Vector2(-PlayerAcceleration, 0));
		} else if (direction == Global.Direction.Right) {
			rigibody.AddForce(new Vector2(PlayerAcceleration, 0));
		} else if (direction == Global.Direction.Up && touchingLadder) {
			rigibody.AddForce(new Vector2(0, PlayerAcceleration));
		}
		rigibody.velocity = new Vector2(Mathf.Clamp(rigibody.velocity.x, -MaxPlayerSpeed, MaxPlayerSpeed), Mathf.Clamp(rigibody.velocity.y, -MaxPlayerSpeed, MaxPlayerSpeed));
	}

	void Jump () {
		if (touchingGround) {
			rigibody.AddForce(new Vector2(0, JumpHeight));
		}
	}

	void EstablishReferences () {
		rigibody = GetComponent<Rigidbody2D>();
	}
}
