using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CellDeaths {

	private static string CellDeathsFile = "celldeaths_file/CellDeaths";
    private static List<string> cellDeaths;

    public static void init()
    {
        cellDeaths = new List<string>();
        TextAsset file = Resources.Load(CellDeathsFile) as TextAsset;
        if (file != null)
        {
            string filestream = file.text;
            string[] fLines = Regex.Split(filestream, "\n");

            for (int i = 0; i < fLines.Length; i++)
            {
                string line = fLines[i];

                if (line != null && line.Length != 0)
                {
                    cellDeaths.Add(line.Trim());
                }
            }
        }
    }

    public static List<string> getCellDeaths()
    {
        return cellDeaths;
    }


}
