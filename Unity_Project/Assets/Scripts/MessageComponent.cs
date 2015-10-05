using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]

public class MessageComponent : MonoBehaviour {

	public delegate void MessageReadAction (bool complete);
	public static MessageReadAction OnMessageRead;

	CanvasGroup canvasGroup;
	Text messageText;

	private bool revealingTheText;
	IEnumerator currentMessageChange;

	private Item currentItem;
	private string currentMessage;


	// Use this for initialization
	void Start () {
		setReferences();
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	public void NextMessage () {
		if (currentItem != null) {
			showMessage();
			string nextMessage = currentItem.ReadMessage();
			if (!string.IsNullOrEmpty(messageText.text) && currentItem.LastMessage(messageText.text) || revealingTheText) {
				if (OnMessageRead != null && !revealingTheText) {
					OnMessageRead(true);
					hideMessage();
					messageText.text = "";
					currentItem.ResetCurrentMessage();
				}

				return;
			}
			currentMessage = nextMessage;
			if (currentMessageChange != null) {
				StopCoroutine(currentMessageChange);
			}
			StartCoroutine(currentMessageChange = RevealText());
		}
	}

	void setReferences () {
		canvasGroup = GetComponent<CanvasGroup>();
		messageText = GetComponentInChildren<Text>();
	}

	void setMessage (string message, bool showMessage, Item source) {
		currentItem = source;
		currentMessage = message;
		if (currentMessageChange != null) {
			StopCoroutine(currentMessageChange);
		}
		StartCoroutine(currentMessageChange = RevealText());
		if (!showMessage) {
			this.hideMessage();
			messageText.text = "";
			currentItem = null;
		} 
	}

	void hideMessage () {
		StartCoroutine(LerpCanvasOpacity(0.0f));
	}

	void showMessage () {
		if (OnMessageRead != null) {
			OnMessageRead(false);
		}
		StartCoroutine(LerpCanvasOpacity(1.0f));
	}

	void subscribeEvents () {
		Item.OnMessage += setMessage;
	}

	void unsubscribeEvents () {
		Item.OnMessage -= setMessage;
	}

	
	IEnumerator LerpCanvasOpacity (float targetOpacity) {
		float numSteps = 60f;
		float step = (targetOpacity - canvasGroup.alpha)/numSteps;
		for (int i = 0; i < numSteps; i++) {
			canvasGroup.alpha += step;
			yield return new WaitForEndOfFrame();
		}
		
		canvasGroup.alpha = targetOpacity;
	}

	IEnumerator RevealText () {
		float pause = 1.0f;
		float regSpeed = 0.25f;
		float fastSpeed = 1.25f;
		float speed = regSpeed;
		while (canvasGroup.alpha == 0) {
			yield return new WaitForEndOfFrame();
		}
		if (!string.IsNullOrEmpty(messageText.text) && !Input.GetKey(KeyCode.Space)) {
			yield return new WaitForSeconds(pause);
		} else {
			yield return new WaitForSeconds(pause/2f);
		}
		revealingTheText = true;
		for (int i = 0; i < currentMessage.Length; i++) {
			if (Input.GetKey(KeyCode.Space)) {
				speed = fastSpeed;
			} else {
				speed = regSpeed;
			}

			messageText.text = currentMessage.Substring(0, i);
			yield return new WaitForSeconds(Time.deltaTime / speed);
		}
		revealingTheText = false;
	}
}
