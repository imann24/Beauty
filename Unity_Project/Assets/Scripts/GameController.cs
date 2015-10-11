using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameController Instance;

	public delegate void LoadLevelAction (Global.Scenes sceneToLoad);
	public delegate void EnterLevelAction (Global.Scenes currentScene);
	public delegate void StartGameAction ();

	public static event LoadLevelAction OnLoadLevel;
	public static event	EnterLevelAction OnEnterLevel;
	public static event StartGameAction OnStartGame;

	public bool GameStart {get; private set;}

	bool instructionScreenActive = true;
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
		SubscribeEvents();
	}

	void OnDestroy () {
		UnsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && !GameStart && !instructionScreenActive) {
			GameStart = true;
			if (OnStartGame != null) {
				OnStartGame();
			}
		}
	}

	void OnLevelWasLoaded (int level) {
		if (OnEnterLevel != null) {
			OnEnterLevel((Global.Scenes) level);
		}
	}

	void SubscribeEvents () {
		CanvasGroupController.OnInstructionsFadeOutFinished += HandleOnInstructionsFadeOutFinished;
	}

	void HandleOnTransitionComplete (bool fadeToBlack) {
		if (fadeToBlack && loadingScene) {
			loadingScene = false;
			Application.LoadLevel((int) targetScene);
		} else if (fadeToBlack) {

		}
	}

	void HandleOnInstructionsFadeOutFinished () {
		instructionScreenActive = false;
	}

	void UnsubscribeEvents () {
		CanvasGroupController.OnInstructionsFadeOutFinished -= HandleOnInstructionsFadeOutFinished;
	}

	public void BeginLoadScene (Global.Scenes sceneToLoad) {
		if (OnLoadLevel != null) {
			OnLoadLevel(sceneToLoad);
		}

		targetScene = sceneToLoad;
		loadingScene = true;
	}


}
