using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	public delegate void InstructionsCompleteAction();
	public static event InstructionsCompleteAction OnInstructionsComplete;

	public static MessageController Instance;
	public MessageComponent BottomOfScreenMessage;
	public enum Screen {Top, Bottom, Left, Right, Center};

	public TextReveal[] Instructions;
	private int instructionIndex;

	// Use this for initialization
	void Start () {
		BeginInstructionText();
		SubscribeEvents();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			AdvanceMessage(Screen.Bottom);
		}
	}

	public void AdvanceMessage (Screen screenLocation) {
		if (screenLocation == Screen.Bottom) {
			BottomOfScreenMessage.NextMessage();
		}
	}

	private void BeginInstructionText () {
		if (Instructions.Length == 0) {
			return;
		}

		Instructions[0].StartRevealText();
		instructionIndex++;
	}

	void SubscribeEvents () {
		TextReveal.OnTextFullyRevealed += HandleInstructionTextRevealed;
	}

	void UnsubscribeEvents () {
		TextReveal.OnTextFullyRevealed -= HandleInstructionTextRevealed;
	}

	void HandleInstructionTextRevealed (string instructionName) {
		if (instructionName == Instructions[instructionIndex-1].name && 
		    instructionIndex < Instructions.Length) {
			Instructions[instructionIndex++].StartRevealText(1.5f);
		}

		if (instructionIndex == Instructions.Length && OnInstructionsComplete != null) {
			OnInstructionsComplete();
		}
	}
}
