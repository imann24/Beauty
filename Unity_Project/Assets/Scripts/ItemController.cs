using UnityEngine;
using System;
using System.Collections;

public class ItemController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		assignItemMessages();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// assigns the messages to the interactable items in the scene
	private void assignItemMessages () {

		string [][] itemMessagesByItem = CSVReader.Instance.ParseCSV();

		if (itemMessagesByItem == null) {
			Debug.LogWarning("The CSV TextAsset is missing from the CSVReader");
			return;
		}

		for (int i = 0; i <itemMessagesByItem.Length; i++) {

			string item = itemMessagesByItem[i][0];
			string [] messages = new string[itemMessagesByItem[i].Length-1];

			Array.Copy(itemMessagesByItem[i], 1, messages, 0, messages.Length);

			try {
				Item.AllItems[item].SetMessages(messages);
          	} catch {
				Debug.LogWarning(item + " does not exist in the scene");
			}
		}
	}
}
