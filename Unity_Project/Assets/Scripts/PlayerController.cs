using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float PlayerAcceleration = 50;
	public float MaxPlayerSpeed = 200;
	public float ClimbSpeed;
	public float JumpHeight = 1000;

	Rigidbody2D rigibody;
	BoxCollider2D myCollider;
	Animator animator;

	bool touchingGround = false;
	bool touchingLadder = false;

	Item currentItemHoveringOver; 

	const string STOP_TRIGGER = "Stopping";
	const string LEFT_TRIGGER = "WalkingLeft";
	const string RIGHT_TRIGGER = "WalkingRight";

	public enum State {WalkingLeft, WalkingRight, Stoppped};
	State currentState;

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
		if (Input.GetKey(KeyCode.Space) && currentItemHoveringOver != null) {
			Notifications.Instance.SetNotification(currentItemHoveringOver.Message, Notifications.Notification.BottomScreen);
		}

		if (Input.GetKey(KeyCode.A)) {
			Movement(Global.Direction.Left);
		} else if (Input.GetKey(KeyCode.D)) {
			Movement(Global.Direction.Right);
		} else if (Input.GetKey(KeyCode.W)) {
			Movement(Global.Direction.Up);
		} else if (Input.GetKey(KeyCode.S)) {
			Movement(Global.Direction.Down);
		} else {
			Movement(Global.Direction.None);
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		touchingGround = true;
		touchingLadder = false;
		rigibody.velocity = new Vector2(0, 0);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.name.Contains(Global.LADDER)) {
			touchingLadder = true;
			myCollider.isTrigger = true;
		} else if (collider.gameObject.name.Contains(Global.PLATFORM) && touchingGround == false ) {
			myCollider.isTrigger = false;
			touchingLadder = false;
			touchingGround = true;
		} else if (collider.gameObject.name.Contains(Global.ITEM)) {
			currentItemHoveringOver = DetectItem(collider);
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		touchingGround = false;
	}

	void OnTriggerExit2D (Collider2D collider) {
		if (collider.gameObject.name.Contains(Global.LADDER)) {
			touchingLadder = false;
			myCollider.isTrigger = false;
		} else if (collider.gameObject.name.Contains(Global.ITEM)) {
			Notifications.Instance.SetNotification("", Notifications.Notification.BottomScreen);
			currentItemHoveringOver = null;
		}
	}

	void Movement (Global.Direction direction) {


		rigibody.isKinematic = false;

		if (direction == Global.Direction.Left) {
			rigibody.velocity = new Vector2(-PlayerAcceleration, 0);
			UpdateState(State.WalkingLeft);
		} else if (direction == Global.Direction.Right) {
			rigibody.velocity = new Vector2(PlayerAcceleration, 0);
			UpdateState(State.WalkingRight);
		} else if (direction == Global.Direction.Up && touchingLadder) {
			rigibody.AddForce(new Vector2(0, ClimbSpeed));
		} else if (direction == Global.Direction.Down) {
			rigibody.AddForce(new Vector2(0, -ClimbSpeed));
		} else if (direction == Global.Direction.None) {
			if (touchingLadder || touchingGround) {
				rigibody.velocity = new Vector2(0, 0);
			}

			UpdateState(State.Stoppped);

			rigibody.isKinematic = touchingLadder;
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
		myCollider = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();

		Global.Player = gameObject;
		Global.StartPos = transform.position;

		currentState = State.Stoppped;
	}
	
	void UpdateState (State updatedState) {
		if (updatedState != currentState) {
			currentState = updatedState;
			UpdateAnimation(currentState);
		}
	}

	void UpdateAnimation (State newState) {
		if (newState == State.Stoppped) {
			animator.SetTrigger(STOP_TRIGGER);
		} else if (newState == State.WalkingLeft) {
			animator.SetTrigger(LEFT_TRIGGER);
		} else if (newState == State.WalkingRight) {
			animator.SetTrigger(RIGHT_TRIGGER);
		}
	}

	Item DetectItem (Collider2D collider) {
		Notifications.Instance.SetNotification(Global.ITEM_INTERACTION_PROMPT, Notifications.Notification.BottomScreen);
		return collider.GetComponent<Item>();
	}
}
