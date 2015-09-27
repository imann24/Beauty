using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notifications : MonoBehaviour {
	public Text BottomScreenText;

	public static Notifications Instance;

	public enum Notification {BottomScreen};
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetNotification (string text, Notification position) {
		if (position == Notification.BottomScreen) {
			BottomScreenText.text = text;
		}
	}

}
