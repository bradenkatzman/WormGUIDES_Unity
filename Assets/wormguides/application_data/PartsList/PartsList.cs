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
		CustomTuple ct = getInfoByLineageName (cellName, query);
		if (ct.getLineageNameIdx () != -1
		    && descriptions [ct.getLineageNameIdx ()].Contains (query)) {
			return 0;
		}

		// only proceed if this cell is a parent of a parts list entry
		if (ct.getIsParentOfPartsListEntryFlag() 
			&& ct.getGenerationsToFirstChildWithKeywordMatch() != 1000) {
			return ct.getGenerationsToFirstChildWithKeywordMatch ();
		}

		// no match in self or children
		return -1;
	}

	private static CustomTuple getInfoByLineageName(string cell, string query) {
		CustomTuple ct = new CustomTuple ();
//		bool setFirstChildMatch = false;
//		for (int i = 0; i < lineageNames.Count; i++) {
//			if (cell.ToLower ().Equals (lineageNames [i].ToLower ())) {
//				ct.setLineageNameIdx (i);
//				return ct;
//			} else if (!setFirstChildMatch 
//						&& lineageNames [i].ToLower ().StartsWith (cell.ToLower ())) {
//
//				ct.setIsParentOfPartsListEntryFlag (true);
//				for (int k = 0; i < lineageNames.Count; i++) {
//					if (lineageNames [k].ToLower ().StartsWith (cell.ToLower ()) 
//						&& descriptions[k].Contains(query) 
//						&& (lineageNames[k].Length - (lineageNames[i].IndexOf(cell) + 1)) < ct.getGenerationsToFirstChildWithKeywordMatch())  {
//
//						setFirstChildMatch = true;
//
//						// find the number of generations to this child and save
//						ct.setGenerationsToFirstChildWithKeywordMatch(lineageNames [i].Length - (lineageNames [i].IndexOf (cell) + 1));
//					}
//				}
//			}
//		}

		return ct;
	}

    public static string getTerminalNameByLineageName(string cell)
    {
        if (cell.Length == 0)
        {
            return cell;
        }

        string terminalName = cell;
        for (int i = 0; i < lineageNames.Count; i++)
        {
            if (lineageNames[i].ToLower().Equals(cell.ToLower())) {
                return functionalNames[i];
            }
        }

        return terminalName;
    }

    public static List<string> getLineageNamesByTerminalName(string cell)
    {
        if (cell.Length == 0)
        {
            return new List<string>();
        }

        List<string> lineageNamesMatch = new List<string>();

        for (int i = 0; i < functionalNames.Count; i++) {
            if (functionalNames[i].ToLower().StartsWith(cell.ToLower()))
            {
                lineageNamesMatch.Add(lineageNames[i]);
            }

        }

        return lineageNamesMatch;
    }

    // TODO - make this handle rules such as XXDL --> there could be a situation where you want to the rule to apply to all XXD, 
    // but right now this will only give you the root XX so you can't distinguish among multiple appended division characters
    public static string getTerminalNameRootByLineageName(string cell)
    {
        if (cell.Length == 0)
        {
            return cell;
        }

        string terminalName = getTerminalNameByLineageName(cell);

        // now parse the terminal name for the root (i.e. everything before an L, R, D, V or #
        for (int i = terminalName.Length-1; i >= 0; i--)
        {
            if (terminalName[i].Equals('L') || terminalName[i].Equals('R') || terminalName[i].Equals('D')
                || terminalName[i].Equals('V') || System.Char.IsDigit(terminalName[i]))
            {
                continue; // keep moving back in the string
            } else
            {
                // we've reached the end of the characters that are appended to the root terminal name. return the root
                return terminalName.Substring(0, i+1);
            }
        }

        // need this to avoid compiler errors
        return "";
    }

    public static List<string> getLineageNames()
    {
        return lineageNames;
    }

    public static List<string> getFunctionalNames()
    {
        return functionalNames;
    }

	private class CustomTuple {
		private int lineageNameIdx;
		private bool isParentOfPartsListEntry;
		private int generationsToFirstChildWithKeywordMatch;

		public CustomTuple() {
			this.lineageNameIdx = -1;
			this.isParentOfPartsListEntry = false;
			this.generationsToFirstChildWithKeywordMatch = 1000;
		}

		public void setLineageNameIdx(int idx) {
			this.lineageNameIdx = idx;
		}

		public void setIsParentOfPartsListEntryFlag(bool b) {
			this.isParentOfPartsListEntry = b;
		}

		public void setGenerationsToFirstChildWithKeywordMatch(int gens) {
			this.generationsToFirstChildWithKeywordMatch = gens;
		}

		public int getLineageNameIdx() {
			return this.lineageNameIdx;
		}

		public bool getIsParentOfPartsListEntryFlag() {
			return this.isParentOfPartsListEntry;
		}

		public int getGenerationsToFirstChildWithKeywordMatch() {
			return this.generationsToFirstChildWithKeywordMatch;
		}
	}
}