using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

using System.IO;

public class ProductionInfoLoader {

	private static int NUMBER_OF_FIELDS = 17;
	private static string PRODUCTION_INFO_FILE_PATH = "production_info_file/Production_Info";
	//private static string PRODUCTION_INFO_FILE_PATH = "Assets/wormguides/models/production_info_file/Production_Info.csv";
	private static string PRODUCTION_INFO_LINE = "Production Information,,,,,,,,,,,,,,,,";
	private static string HEADER_LINE = "Cells,Image Series,Marker,Strain,Compressed Embryo?,Temporal "
	                                    + "Resolution,Segmentation,cytoshow link,Movie start timeProperty (min),isSulstonMode?,Total Time Points,"
	                                    + "X_SCALE,Y_SCALE,Z_SCALE,Key_Frames_Rotate,Key_Values_Rotate,Initial_Rotation";

	public static List<List<string>> buildProductionInfo ()
	{

		// initialize lists
		List<List<string>> productionInfo = new List<List<string>> ();

		// process prod info file
		TextAsset file = Resources.Load (PRODUCTION_INFO_FILE_PATH) as TextAsset;
		if (file != null) {
			List<string> cells = new List<string> ();
			List<string> imageSeries = new List<string> ();
			List<string> markers = new List<string> ();
			List<string> strains = new List<string> ();
			List<string> compressedEmbryo = new List<string> ();
			List<string> temporalResolutions = new List<string> ();
			List<string> segmentations = new List<string> ();
			List<string> cytoshowLinks = new List<string> ();
			List<string> movieStartTime = new List<string> ();
			List<string> isSulston = new List<string> ();
			List<string> totalTimePoints = new List<string> ();
			List<string> xScale = new List<string> ();
			List<string> yScale = new List<string> ();
			List<string> zScale = new List<string> ();
			List<string> keyFramesRotate = new List<string> ();
			List<string> keyValuesRotate = new List<string> ();
			List<string> initialRotation = new List<string> ();


			string filestream = file.text;
			string[] fLines = Regex.Split (filestream, "\n|\r|\r\n");

			for (int i = 0; i < fLines.Length; i++) {
				string line = fLines [i];

				// skip product info line and header line
				if (line.Equals (PRODUCTION_INFO_LINE)) {
					line = fLines [++i];
					if (line.Equals (HEADER_LINE)) {
						line = fLines [++i];
					}
				}

				// make sure we've arrived at a valid line
				if (line.Length <= 1) {
					break;
				}

				// tokenize the line
				string[] values = line.Split (',');

				if (values.Length == NUMBER_OF_FIELDS) {
					cells.Add (values [0]);
					imageSeries.Add (values [1]);
					markers.Add (values [2]);
					strains.Add (values [3]);
					compressedEmbryo.Add (values [4]);
					temporalResolutions.Add (values [5]);
					segmentations.Add (values [6]);
					cytoshowLinks.Add (values [7]);
					movieStartTime.Add (values [8]);
					isSulston.Add (values [9]);
					totalTimePoints.Add (values [10]);
					xScale.Add (values [11]);
					yScale.Add (values [12]);
					zScale.Add (values [13]);
					keyFramesRotate.Add (values [14]);
					keyValuesRotate.Add (values [15]);
					initialRotation.Add (values [16]);
				}
			}

			// add lists to production info
			productionInfo.Add (cells);
			productionInfo.Add (imageSeries);
			productionInfo.Add (markers);
			productionInfo.Add (strains);
			productionInfo.Add (compressedEmbryo);
			productionInfo.Add (temporalResolutions);
			productionInfo.Add (segmentations);
			productionInfo.Add (cytoshowLinks);
			productionInfo.Add (movieStartTime);
			productionInfo.Add (isSulston);
			productionInfo.Add (totalTimePoints);
			productionInfo.Add (xScale);
			productionInfo.Add (yScale);
			productionInfo.Add (zScale);
			productionInfo.Add (keyFramesRotate);
			productionInfo.Add (keyValuesRotate);
			productionInfo.Add (initialRotation);
		}
		return productionInfo;
	}
}










//		string FilePath = PRODUCTION_INFO_FILE_PATH;
//		Debug.Log (FilePath);
//		if (File.Exists (FilePath)) {
//			using (var fs = File.OpenRead (FilePath))
//			using (var reader = new StreamReader (fs)) {
//				while (!reader.EndOfStream) {
//					string line = reader.ReadLine ();
//
//					// skip product info line and header line
//					if (line.Equals (PRODUCTION_INFO_LINE)) {
//						line = reader.ReadLine ();
//						if (line.Equals (HEADER_LINE)) {
//							line = reader.ReadLine ();
//						}
//					}
//
//					// make sure we've arrived at a valid line
//					if (line.Length <= 1) {
//						break;
//					}
//
//					// tokenize the line
//					string[] values = line.Split (',');
//
//					if (values.Length == NUMBER_OF_FIELDS) {
//						cells.Add (values [0]);
//						imageSeries.Add (values [1]);
//						markers.Add (values [2]);
//						strains.Add (values [3]);
//						compressedEmbryo.Add (values [4]);
//						temporalResolutions.Add (values [5]);
//						segmentations.Add (values [6]);
//						cytoshowLinks.Add (values [7]);
//						movieStartTime.Add (values [8]);
//						isSulston.Add (values [9]);
//						totalTimePoints.Add (values [10]);
//						xScale.Add (values [11]);
//						yScale.Add (values [12]);
//						zScale.Add (values [13]);
//						keyFramesRotate.Add (values [14]);
//						keyValuesRotate.Add (values [15]);
//						initialRotation.Add (values [16]);
//					}
//				}
//			}
//
//			// add lists to production info
//			productionInfo.Add(cells);
//			productionInfo.Add (imageSeries);
//			productionInfo.Add (markers);
//			productionInfo.Add (strains);
//			productionInfo.Add (compressedEmbryo);
//			productionInfo.Add (temporalResolutions);
//			productionInfo.Add (segmentations);
//			productionInfo.Add (cytoshowLinks);
//			productionInfo.Add (movieStartTime);
//			productionInfo.Add (isSulston);
//			productionInfo.Add (totalTimePoints);
//			productionInfo.Add (xScale);
//			productionInfo.Add (yScale);
//			productionInfo.Add (zScale);
//			productionInfo.Add (keyFramesRotate);
//			productionInfo.Add (keyValuesRotate);
//			productionInfo.Add (initialRotation);
//
//			Debug.Log (productionInfo.Count);
//		} else {
//			Debug.Log ("didn't find prod info file");
//		}
//
//		return productionInfo;
//	} 
//}