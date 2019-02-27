using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UrlParser
{

    // for now, this will return a nx4 list with the following contexts:
    // 1. entity name
    // 2. type (L - lineage, T - terminal, D - description, M - structure-based name)
    // 3. options (ancestor, descendant)
    // 4. color HEX
	public static List<List<string>> parseUrlRules(string url)
	{

        List<List<string>> rules = new List<List<string>>();
        List<string> ruleStrings = parseRuleArgs(url);

        //annotationManager.clearRulesList();

        List<string> types = new List<string>();
        //List<SearchOption> options = new List<SearchOption>();

        StringBuilder sb;
        bool noTypeSpecified;
        bool isMSL;
        String wholeColorString;
        String name;

        List<string> entityNames = new List<string>();
        List<string> ruleTypes = new List<string>();
        List<string> optionsList = new List<string>();
        List<string> colors = new List<string>();

        foreach (string ruleString in ruleStrings)
        {
            // temporary representation of rule
            List<string> rule = new List<string>();
            string entityName = "";
            string ruleType = "";
            string options = "";
            string color = "";
            // --------------------------------


            types.Clear();
            sb = new StringBuilder(ruleString);
            noTypeSpecified = false;
            isMSL = false;

            // determine if rule is a cell/cellbody rule, or a multicellular structure rule
            try
            {
                // multicellular rules have a null SearchType
                // parse SearchType args
                // lineage
                if (ruleString.IndexOf("-s") > -1)
                {
                    types.Add("-s");
                    ruleType = "L"; // temporary representation
                }
                // functional
                if (ruleString.IndexOf("-n") > -1)
                {
                    types.Add("-n");
                    ruleType = "F"; // temporary representation
                }
                // description - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-d") > -1)
                {
                    types.Add("-d");
                }
                // gene - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-g") > -1)
                {
                    types.Add("-g");
                }
                // multicellular structure cell-based - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-m") > -1)
                {
                    types.Add("-m");
                }
                // structure name-based
                if (ruleString.IndexOf("-M") > -1)
                {
                    types.Add("-M");
                    ruleType = "M"; // temporary representation
                }
                // structure heading-based - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-H") > -1)
                {
                    types.Add("-H");
                }
                // connectome - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-c") > -1)
                {
                    types.Add("-c");
                }
                // neighbor - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("-b") > -1)
                {
                    types.Add("-b");
                }
                // manually specified list - NOT CURRENTLY SUPPORTED
                if (ruleString.IndexOf("MSL") > -1)
                {
                    types.Clear(); // remove any other types
                    types.Add("-MSL");
                    isMSL = true;
                }

                // remove type arguments from url string
                //if (types.Count != 0)
                //{
                //    foreach (string arg in types)
                //    {
                //        int i = sb.indexOf(arg);
                //        sb.Replace(i, i + arg.Length, "");
                //    }
                //}
                //else
                //{
                //    noTypeSpecified = true;
                //    continue; // temporary --> we'll just skip the rest of the processing if there isn't a type specified
                //}

                String colorHex = "";
                double alpha = 1.0;
                if (ruleString.IndexOf("+#") > -1)
                {
                    // ff112233
                    wholeColorString = ruleString.Substring(ruleString.IndexOf("+#") + 2);
                    // whole color string format: alpha, red, green, blue
                    colorHex = wholeColorString.Substring(2);
                    String alphaHex = wholeColorString.Substring(0, 2);
                    alpha = (Convert.ToInt32(alphaHex, 16) + 1) / 256.0;
                    color = colorHex; // temporary representation
                }
                 
                //options.clear();
                int i;
                if (ruleString.IndexOf("%3E") > -1)
                {
                    //    options.add(ANCESTOR);
                    //i = sb.indexOf("%3C");
                    //sb.replace(i, i + 3, "");
                    options += "A"; // temporary representation
                }
                else if (ruleString.IndexOf(">") > -1)
                {
                    //    options.add(ANCESTOR);
                    //    i = sb.indexOf(">");
                    //    sb.replace(i, i + 1, "");
                    options += "A"; // temporary representation
                }
                //if (sb.indexOf("$") > -1)
                //{
                //    options.add(CELL_NUCLEUS);
                //    i = sb.indexOf("$");
                //   sb.replace(i, i + 1, "");
                //}
                if (ruleString.IndexOf("%3C") > -1)
                {
                    //    options.add(DESCENDANT);
                    //    i = sb.indexOf("%3E");
                    //    sb.replace(i, i + 3, "");
                    options += "D"; // temporary representation
                }
                if (ruleString.IndexOf("<") > -1)
                {
                    //    options.add(DESCENDANT);
                    //    i = sb.indexOf("<");
                    //    sb.replace(i, i + 1, "");
                    options += "D";
                }
                //if (sb.indexOf("@") > -1)
                //{
                //    options.add(CELL_BODY);
                //    i = sb.indexOf("@");
                //    sb.replace(i, i + 1, "");
                //}


                // extract name(s) from what's left of rule
                //name = ruleString.Substring(0, ruleString.IndexOf("+"));
                entityName = ruleString.Substring(0, ruleString.IndexOf("-")); // temporary representation

                // temporary representation
                if (!entityName.Equals("") && !ruleType.Equals("")
                    && !color.Equals(""))
                {
                    
                    entityNames.Add(entityName);
                    ruleTypes.Add(ruleType);
                    optionsList.Add(options);
                    colors.Add(color);
                }
            } // END TRY
            catch (System.IndexOutOfRangeException IOE)
            {
                Debug.Log("Invalid color rule format");
            }
        } // END FOREACH

        // add the compiled rule data
        // idx 0 - rule entity names
        // idx 1 - rule types
        // idx 2 - rule options
        // idx 3 - rule colors
        if (entityNames.Count != colors.Count || entityNames.Count != ruleTypes.Count || entityNames.Count != optionsList.Count)
        {
            Debug.Log("rule data arrays are not parallel");
        }

        rules.Add(entityNames);
        rules.Add(ruleTypes);
        rules.Add(optionsList);
        rules.Add(colors);

        return rules;
	}

    private static List<String> parseRuleArgs(string url)
    {
        List<string> parsedRuleArgs = new List<string>();

        if (url.Length == 0 || !url.Contains("testurlscript?/"))
        {
            return parsedRuleArgs;
        }

        string[] args = url.Split('/');

        // extract rule args
        int i = 0;
        while(!args[i].ToLower().Equals("set"))
        {
            i++;
        }

        // skip the "set" token
        i++;

        //iterate through set parameters until we hit the view parameters
        while (i < args.Length && !args[i].Equals("view"))
        {
            parsedRuleArgs.Add(args[i]);
            i++;
        }

        return parsedRuleArgs;
    }
}
