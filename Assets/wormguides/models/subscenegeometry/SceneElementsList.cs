using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class SceneElementsList {
	private static string CELL_CONFIG_FILE_PATH = "Assets/wormguides/models/shapes_file/CellShapesConfig.csv";
	private static string ASTERISK = "*";
	private static string SLASH = "/";

	private static int NUM_CSV_FIELDS = 8;
	private static int DESCRIPTION_IDX = 0;
	private static int CELL_IDX = 1;
	private static int MARKER_IDX = 2;
	private static int IMAGING_SRC_IDX = 3;
	private static int RESOURCE_LOCATION_IDX = 4;
	private static int START_TIME_IDX = 5;
	private static int END_TIME_IDX = 6;
	private static int COMMENTS_IDX = 7;

	private List<SceneElement> elementsList;

	public SceneElementsList(LineageData lineageData) {
		elementsList = new List<SceneElement> ();
		buildListFromConfig (lineageData);
	}

	private void buildListFromConfig(LineageData lineageData) {
		string FilePath = Directory.GetCurrentDirectory () + SLASH + CELL_CONFIG_FILE_PATH;
		if (File.Exists (FilePath)) {
			processStream (FilePath, lineageData);
		}
	}

	private void processStream(string FilePath, LineageData lineageData) {
		using (var fs = File.OpenRead (FilePath))
		using (var reader = new StreamReader (fs)) {
			while (!reader.EndOfStream) {
				// do the reading			
			
			}
		}
	}
}