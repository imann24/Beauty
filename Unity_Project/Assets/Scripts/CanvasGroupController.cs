using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]

public class CanvasGroupController : MonoBehaviour {
	public delegate void InstructionsFadeOutFinished ();
	public static event InstructionsFadeOutFinished OnInstructionsFadeOutFinished;

	public enum CanvasGroupType {TitleScreen, InstructionScreen, InstructionMessage};
	public CanvasGroupType Type;
	CanvasGroup canvasGroup;

	public int fadedInCount;

	// Use this for initialization
	void Start () {
		SubscribeEvents();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void SubscribeEvents () {
		if (Type == CanvasGroupType.TitleScreen) {
			GameController.OnStartGame += HandleOnStartGame;
		} else if (Type == CanvasGroupType.InstructionMessage) {
			TextReveal.OnTextFullyRevealed += HandleMessageRevealed;
		} else if (Type == CanvasGroupType.InstructionScreen) {
			MessageController.OnInstructionsComplete += HandleInstructionsComplete;
		}
	}

	void UnsubscribeEvents () {
		if (Type == CanvasGroupType.TitleScreen) {
			GameController.OnStartGame -= HandleOnStartGame;
		} else if (Type == CanvasGroupType.InstructionMessage) {
			TextReveal.OnTextFullyRevealed -= HandleMessageRevealed;
		} else if (Type == CanvasGroupType.InstructionScreen) {
			MessageController.OnInstructionsComplete -= HandleInstructionsComplete;
		}
	}

	private void HandleOnStartGame () {
		StartCoroutine(LerpOpacity(0));
	}

	private void HandleMessageRevealed (string sourceName) {
		if (gameObject.name == sourceName) {
			StartCoroutine(LerpOpacity(0, 1.0f));
		} else if (fadedInCount == 0) {
			StartCoroutine(LerpOpacity(1.0f));
		}
	}

	private void HandleInstructionsComplete () {
		StartCoroutine(LerpOpacity(0, Input.anyKey?2.5f:7.5f));
	}

	IEnumerator LerpOpacity (float targetOpacity, float pauseTime = 0.0f) {
		float numSteps = 60f;
		float step = (targetOpacity - canvasGroup.alpha)/numSteps;

		yield return new WaitForSeconds(pauseTime);
		for (int i = 0; i < numSteps; i++) {
			canvasGroup.alpha += step;
			yield return new WaitForEndOfFrame();
		}

		canvasGroup.alpha = targetOpacity;

		CheckForInstructionScreenFadeout();

	}

	void CheckForInstructionScreenFadeout () {
		if (canvasGroup.alpha == 0 && 
		    Type == CanvasGroupType.InstructionScreen &&
		    OnInstructionsFadeOutFinished != null) {
			OnInstructionsFadeOutFinished();
		}
	}
}
