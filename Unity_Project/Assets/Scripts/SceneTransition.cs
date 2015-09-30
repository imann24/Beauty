using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {

	public SceneTransition Instance;
	public delegate void TransitionCompleteAction (bool fadeToBlack);
	public static event TransitionCompleteAction OnTransitionComplete;
	CanvasGroup canvasGroup;
	
	void Awake () {
		// Singleton implementation
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
			canvasGroup = GetComponent<CanvasGroup>();
		} else {
			Destroy(this);
		}
	}


	// Use this for initialization
	void Start () {
		SubscribeReferences();
	}


	void OnDestroy () {
		UnsubscribeReferences();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void TransitionOutOfScene (Global.Scenes sceneToLoad) {
		Debug.Log("Fading in");
		StartCoroutine(LerpCanvasOpacity(1.0f));
	}

	void TransitionIntoScene (Global.Scenes currentScene) {
		Debug.Log("Fading out");
		StartCoroutine(LerpCanvasOpacity(0.0f));
	}

	void SubscribeReferences () {
		GameController.OnLoadLevel += TransitionOutOfScene;
		GameController.OnEnterLevel += TransitionIntoScene;
	}

	void UnsubscribeReferences () {
		GameController.OnLoadLevel -= TransitionOutOfScene;
		GameController.OnEnterLevel -= TransitionIntoScene;
	}

	IEnumerator LerpCanvasOpacity (float targetOpacity) {
		float numSteps = 60f;
		float step = (targetOpacity - canvasGroup.alpha)/numSteps;
		for (int i = 0; i < numSteps; i++) {
			canvasGroup.alpha += step;
			yield return new WaitForEndOfFrame();
		}

		canvasGroup.alpha = targetOpacity;

		if (OnTransitionComplete != null) {
			OnTransitionComplete(targetOpacity == 1.0f);
		}
	}
}
