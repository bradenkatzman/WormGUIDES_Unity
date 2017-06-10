using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PartsListLoader {

	private static int NUMBER_OF_FIELDS = 3;
	private static string PARTSLIST_FILE_PATH = "partslist_file/partslist";
	private static int FUNCTIONAL_NAME_IDX = 0;
	private static int LINEAGE_NAME_IDX = 1;
	private static int DESCRIPTION_IDX = 2;

	public static List<List<string>> buildPartsList() {
		List<List<string>> partsList = new List<List<string>> ();

		// process prod info file
		TextAsset file = Resources.Load (PARTSLIST_FILE_PATH) as TextAsset;
		if (file != null) {
			List<string> functionalNames = new List<string> ();
			List<string> lineageNames = new List<string> ();
			List<string> descriptions = new List<string> ();

			string filestream = file.text;
			string[] fLines = Regex.Split (filestream, "\n|\r|r\n");

			for (int i = 0; i < fLines.Length; i++) {
				string line = fLines [i];

				// tokenize the line
				string[] values = line.Split('	');

				if (values.Length == NUMBER_OF_FIELDS) {
					functionalNames.Add (values [FUNCTIONAL_NAME_IDX]);
					lineageNames.Add (values [LINEAGE_NAME_IDX]);
					descriptions.Add (values [DESCRIPTION_IDX]);
				}
			}

			// add lists to partslist
			partsList.Add(functionalNames);
			partsList.Add (lineageNames);
			partsList.Add (descriptions);
		}

		return partsList;
	}
}