using UnityEngine;
using System.Collections;

public class Global {

	public enum Scenes {Id, Ego, SuperEgo};

	public enum Direction {Left, Right, Up, Down, None};

	// Used to identify game objects
	public const string LADDER = "Ladder";
	public const string PLATFORM = "Platform";
	public const string ITEM = "Item";

	// the player
	public static GameObject Player;
	public static Vector3 StartPos;

	// Tutorial
	public const string ITEM_INTERACTION_PROMPT = "Press Space to interact with item";
}
