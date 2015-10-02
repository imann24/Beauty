using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public static Dictionary <Global.Direction, State> DirectionToState = new Dictionary<Global.Direction, State>();

	public delegate void WalkingAction (State newState);
	public static event WalkingAction OnWalkChange;

	public float PlayerAcceleration = 50;
	public float MaxPlayerSpeed = 200;
	public float ClimbSpeed;
	public float JumpHeight = 1000;
	public bool RigibodyDisabled = false;

	// state
	public bool IsWalking {get; private set;}

	Rigidbody2D rigibody;
	BoxCollider2D myCollider;
	Animator animator;

	bool touchingGround = false;
	bool touchingLadder = false;

	Item currentItemHoveringOver; 

	const string STOP_TRIGGER = "Stopping";
	const string LEFT_TRIGGER = "WalkingLeft";
	const string RIGHT_TRIGGER = "WalkingRight";

	public enum State {WalkingLeft, WalkingRight, WalkingUp, WalkingDown, Stoppped};
	State currentState;
	Global.Direction Facing = Global.Direction.Right;

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
			Notifications.Instance.SetNotification(currentItemHoveringOver.ReadMessage(), Notifications.Notification.BottomScreen);
		}

		bool noMovement = true;

		
		State primaryState = State.Stoppped;

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			primaryState = State.WalkingUp;
			Movement(Global.Direction.Up, rigibody.velocity.x);
		} else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			primaryState = State.WalkingDown;
			Movement(Global.Direction.Down, rigibody.velocity.x);
		} 



		
		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && 
		    (!Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.RightArrow))) {
			primaryState = State.WalkingLeft;
			Movement(Global.Direction.Left, rigibody.velocity.y);
		} else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&
		           (!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.LeftArrow))) {
			primaryState = State.WalkingRight;
			Movement(Global.Direction.Right, rigibody.velocity.y);
		} 

		if (primaryState != State.Stoppped) {
			noMovement = false;
			if (!IsWalking) {
				IsWalking = true;
				if (OnWalkChange != null) {
					OnWalkChange(primaryState);
				}
			}
		} else {
			if (IsWalking) {
				IsWalking = false;
				if (OnWalkChange != null) {
					OnWalkChange(primaryState);
				}
			}
		}

		if (noMovement) {
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

	void Movement (Global.Direction direction, float otherDirectionSpeed = 0) {


		rigibody.isKinematic = false;

		if (direction == Global.Direction.Left) {
			rigibody.velocity = new Vector2(-PlayerAcceleration, otherDirectionSpeed);
			UpdateState(State.WalkingLeft);
			Facing = Global.Direction.Left;
		} else if (direction == Global.Direction.Right) {
			rigibody.velocity = new Vector2(PlayerAcceleration, otherDirectionSpeed);
			UpdateState(State.WalkingRight);
			Facing = Global.Direction.Right;
		} else if (direction == Global.Direction.Up) {
			rigibody.velocity = new Vector2(otherDirectionSpeed, PlayerAcceleration);
			UpdateState(DirectionToState[Facing]);
		} else if (direction == Global.Direction.Down) {
			rigibody.velocity = new Vector2(otherDirectionSpeed, -PlayerAcceleration);
			UpdateState(DirectionToState[Facing]);
		} else if (direction == Global.Direction.None) {

			rigibody.velocity = new Vector2(0, 0);

			UpdateState(State.Stoppped);

			rigibody.isKinematic = touchingLadder;

			if (RigibodyDisabled) {
				rigibody.Sleep();
			}
		}


		rigibody.velocity = new Vector2(Mathf.Clamp(rigibody.velocity.x, -MaxPlayerSpeed, MaxPlayerSpeed),
		                                Mathf.Clamp(rigibody.velocity.y, -MaxPlayerSpeed, MaxPlayerSpeed));
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

		rigibody.gravityScale = Global.GRAVITY_ON?1:0;

		InitializeDictionary();
	}
	
	void UpdateState (State updatedState) {
		if (updatedState != currentState) {
			currentState = updatedState;
			UpdateAnimation(updatedState);
		}
	}

	void UpdateAnimation (State newState) {
		Debug.Log("Changing the animation to " + newState);
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

	// Creates the dictionary of enums to map between directions and states
	public static void InitializeDictionary () {
		if (DirectionToState.Count == 0) {
			for (int i = 0; i < Global.DIRECTION_COUNT; i++) {
				DirectionToState.Add((Global.Direction)i, (State)i);
			}
		}
	}
}
