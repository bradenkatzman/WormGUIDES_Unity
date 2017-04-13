using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeometryLoader {

	private static string OBJ_EXT = ".obj";
	private static string VERTEX_LINE = "v";
	private static string FACE_LINE = "f";
	private static string SLASH = "/";
	private static string T = "_t";
	private static string ASSETS_PATH = "Assets/";

	/*
	 * 
	 */
	public static int getEffectiveStartTime(
		string resourcePath,
		int startTime,
		int endTime) {

		resourcePath = SLASH + resourcePath;

		string FilePath = Directory.GetCurrentDirectory () + SLASH + resourcePath;
		if (File.Exists (FilePath)) {
			// check for obj file with a time
			for (int time = startTime; time <= endTime; time++) {
				string url = Directory.GetCurrentDirectory() + SLASH + resourcePath + T + time.ToString() + OBJ_EXT);

				if (File.Exists(url)) {
					return time;
				}
			}
		}

		return Int32.MinValue;
	}
}
