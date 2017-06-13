using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedAvgHexcode {

	private static int HEX_CODE_LENGTH = 8;

	// vars for indexing hex code
	private static int R_START_IDX = 0;
	private static int G_START_IDX = 2;
	private static int B_START_IDX = 4;
	private static int SINGLE_COLOR_LENGTH = 2;

	private static int ONE = 1;

	private static string ZERO = "0";

	private static string DEFAULT_HEX = "FFFFFF";

	private static string SHARP = "#";

	private static string ALPHA_VAL = "FF";

	public static string computeWeightedAverageHexcode(string[] hexCodes, float[] weights) {
		foreach (string hex in hexCodes) if (hex.Length != HEX_CODE_LENGTH) return DEFAULT_HEX;

		// first separate the two colors into 3 color numbers for R, G, B
		string[] r_strs = new string[hexCodes.Length];
		string[] g_strs = new string[hexCodes.Length];
		string[] b_strs = new string[hexCodes.Length];

		for (int i = 0; i < hexCodes.Length; i++) {
			r_strs [i] = hexCodes [i].Substring (R_START_IDX, SINGLE_COLOR_LENGTH);
			g_strs [i] = hexCodes [i].Substring (G_START_IDX, SINGLE_COLOR_LENGTH);
			b_strs [i] = hexCodes [i].Substring (B_START_IDX, SINGLE_COLOR_LENGTH);
		}

		// convert each color string into an int (specify explicitly that we are parsing a hex-based representation of a number)
		int[] r_ints = new int[hexCodes.Length];
		int[] g_ints = new int[hexCodes.Length];
		int[] b_ints = new int[hexCodes.Length];

		for (int i = 0; i < hexCodes.Length; i++) {
			r_ints [i] = System.Int32.Parse (r_strs [i], System.Globalization.NumberStyles.AllowHexSpecifier);
			g_ints [i] = System.Int32.Parse (g_strs [i], System.Globalization.NumberStyles.AllowHexSpecifier);
			b_ints [i] = System.Int32.Parse (b_strs [i], System.Globalization.NumberStyles.AllowHexSpecifier);
		}


		// calculate the weighted average
		int r_weighted, g_weighted, b_weighted;
		r_weighted = g_weighted = b_weighted = 0;

		for (int i = 0; i < hexCodes.Length; i++) {
			r_weighted += (int)(r_ints [i] * weights [i]);
			g_weighted += (int)(g_ints [i] * weights [i]);
			b_weighted += (int)(b_ints [i] * weights [i]);
		}

		// convert the weighted ints to two-digit hexadecimal strings
		string r_weighted_str, g_weighted_str, b_weighted_str;
		r_weighted_str = r_weighted.ToString ("X");
		g_weighted_str = g_weighted.ToString("X");
		b_weighted_str = b_weighted.ToString("X");

		// zero pad the hex strings when necessary
		if (r_weighted_str.Length == ONE) r_weighted_str = addZeroPad (r_weighted_str);
		if (g_weighted_str.Length == ONE) g_weighted_str = addZeroPad (g_weighted_str);
		if (b_weighted_str.Length == ONE) b_weighted_str = addZeroPad (b_weighted_str);

		// concatenate the individual codes into a single hex code, append the "FF" alpha channel
		string weightedHexCode = SHARP +
									r_weighted_str +
									g_weighted_str + 
									b_weighted_str + 
								ALPHA_VAL;

		return weightedHexCode;
	}

	private static string addZeroPad(string str) {
		return ZERO += str;
	}
}