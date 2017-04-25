using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using System.IO;

public class SceneElementsList {
	private static string CELL_CONFIG_FILE_PATH = "shapes_file/CellShapesConfig";
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
		processStream (CELL_CONFIG_FILE_PATH, lineageData);
	}

	private void processStream(string FilePath, LineageData lineageData) {
		TextAsset file = Resources.Load (CELL_CONFIG_FILE_PATH) as TextAsset;
		if (file != null) {
			string filestream = file.text;
			string[] fLines = Regex.Split (filestream, "\n|\r|\r\n");

			string line;
			string name;
			string lineageName;
			string resourceLocation;
			int startTime;
			int endTime;
			List<string> cellNames;
			for (int i = 1; i < fLines.Length; i++) {
				line = fLines [i];

				string[] tokens = line.Split (',');

				if (tokens.Length == NUM_CSV_FIELDS) {
					name = tokens [DESCRIPTION_IDX];

					if (isCategoryLine (tokens)) {
						// skip over these for now
					} else {
						resourceLocation = tokens [RESOURCE_LOCATION_IDX];
						startTime = (int)Int32.Parse (tokens [START_TIME_IDX]);
						endTime = (int)Int32.Parse (tokens [END_TIME_IDX]);

						// check for first time that the .obj resource exists
						int effectiveStartTime = GeometryLoader.getEffectiveStartTime(resourceLocation, startTime, endTime);
						if (effectiveStartTime != startTime) {
							startTime = effectiveStartTime;
						}

						// vector of cell names
						cellNames = new List<string>();
						string[] cellNamesTokens = tokens [CELL_IDX].Split (' ');
						for (int k = 0; k < cellNamesTokens.Length; k++) {
							cellNames.Add (cellNamesTokens [k]);
						}

						lineageName = name;
						if (name.Contains ("(")) {
							lineageName = name.Substring (0, name.IndexOf ("(")).Trim ();
						}

						if (lineageData.isCellName (lineageName)) {
							effectiveStartTime = lineageData.getFirstOccurenceOf (lineageName);
							int effectiveEndTime = lineageData.getLastOccurentOf (lineageName);

							// use the later one of the config start time and the effective lineage start time
							startTime = effectiveStartTime > startTime ? effectiveStartTime : startTime;

							// use the earlier one of the config start time and effective lineage start time
							endTime = effectiveEndTime < endTime ? effectiveEndTime : endTime;
						}
							
						SceneElement element = new SceneElement (
							lineageName,
							cellNames,
							tokens [MARKER_IDX],
							tokens [IMAGING_SRC_IDX],
							resourceLocation,
							startTime,
							endTime,
							tokens [COMMENTS_IDX]);
						addSceneElement (element);

						// all the map stuff happens after here in orig WG
					}
				}
			}
		}
	}





