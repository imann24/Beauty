using UnityEngine;
using System.Collections;

public class CSVReader : MonoBehaviour {
	public TextAsset CSV;
	public TextAsset TSV;
	public static CSVReader Instance;
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string [][] ParseTSV (TextAsset TSV) {
		return ParseCSV(TSV, '\t');
	}

	// Returns a 2d array of the CSV
	public string [][] ParseCSV (TextAsset CSV, char separator = ',') {
		if (CSV == null) {
			return null;
		}

		string[] CSVByLine = CSV.text.Split('\n');
		string[][] CSVByCell = new string[CSVByLine.Length][];

		for (int i = 0; i < CSVByLine.Length; i++) {
			CSVByCell[i] = Utility.SplitString(CSVByLine[i], separator);
		}

		return CSVByCell;
	}

	public string [][] ParseCSV () {
		return ParseCSV(CSV);
	}

	public string [][] ParseTSV () {
		return ParseTSV(TSV);
	}
}
