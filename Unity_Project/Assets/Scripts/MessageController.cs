using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	public static MessageController Instance;
	public GameObject TopOfScreenMessage;
	public enum Screen {Top, Bottom, Left, Right, Center};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetMessage (Screen screenLocation) {
		if (screenLocation == Screen.Top) {
			//TopOfScreenMessage
		}
	}
}
