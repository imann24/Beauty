using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameController Instance;

	public delegate void LoadLevelAction (Global.Scenes sceneToLoad);
	public delegate void EnterLevelAction (Global.Scenes currentScene);
	
	public static event LoadLevelAction OnLoadLevel;
	public static event	EnterLevelAction OnEnterLevel;

	bool loadingScene;
	Global.Scenes targetScene;

	void Awake () {
		// Singleton implementation
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start () {
	}

	void OnDestroy () {
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded (int level) {
		if (OnEnterLevel != null) {
			OnEnterLevel((Global.Scenes) level);
		}
	}

	void SubscribeEvents () {
		SceneTransition.OnTransitionComplete += HandleOnTransitionComplete;
	}

	void HandleOnTransitionComplete (bool fadeToBlack) {
		if (fadeToBlack && loadingScene) {
			loadingScene = false;
			Application.LoadLevel((int) targetScene);
		} else if (fadeToBlack) {

		}
	}

	void UnsubscribeEvents () {
		SceneTransition.OnTransitionComplete -= HandleOnTransitionComplete;
	}

	public void BeginLoadScene (Global.Scenes sceneToLoad) {
		if (OnLoadLevel != null) {
			OnLoadLevel(sceneToLoad);
		}

		targetScene = sceneToLoad;
		loadingScene = true;
	}


}
