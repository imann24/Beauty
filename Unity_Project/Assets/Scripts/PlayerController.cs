using UnityEngine;
using System.Linq;
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
	public bool CanMove {get; private set;}

	Rigidbody2D rigibody;
	BoxCollider2D myCollider;
	Animator animator;

	bool touchingGround = false;
	bool touchingLadder = false;
	bool onInstructionScreen = true;

	Item currentItemHoveringOver; 

	// For animations
	const string STOP_TRIGGER = "Stopping";
	const string LEFT_TRIGGER = "WalkingLeft";
	const string RIGHT_TRIGGER = "WalkingRight";
	const string MOTHER_TRIGGER = "AddMother";
	const string REMOVE_MOTHER_TRIGGER = "RemoveMother";

	public enum State {WalkingLeft, WalkingRight, WalkingUp, WalkingDown, Stoppped};
	State currentState;
	Global.Direction Facing = Global.Direction.Right;

	// Use this for initialization
	void Start () {
		EstablishReferences();
		SubscribeEvents();
	}
	
	void OnDestroy () {
		UnsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {

		if (onInstructionScreen) {
			return;
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



		
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			Facing = Global.Direction.Left;
		}

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			Facing = Global.Direction.Right;
		} 

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
			primaryState = DirectionToState[Facing];
			Movement(Facing, rigibody.velocity.y);
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
			//myCollider.isTrigger = true;
		} else if (collider.gameObject.name.Contains(Global.PLATFORM) && touchingGround == false ) {
			//myCollider.isTrigger = false;
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
			//myCollider.isTrigger = false;
		} else if (collider.gameObject.name.Contains(Global.ITEM)) {
			Notifications.Instance.SetNotification("", Notifications.Notification.BottomScreen);
			currentItemHoveringOver = null;
		}
	}

	void Movement (Global.Direction direction, float otherDirectionSpeed = 0) {
		if (!CanMove) {
			return;
		}

		if (CheckForNoMovement() || (Input.GetKeyDown(KeyCode.Space) && ItemController.ActiveItem != null)) {
			UpdateAnimation(State.Stoppped);
		} else {
			animator.ResetTrigger(STOP_TRIGGER);
		}

		rigibody.isKinematic = false;

		if (direction == Global.Direction.Left) {
			rigibody.velocity = new Vector2(-PlayerAcceleration, otherDirectionSpeed);
			UpdateState(State.WalkingLeft);
		} else if (direction == Global.Direction.Right) {
			rigibody.velocity = new Vector2(PlayerAcceleration, otherDirectionSpeed);
			UpdateState(State.WalkingRight);
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
		CanMove = true;

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
		if (newState == State.Stoppped) {
			animator.SetTrigger(STOP_TRIGGER);
		} else if (newState == State.WalkingLeft) {
			animator.SetTrigger(LEFT_TRIGGER);
		} else if (newState == State.WalkingRight) {
			animator.SetTrigger(RIGHT_TRIGGER);
		}
	}

	void TogglePlayerMovement (bool enabled) {
		CanMove = enabled;
		if (!enabled) {
			rigibody.velocity = new Vector2(0, 0);
			UpdateState (State.Stoppped);
		}
	}
	
	Item DetectItem (Collider2D collider) {
		Notifications.Instance.SetNotification(Global.ITEM_INTERACTION_PROMPT, Notifications.Notification.BottomScreen);
		return collider.GetComponent<Item>();
	}

	void SubscribeEvents () {
		SceneTransition.OnTransitionBegin += (fadeToBlack) => TogglePlayerMovement(!fadeToBlack);
		MessageComponent.OnMessageRead += TogglePlayerMovement;
		TeleportArea.OnTeleportToRoom += HandleOnTeleportToRoom;
		GameController.OnStartGame += HandleOnStartGame;
	}
	
	void UnsubscribeEvents () {
		SceneTransition.OnTransitionBegin -= (fadeToBlack) => TogglePlayerMovement(!fadeToBlack);
		MessageComponent.OnMessageRead -= TogglePlayerMovement;
		TeleportArea.OnTeleportToRoom -= HandleOnTeleportToRoom;
		GameController.OnStartGame -= HandleOnStartGame;
	}

	void HandleOnTeleportToRoom (Global.Rooms TargetRoom) {
		StartCoroutine(TimedAnimationTransition(TargetRoom));
	}

	void HandleOnStartGame () {
		onInstructionScreen = false;
		CanMove = true;
	}

	bool CheckForNoMovement () {
		return !Input.anyKey;
	}

	IEnumerator TimedAnimationTransition (Global.Rooms TargetRoom) {
		yield return new WaitForSeconds(0.9f);

		Vector2 colliderOffset = myCollider.offset;

		if (TargetRoom == Global.Rooms.OutsideLeft ||
		    TargetRoom == Global.Rooms.OutsideRight) {
			animator.SetTrigger(MOTHER_TRIGGER);
			transform.Translate(Vector2.up);
			colliderOffset.y = -1.15f;
		} else if (TargetRoom == Global.Rooms.HallwayRight || 
		           TargetRoom == Global.Rooms.CreditsLeft) {
			animator.SetTrigger(REMOVE_MOTHER_TRIGGER);
			transform.Translate(Vector2.down);
			colliderOffset.y = -0.75f;
		}


		myCollider.offset = colliderOffset;
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