//		using (var fs = File.OpenRead (FilePath))
//		using (var reader = new StreamReader (fs)) {
//			if (reader.EndOfStream) return;
//
//			// skip csv file heading
//			reader.ReadLine();
//
//			while (!reader.EndOfStream) {
//
//				string line;
//				string name;
//				string lineageName;
//				string resourceLocation;
//				int startTime;
//				int endTime;
//				List<string> cellNames;
//				
//				line = reader.ReadLine ();
//				if (line != null) {
//					string[] tokens = line.Split (',');
//
//					if (tokens.Length == NUM_CSV_FIELDS) {
//						name = tokens [DESCRIPTION_IDX];
//
//						if (isCategoryLine (tokens)) {
//							// skip over these for now
//						} else {
//							resourceLocation = tokens [RESOURCE_LOCATION_IDX];
//							startTime = (int)Int32.Parse (tokens [START_TIME_IDX]);
//							endTime = (int)Int32.Parse (tokens [END_TIME_IDX]);
//
//							// check for first time that the .obj resource exists
//							int effectiveStartTime = GeometryLoader.getEffectiveStartTime(resourceLocation, startTime, endTime);
//							if (effectiveStartTime != startTime) {
//								startTime = effectiveStartTime;
//							}
//
//							// vector of cell names
//							cellNames = new List<string>();
//							string[] cellNamesTokens = tokens [CELL_IDX].Split (' ');
//							for (int i = 0; i < cellNamesTokens.Length; i++) {
//								cellNames.Add (cellNamesTokens [i]);
//							}
//
//							lineageName = name;
//							if (name.Contains ("(")) {
//								lineageName = name.Substring (0, name.IndexOf ("(")).Trim ();
//							}
//
//							if (lineageData.isCellName (lineageName)) {
//								effectiveStartTime = lineageData.getFirstOccurenceOf (lineageName);
//								int effectiveEndTime = lineageData.getLastOccurentOf (lineageName);
//
//								// use the later one of the config start time and the effective lineage start time
//								startTime = effectiveStartTime > startTime ? effectiveStartTime : startTime;
//
//								// use the earlier one of the config start time and effective lineage start time
//								endTime = effectiveEndTime < endTime ? effectiveEndTime : endTime;
//							}
//
//							SceneElement element = new SceneElement (
//								                       lineageName,
//								                       cellNames,
//								                       tokens [MARKER_IDX],
//								                       tokens [IMAGING_SRC_IDX],
//								                       resourceLocation,
//								                       startTime,
//								                       endTime,
//								                       tokens [COMMENTS_IDX]);
//							addSceneElement (element);
//
//							// all the map stuff happens after here in orig WG
//						}
//					}
//				}
//			}
//		}
//	}

	private bool isCategoryLine(string[] tokens) {
		if (tokens.Length == NUM_CSV_FIELDS && !(tokens [DESCRIPTION_IDX].Length == 0)) {
			bool isCategoryLine = true;

			// check that all other fields are empty
			for (int i = 1; i < NUM_CSV_FIELDS; i++) {
				isCategoryLine &= tokens [i].Length == 0;
			}
			return isCategoryLine;
		}
		return false;
	}

	/*
	 * 
	 */ 
	public int getFirstOccurenceOf(string name) {
		int time = Int32.MinValue;
		foreach (SceneElement se in elementsList) {
			if (se.getSceneName ().ToLower ().Equals (name.ToLower ())) {
				time = se.getStartTime ();
			}
		}
		return time + 1;
	}

	/*
	 * 
	 */
	public int getLastOccurrenceOf(string name) {
		int time = Int32.MinValue;
		foreach (SceneElement se in elementsList) {
			if (se.getSceneName ().ToLower ().Equals (name.ToLower ())) {
				time = se.getEndTime ();
			}
		}
		return time + 1;
	}

	/*
	 * 
	 */ 
	public void addSceneElement(SceneElement element) {
		if (element != null) {
			elementsList.Add (element);
		}
	}

	/*
	 * 
	 */
	public string[] getSceneElementNamesAtTime(int time) {
		// add lineage names of all structures at time
		List<string> list = new List<string>();
		foreach (SceneElement se in elementsList) {
			if (se.existsAtTime (time)) {
				if (se.isMulticellular () || se.getAllCells ().Count == 0) {
					list.Add (se.getSceneName ());
				} else {
					list.Add (se.getAllCells () [0]);
				}
			}
		}
		return list.ToArray ();
	}

	/*
	 * 
	 */ 
	public List<SceneElement> getSceneElementsAtTime(int time) {
		List<SceneElement> elements = new List<SceneElement> ();
		foreach (SceneElement se in elementsList) {
			if (se.existsAtTime (time)) {
				elements.Add (se);
			}
		}
		return elements;
	}

	/*
	 * 
	 */
	public List<string> getAllSceneNames() {
		List<string> names = new List<string> ();
		foreach (SceneElement se in elementsList) {
			names.Add (se.getSceneName ());
		}

		// make sure this changes the list and doesn't create a new one that is set to nothing
		names.Sort ();

		return names;
	}

	/*
	 * 
	 */ 
	public List<string> getAllMulticellSceneNames() {
		List<string> names = new List<string> ();
		foreach (SceneElement se in elementsList) {
			if (se.isMulticellular ()) {
				names.Add (se.getSceneName ());
			}
		}

		// make sure this changes the list and doesn't create a new one that is set to nothing
		names.Sort ();

		return names;
	}

	/*
	 * 
	 */ 
	public bool isMulticellStructureName(string name) {
		name = name.Trim ();
		foreach (string cellName in getAllMulticellSceneNames()) {
			if (cellName.ToLower ().Equals (name.ToLower ())) {
				return true;
			}
		}
		return false;
	}

	/*
	 * TODO
	 */
	public string getCommentbyName(string name) {
		return "";
	}

	public List<SceneElement> getElementsList() {
		return elementsList;
	}
}