using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	public static MessageController Instance;
	public MessageComponent TopOfScreenMessage;
	public enum Screen {Top, Bottom, Left, Right, Center};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			AdvanceMessage(Screen.Top);
		}
	}

	public void AdvanceMessage (Screen screenLocation) {
		if (screenLocation == Screen.Top) {
			TopOfScreenMessage.NextMessage();
		}
	}
}
