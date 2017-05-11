using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeometryLoader {

	private static string SLASH = "/";
	private static string T = "_t";

	/*
	 * 
	 */
	public static int getEffectiveStartTime(
		string resourcePath,
		int startTime,
		int endTime) {

		resourcePath = SLASH + resourcePath;

		TextAsset file = Resources.Load(resourcePath) as TextAsset;

		if (file != null) {
			// check for obj file with a time
			for (int time = startTime; time <= endTime; time++) {
				TextAsset f = Resources.Load((resourcePath + T + time.ToString())) as TextAsset;

				if (f != null) {
					return time;
				}
			}
		}

		return Int32.MinValue;
	}

	public static GameObject loadObj(string resourcePath) {
		var obj = Resources.Load (resourcePath);
		if (obj != null) {
			return GameObject.Instantiate (obj) as GameObject;
		}
		return null;
	}
}
