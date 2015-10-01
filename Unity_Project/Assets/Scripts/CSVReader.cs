using UnityEngine;
using System.Collections;

public class CSVReader : MonoBehaviour {
	public TextAsset CSV;
	public static CSVReader Instance;
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Returns a 2d array of the CSV
	public string [][] ParseCSV (TextAsset CSV) {
		string[] CSVByLine = CSV.text.Split('\n');
		string[][] CSVByCell = new string[CSVByLine.Length][];

		for (int i = 0; i < CSVByLine.Length; i++) {
			CSVByCell[i] = Utility.SplitString(CSVByLine[i], ',');
		}

		return CSVByCell;
	}
}
