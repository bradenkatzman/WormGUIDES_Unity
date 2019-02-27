using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColorScheme
{
    private int firstIDX = 0;
    private int lastIDX = 1;

    private Dictionary<string, int> currentColorSchemeDict;
    private List<Color> currentColorList;


    private string[] colorURLS = new string[] {"http://scene.wormguides.org/wormguides/testurlscript?/set/saavl-n$@+#ffff0000/sibd-n$@+#ffe6ccff/avg-n$@+#ffffff00/rip-n$@+#ff99cc99/rih-n$@+#ff99cc99/smdd-n$@+#ff4d66cc/aiy-n$@+#ffff9900/rig-n$@+#fff90557/pvt-n$@+#ffffffff/dva-n$@+#ffffb366/ala-n$@+#ff01c501/aim-n$@+#ffffff66/aial-n$@+#ff8066cc/afd-n$@+#ff4de6e2/rim-n$@+#ffb399ff/avd-n$@+#ffff9980/CEH-37=Amphid=Multicellular=Structure-H+#ff994d66/rmdd-n$@+#ff664db3/rmg-n$@+#ffffffff/ada-n$@+#ffffffff/aiz-n$@+#ffffffff/bdu-n$@+#ffffffff/smbd-n$@+#ffffffff/pha-n$@+#ffffffff/hsn-n$@+#ffffffff/phb-n$@+#ffffffff/Embryo=Outline-M+#4b00f2ff/Hypoderm-M+#043381aa/view/time=360/rX=0.0/rY=0.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/Android/",
    "http://scene.wormguides.org/wormguides/testurlscript?/set/E-s<$+#ff13ff03/MS-s<$+#ff05ffd6/D-s<$+#fffd00ff/C-s<$+#ffffb3b3/P4-s<$+#ffffff4d/ABal-s<$+#ffff6666/ABar-s<$+#ffcc3333/ABpl-s<$+#ff4b6efc/ABpr-s<$+#ff0034ff/view/time=294/rX=0.0/rY=0.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/Android/"};

   
    // 

    private string[] colorSchemeNames = new string[] {"Tour of the Tract and Simplified Single Cell Shape Model",
     "Lineage/Spatial Relationships"};

    private string colorSchemeNameForExternalURL = "External Color URL";

    private string colorSchemeBaseURL = "http://scene.wormguides.org/wormguides/testurlscript?/set/";

    public enum CS { TourTract_NerveRing, LineageSpatialRelationships };

    private CS cs;

    // constructor called when a color scheme URL is appended to the base URL of the webpage
    public ColorScheme(string externalURL, Dropdown colorSchemeDropdown)
    {
        // handle the addition of the URL by adding it to the list of default URLs and setting its index as the starting index

        // first, format the external URL (it is assumed that it does not contain the base)
        externalURL = colorSchemeBaseURL + externalURL;
        Debug.Log("Made external URL: " + externalURL);

        // add the new url to the lists
        colorURLS = new string[] { colorURLS[0], colorURLS[1], externalURL };
        colorSchemeNames = new string[] { colorSchemeNames[0], colorSchemeNames[1], colorSchemeNameForExternalURL };

        colorSchemeDropdown.ClearOptions();
        colorSchemeDropdown.AddOptions(new List<string>(colorSchemeNames));

        lastIDX = 2;

        setColorScheme(lastIDX);
    }

    public ColorScheme(int idx, Dropdown colorSchemeDropdown)
    {
        colorSchemeDropdown.ClearOptions();
        colorSchemeDropdown.AddOptions(new List<string>(colorSchemeNames));

        setColorScheme(idx);
    }

    public CS getColorScheme()
    {
        return this.cs;
    }

    public void setColorScheme(int idx)
    {
        // parse the rules and build the results into an easily indexable dictionary
        currentColorSchemeDict = unpackRulesIntoExhaustiveList(UrlParser.parseUrlRules(colorURLS[idx]));
    }

    public Dictionary<string, int> getCurrentColorSchemeDict()
    {
        return this.currentColorSchemeDict;
    }

    public Color getColorByIndex(int idx)
    {
        return currentColorList[idx];
    }

    private Dictionary<string, int> unpackRulesIntoExhaustiveList(List<List<string>> colorSchemeAsLists)
    {
        Dictionary<string, int> exhaustiveRulesDict = new Dictionary<string, int>();

        // error handling
        if (colorSchemeAsLists.Count != 4
            || colorSchemeAsLists[0].Count != colorSchemeAsLists[1].Count
            || colorSchemeAsLists[0].Count != colorSchemeAsLists[2].Count
            || colorSchemeAsLists[0].Count != colorSchemeAsLists[3].Count)
        {
            Debug.Log("non-parallel rule lists: " + colorSchemeAsLists[0].Count + ", " + colorSchemeAsLists[1].Count + ", " + colorSchemeAsLists[2].Count + ", " + colorSchemeAsLists[3].Count);
            return exhaustiveRulesDict;
        }

        // set the list of colors in the class for reference
        currentColorList = buildCurrentColorList(colorSchemeAsLists[3]);

        // iterate over rules, unpack rules into single entity names and corresponding color, and build the dictionary
        for (int i = 0; i < colorSchemeAsLists[0].Count; i++)
        {
            // get the entity name
            string entityName = colorSchemeAsLists[0][i];
            string ruleType = colorSchemeAsLists[1][i];
            string ruleOptions = colorSchemeAsLists[2][i];
            int colorIdx = i;

            // LINEAGE RULE UNPACKING
            if (ruleType.ToLower().Contains("l")) // lineage rule
            {
                // if ancestors or descendants are include, this will require a search
                if (ruleOptions.ToLower().Contains("a"))
                {
                    List<string> specialCases = SulstonLineage.getSpecialCases();

                    for (int k = entityName.Length; k >= 0; k--)
                    {
                        string substr = entityName.Substring(0, k);
                        if (specialCases.Contains(substr.ToUpper()))
                        {
                            // add the special case
                            if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                            {
                                exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                            }

                            // add the ancestors of the special case
                            foreach (string ancestor in SulstonLineage.getAncestorsOfSpecialCase(substr))
                            {
                                if (!exhaustiveRulesDict.ContainsKey(ancestor.ToLower()))
                                {
                                    exhaustiveRulesDict.Add(ancestor.ToLower(), colorIdx);
                                }
                            }

                            break; // at this point all of the ancestors have been added. move on
                        }

                        // add the substring to the results list (if we've reached here, the substring isn't a special case 
                        // and each letter we peel off if a ancestor by C elegans lineage naming convention
                        if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                        {
                            exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                        }
                    }
                }
                else if (ruleOptions.ToLower().Contains("d"))
                {
                    // add the entityName itself
                    if (!exhaustiveRulesDict.ContainsKey(entityName.ToLower()))
                    {
                        exhaustiveRulesDict.Add(entityName.ToLower(), colorIdx);
                    }

                    // first check if this is a special case
                    List<string> specialCases = SulstonLineage.getSpecialCases();

                    if (specialCases.Contains(entityName.ToUpper()))
                    {
                        // add all of the descendants in the special case tree
                        List<string> cases = SulstonLineage.getDescendantsOfSpecialCase(entityName);
                        if (cases.Count == 0) // this is already a leaf node, add its descendants directly
                        {
                            // query all of the descendants from this special case leaf and add them to the dictionary
                            List<string> specialCaseDescendants = addAllDescendants(entityName);
                            foreach (string s in specialCaseDescendants)
                            {
                                if (!exhaustiveRulesDict.ContainsKey(s.ToLower()))
                                {
                                    exhaustiveRulesDict.Add(s.ToLower(), colorIdx);
                                }
                            }
                        } else // it's not a leaf node, so query its special case descendants and add all of those's descendants
                        {
                            foreach (string descendant in cases)
                            {
                                if (!exhaustiveRulesDict.ContainsKey(descendant.ToLower()))
                                {
                                    exhaustiveRulesDict.Add(descendant.ToLower(), colorIdx);
                                }


                                // to tell if this descendant is a leaf node, see if it's descendants list is empty
                                if (SulstonLineage.getDescendantsOfSpecialCase(descendant).Count == 0)
                                {
                                    // query all of the descendants from this special case leaf and add them to the dictionary
                                    List<string> specialCaseDescendants = addAllDescendants(descendant);
                                    foreach (string s in specialCaseDescendants)
                                    {
                                        if (!exhaustiveRulesDict.ContainsKey(s.ToLower()))
                                        {
                                            exhaustiveRulesDict.Add(s.ToLower(), colorIdx);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // standard lineage name, so add all of its descendants through the standard C elegans lineage string building algorithm
                        List<string> descendants = addAllDescendants(entityName);
                        foreach (string s in descendants)
                        {
                            if (!exhaustiveRulesDict.ContainsKey(s.ToLower()))
                            {
                                exhaustiveRulesDict.Add(s.ToLower(), colorIdx);
                            }
                        }
                    }
                }
            }



            // FUNCTIONAL RULE UNPACKING
            else if (ruleType.ToLower().Equals("f")) // functional
            {
                List<string> functionalNames = PartsList.getFunctionalNames();
                bool isTerminalNameRoot = false;
                List<string> matchingLineageNames = new List<string>(); // if the terminal name for this rule is either a root of a class of terminal names
                                                                        // e.g. hyp, or the terminal name is full but has multiple lineage names associated with it
                                                                        // e.g. int or hyp7, we will build up a list of all of the lineage names that correspond 
                for (int k = 0; k < functionalNames.Count; k++)
                {
                    string s = functionalNames[k];
                    if (s.ToLower().StartsWith(entityName.ToLower()) || s.ToLower().Equals(entityName.ToLower())) {
                        // we've got a match - check if it is exact or partial by checking the length
                        if (s.Length != entityName.Length)
                        {
                            // they're different lengths, so the entity name is the root of a terminal name
                            isTerminalNameRoot = true; // this will be set true multiple times during this loop if it's true once - that's okay

                            // add the lineage names matching the terminal name which starts with the entity name in the rule
                            matchingLineageNames.AddRange(PartsList.getLineageNamesByTerminalName(s));
                        }
                        else
                        {
                            // even if they're the same length, there still be many lineage names associated with this functional name e.g. 'int'
                            List<string> lineageNames = PartsList.getLineageNamesByTerminalName(s);
                            for (int j = 0; j < lineageNames.Count; j++)
                            if (!exhaustiveRulesDict.ContainsKey(lineageNames[j].ToLower()))
                            {
                                exhaustiveRulesDict.Add(lineageNames[j].ToLower(), colorIdx);
                            }

                            break;
                        }
                    }
                }

                // remove any duplicates in matchingLineageNames
                matchingLineageNames = matchingLineageNames.Distinct().ToList();

                // so if we do have an entity name that is a root of a terminal name, the above loop should have built up a list of 
                // the different full terminal names that have that root. Let's add them now
                foreach(string s in matchingLineageNames)
                {
                    if (!exhaustiveRulesDict.ContainsKey(s.ToLower()))
                    {
                        exhaustiveRulesDict.Add(s.ToLower(), colorIdx);
                    }
                }

                if (ruleOptions.ToLower().Contains("a"))
                {
                    List<string> specialCases = SulstonLineage.getSpecialCases();

                    // need to check if the entityName is a full terminal name (e.g. hyp7) or a terminal name root (hyp)
                    if (isTerminalNameRoot)
                    {
                        // again, if this is true, there should be a list of all lineage names that have that functional name root. Apply the ancestry naming
                        // algorithm to each of those names
                        foreach(string lineageName in matchingLineageNames)
                        {

                            for (int k = lineageName.Length; k >= 0; k--)
                            {
                                string substr = lineageName.Substring(0, i);
                                if (specialCases.Contains(substr.ToUpper()))
                                {
                                    // add the special case
                                    if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                                    {
                                        exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                                    }

                                    // add the ancestors of the special case
                                    foreach (string ancestor in SulstonLineage.getAncestorsOfSpecialCase(substr))
                                    {
                                        if (!exhaustiveRulesDict.ContainsKey(ancestor.ToLower()))
                                        {
                                            exhaustiveRulesDict.Add(ancestor.ToLower(), colorIdx);
                                        }
                                    }

                                    break; // at this point all of the ancestors have been added. move on
                                }

                                // add the substring to the results list (if we've reached here, the substring isn't a special case 
                                // and each letter we peel off is an ancestor by C elegans lineage naming convention
                                if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                                {
                                    exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                                }
                            }
                        }

                    }
                    /*
                    else // we've got a full name so just do the standard procedure for this terminal name on its lineage name
                    {
                        string lineageName = PartsList.getLineageNameByTerminalName(entityName);
                        for (int k = lineageName.Length; k >= 0; k--)
                        {
                            string substr = lineageName.Substring(0, i);
                            if (specialCases.Contains(substr))
                            {
                                // add the special case
                                if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                                {
                                    exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                                }

                                // add the ancestors of the special case
                                foreach (string ancestor in SulstonLineage.getAncestorsOfSpecialCase(substr))
                                {
                                    if (!exhaustiveRulesDict.ContainsKey(ancestor.ToLower()))
                                    {
                                        exhaustiveRulesDict.Add(ancestor.ToLower(), colorIdx);
                                    }
                                }

                                break; // at this point all of the ancestors have been added. move on
                            }

                            // add the substring to the results list (if we've reached here, the substring isn't a special case 
                            // and each letter we peel off is an ancestor by C elegans lineage naming convention
                            if (!exhaustiveRulesDict.ContainsKey(substr.ToLower()))
                            {
                                exhaustiveRulesDict.Add(substr.ToLower(), colorIdx);
                            }
                        } 
                    } */
                }
            }
        }

        return exhaustiveRulesDict;
    }

    private List<string> addAllDescendants(string searchString)
    {
        List<string> results = new List<string>();

        // add descendants that follow the standard C elegans naming convention
        List<string> lineageNames = PartsList.getLineageNames();
        // for every match found, generate all descendants between the search string and the match
        foreach (string s in lineageNames)
        {
            if (s.ToLower().StartsWith(searchString.ToLower()))
            {
                for (int k = searchString.Length + 1; k <= s.Length; k++)
                {
                    results.Add(s.Substring(0, k));
                }
            }
        }

        // do the same with the cell deaths
        List<string> cellDeaths = CellDeaths.getCellDeaths();
        foreach (string s in cellDeaths)
        {
            if (s.ToLower().StartsWith(searchString.ToLower()))
            {
                for (int k = searchString.Length + 1; k <= s.Length; k++)
                {
                    results.Add(s.Substring(0, k));
                }
            }
        }

        return results;
    }

    private List<Color> buildCurrentColorList(List<string> hexCodesList)
    {
        List<Color> colorList = new List<Color>();
        foreach(string hex in hexCodesList)
        {
            colorList.Add(buildColorFromHex(hex));
        }
        return colorList;
    }



    private Color buildColorFromHex(string ruleColorHex)
    {
        Color c = new Color();
        if (ruleColorHex != null)
        {
            if (ColorUtility.TryParseHtmlString(("#" + ruleColorHex), out c))
            {
                return c;
            }
        }

        // color could not be built, log error and apply default color in its place
        Debug.Log("color could not be built for rule. Color string is: '" + ruleColorHex + "'. Using default color in its place");

        // gray color with ~rgb(211, 211, 211) and alpha = 0.3
        return new Color(0.82f, 0.82f, 0.82f, 0.3f);
    }
}