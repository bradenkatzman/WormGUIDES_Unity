using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using System.IO;

public class LineageDataLoader {

	private static string ENTRY_PREFIX = "nucleifiles/";
	private static string T = "t";
	private static string ENTRY_EXT = "-nuclei";
	private static string SLASH = "/";
	private static int NUMBER_OF_TOKENS = 21;
	private static int VALID_IDX = 1,
			XCOR_IDX = 5,
			YCOR_IDX = 6,
			ZCOR_IDX = 7,
			DIAMETER_IDX = 8,
			ID_IDX = 9;
	private static string ONE_ZERO_PAD = "0";
	private static string TWO_ZERO_PAD = "00";
	private static int X_POS_IDX = 0;
	private static int Y_POS_IDX = 1;
	private static int Z_POS_IDX = 2;
	private static List<string> allCellNames = new List<string> ();
	private static int avgX;
	private static int avgY;
	private static int avgZ;

	/*
	 * 
	 */ 
	public static LineageData loadNucFiles(ProductionInfo productionInfo, GameObject WormGUIDES_Unity) {
		// initialize lineage data
		LineageData ld = new LineageData();
		ld.setAllCellNames (allCellNames);
		ld.setXYZScale (productionInfo.getXScale (), productionInfo.getYScale (), productionInfo.getZScale ());

		string urlStr;
		for (int i = 1; i <= productionInfo.getTotalTimePoints (); i++) {
			urlStr = getResourceAtTime (i);
			if (urlStr != null) {
				process (ld, i, urlStr);
			} else {
				Debug.Log ("Could not find file: " + urlStr);
			}
		}

		// translate all cells to center around (0,0,0)
		setOriginToZero(ld);

		return ld;
	}

	private static string getResourceAtTime(int i) {
		string resourceUrlStr = null;
		if (i >= 1) {
			if (i < 10) {
				resourceUrlStr = ENTRY_PREFIX + T + TWO_ZERO_PAD + i.ToString () + ENTRY_EXT;
			} else if (i < 100) {
				resourceUrlStr = ENTRY_PREFIX + T + ONE_ZERO_PAD + i.ToString () + ENTRY_EXT;
			} else {
				resourceUrlStr = ENTRY_PREFIX + T + i.ToString () + ENTRY_EXT;
			}
		}

		return resourceUrlStr;
	}

	private static void process(LineageData ld, int time, string FilePath) {
		ld.addTimeFrame ();

		TextAsset file = Resources.Load (FilePath) as TextAsset;
		if (file != null) {

			string filestream = file.text;
			string[] fLines = Regex.Split (filestream, "\n|\r|\r\n");


			for (int i = 0; i < fLines.Length; i++) {
				string line = fLines [i];

				string[] tokens = new string[NUMBER_OF_TOKENS];
				string[] values = line.Split (',');

				if (values.Length == NUMBER_OF_TOKENS) {
					int k = 0;
					for (int j = 0; j < values.Length; j++) {
						tokens [k++] = values [j].Trim ();
					}

					if ((int)Int32.Parse (tokens [VALID_IDX]) == 1) {
						makeNucleus (ld, time, tokens);
					}
				}
			}
		} else {
			Debug.Log ("couldn't find file: " + FilePath);
		}
	}

				

//		using (var fs = File.OpenRead (FilePath))
//		using (var reader = new StreamReader (fs)) {
//			while (!reader.EndOfStream) {
//				string line = reader.ReadLine ();
//				if (line != null) {
//					string[] tokens = new string[NUMBER_OF_TOKENS];
//					string[] values = line.Split (',');
//
//					int k = 0;
//					for (int i = 0; i < values.Length; i++) {
//						tokens [k++] = values [i].Trim();
//					}
//
//					if ((int)Int32.Parse (tokens [VALID_IDX]) == 1) {
//						makeNucleus (ld, time, tokens);
//					}
//				}
//			}
//		}
	//}

	private static void makeNucleus(LineageData ld, int time, string[] tokens) {
		ld.addNucleus (
			time,
			tokens [ID_IDX],
			(int)Int32.Parse (tokens [XCOR_IDX]),
			(int)Int32.Parse (tokens [YCOR_IDX]),
			Math.Round (Double.Parse (tokens [ZCOR_IDX])),
			(int)Int32.Parse (tokens [DIAMETER_IDX]));
	}

	public static void setOriginToZero(LineageData ld) {
		int totalPositions = 0;
		double sumX = 0.0;
		double sumY = 0.0;
		double sumZ = 0.0;

		// sum up all x-, y- and z-coordinates of nuclei
		for (int i = 0; i < ld.getNumberOfTimePoints (); i++) {
			double[][] positionsArray = ld.getPositions (i);
			for (int j = 1; j < positionsArray.Length; j++) {
				sumX += positionsArray [j][X_POS_IDX];
				sumY += positionsArray [j][Y_POS_IDX];
				sumZ += positionsArray [j][Z_POS_IDX];
				totalPositions++;
			}
		}

		// find average of x-, y- and z-coordinates
		avgX = (int) (sumX / totalPositions);
		avgY = (int) (sumY / totalPositions);
		avgZ = (int) (sumZ / totalPositions);

		Debug.Log ("Average nuclei position offsets from origin (0, 0, 0): (" + 
			string.Format ("{0}, ", avgX) +
			string.Format ("{0}, ", avgY) +
			string.Format ("{0})", avgZ));

		ld.shiftAllPositions (avgX, avgY, avgZ);
	}

	public static int getAvgXOffsetFromZero() {
		return avgX;
	}

	public static int getAvgYOffsetFromZero() {
		return avgY;
	}

	public static int getAvgZOffsetFromZero() {
		return avgZ;
	}
}