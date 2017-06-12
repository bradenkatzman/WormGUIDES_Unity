using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartsList {
	private static List<string> functionalNames;
	private static List<string> lineageNames;
	private static List<string> descriptions;

	public static void initPartsList() {
		List<List<string>> pl = PartsListLoader.buildPartsList();
		if (pl != null 
			&& pl.Count == 3 
			&& (pl[1].Count == pl[2].Count)) {
			functionalNames = pl [0];
			lineageNames = pl [1];
			descriptions = pl [2];
		}
	}


	/*
	 * Determine if the given cell has a keyword match in its description
	 * or in the descriptions of its children with the given query string
	 * 
	 * If the given cell is a pure clone i.e. has a keyword match in its description, return 0
	 * If the given cell has a keyword match in its child, return the number of generations between parent and child
	 * If the given cell has no keyword match in its description or its children's descriptions, return -1
	 */ 
	public static int findDescriptionMatch(string cellName, string query) {
		// first see if this cell is a pure clone by looking for its index in the lineage names list
		int cellNameIdx = getIndexByLineageName (cellName);
		if (cellNameIdx != -1 
			&& descriptions[cellNameIdx].Contains(query)) return 0;

		// only proceed if this cell is a parent of a parts list entry
		if (isParentOfPartsListEntry (cellName)) {
			int generationsToFirstChildWithKeywordMatch = 100;

			for (int i = 0; i < lineageNames.Count; i++) {
				if (lineageNames [i].ToLower ().StartsWith (cellName.ToLower ()) 
					&& descriptions[i].Contains(query) 
					&& (lineageNames[i].Length - (lineageNames[i].IndexOf(cellName) + 1)) < generationsToFirstChildWithKeywordMatch)  {

					// find the number of generations to this child and save
					generationsToFirstChildWithKeywordMatch = lineageNames [i].Length - (lineageNames [i].IndexOf (cellName) + 1);
				}
			}

			if (generationsToFirstChildWithKeywordMatch != 100) return generationsToFirstChildWithKeywordMatch;
		}

		// no match in self or children
		return -1;
	}

	private static bool isParentOfPartsListEntry(string cellName) {
		foreach (string entry in lineageNames) {
			if (entry.StartsWith(cellName)) return true;
		}

		return false;
	}

	private static int getIndexByLineageName(string cell) {
		for (int i = 0; i < lineageNames.Count; i++) {
			if (cell.ToLower ().Equals (lineageNames [i].ToLower ())) {
				return i;
			}
		}

		return -1;
	}
}