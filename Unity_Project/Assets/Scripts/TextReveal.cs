using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]

public class TextReveal : MonoBehaviour {
	public delegate void TextFullyRevealed(string textName);
	public static event TextFullyRevealed OnTextFullyRevealed;

	Text textAsset;
	string fullText;
	// Use this for initialization
	void Start () {
		AssignReferences();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AssignReferences () {
		textAsset = GetComponent<Text>();
		fullText = textAsset.text;
		textAsset.text = string.Empty;
	}

	public void StartRevealText (float waitTime = 0.0f) {
		StartCoroutine(RevealText(waitTime));
	}

	IEnumerator RevealText (float waitTime = 0.0f) {
		float normalSpeed = 0.1f;
		float acceleratedSpeed = 0.5f;
		float speed = normalSpeed;

		yield return new WaitForSeconds(waitTime);

		for (int i = 0; i <= fullText.Length; i++) {
			speed = Input.anyKey?acceleratedSpeed:normalSpeed;
			textAsset.text = fullText.Substring(0, i);
			yield return new WaitForSeconds(Time.deltaTime/speed);
		}

		if (OnTextFullyRevealed != null) {
			OnTextFullyRevealed(gameObject.name);
		}
	}
}
