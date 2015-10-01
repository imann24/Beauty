using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {
	public static Dictionary<string, Item> AllItems = new Dictionary<string, Item>();
	public string [] Messages;
	private int currentMessageIndex = 0;

	void Start () {
		AllItems.Add(gameObject.name, this);
	}

	void OnDestroy () {
		AllItems.Remove(gameObject.name);
	}

	public string ReadMessage () {
		return Messages[currentMessageIndex++%Messages.Length];
	}

	public void SetMessages (string [] messages) {
		Messages = messages;
	}
}
