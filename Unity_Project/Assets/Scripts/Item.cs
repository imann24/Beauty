using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider2D))]

public class Item : MonoBehaviour {
	public float ColliderScale = 1.25f;

	public delegate void MessageAction (string message, bool playerEnteringZone, Item eventSource);
	public static event MessageAction OnMessage;

	public static Dictionary<string, Item> AllItems = new Dictionary<string, Item>();
	public string [] Messages;
	private int currentMessageIndex = 0;

	private float maxGlowOpacity = 1.0f;
	private static string GlowObjectName = "Glow";
	SpriteRenderer glowRenderer;

	void Awake () {
		AllItems.Add(gameObject.name, this);
		glowRenderer = getGlowSpriteRenderer();
		GetComponent<BoxCollider2D>().size *= ColliderScale;
	}

	void OnDestroy () {
		AllItems.Remove(gameObject.name);
	}

	public string ReadMessage () {
		if (Messages.Length == 0) {
			Debug.LogWarning("No messages to display");
			return "";
		}
		int offset = 1;

		return Messages[currentMessageIndex<Messages.Length-offset?currentMessageIndex++:currentMessageIndex];
	}

	public string ResetCurrentMessage () {
		return Messages[currentMessageIndex = 0];
	}
	
	public bool LastMessage (string currentMessage) {

		return Messages.Length == 0 || Messages[Messages.Length-1].Contains(currentMessage);
	}


	public void SetMessages (string [] messages) {
		Messages = messages;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (Messages.Length == 0) {
			return;
		}
		StartCoroutine(FadeOpacity(maxGlowOpacity));
		if (collider.gameObject == Global.Player && OnMessage != null) {
			OnMessage(Messages[0], true, this);
		}
	}

	void OnTriggerExit2D (Collider2D collider) {
		if (Messages.Length == 0) {
			return;
		}

		StartCoroutine(FadeOpacity(0));
		if (collider.gameObject == Global.Player && OnMessage != null) {
			OnMessage("", false, this);
		}
	}

	void OnTriggerStay2D (Collider2D collider) {
		if (ItemController.ActiveItem != null && ItemController.ActiveItem != this) {
			StartCoroutine(FadeOpacity(0));
		}
	}

	SpriteRenderer getGlowSpriteRenderer () {
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).name.Contains(GlowObjectName)) {
				return transform.GetChild(i).GetComponent<SpriteRenderer>();
			}
		}

		return null;
	}

	IEnumerator FadeOpacity (float targetOpacity) {
		Color currentColorOfSprite = glowRenderer.color;
		float numSteps = 60f;

		float step = (targetOpacity - glowRenderer.color.a)/numSteps;

		for (int i = 0; i < numSteps; i++) {
			currentColorOfSprite = glowRenderer.color;
			currentColorOfSprite.a = Mathf.Clamp(currentColorOfSprite.a += step, 0, maxGlowOpacity);
			glowRenderer.color = currentColorOfSprite;
			yield return new WaitForEndOfFrame();
		}

		glowRenderer.color = currentColorOfSprite;
	}
	
}
