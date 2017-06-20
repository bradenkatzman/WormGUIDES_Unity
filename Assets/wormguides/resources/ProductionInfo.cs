using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ProductionInfo {

	private static string TRUE = "TRUE";
	private static int DEFAULT_START_TIME = 1;
	private static List<List<string>> productionInfoData;

	public static void initProductionInfo() {
		productionInfoData = ProductionInfoLoader.buildProductionInfo ();
	}

	// TODO
	public static List<string> getNuclearInfo() {
		return new List<string> ();
	}

	/*
	 * 
	 */ 
	public static bool getIsSulstonFlag() {
		return TRUE.ToLower().Equals((productionInfoData[9])[0].ToLower());
	}

	public static int getTotalTimePoints() {
		return (int)Int32.Parse ((productionInfoData [10]) [0]);
	}

	public static int getXScale() {
		return (int)Int32.Parse ((productionInfoData [11]) [0]);
	}

	public static int getYScale() {
		return (int)Int32.Parse ((productionInfoData [12]) [0]);
	}

	public static int getZScale() {
		return (int)Int32.Parse ((productionInfoData [13]) [0]);
	}

	public static int getDefaultStartTime() {
		return DEFAULT_START_TIME;
	}

	// TODO
	public int getMovieTimeOffset() {
		return 0;
	}

	// TODO
	public static List<string> getCellShapeData(string queryCell) {
		return new List<string> ();
	}

	// TODO
	public static double[] getKeyFramesRotate() {
		return new double[3];
	}

	// TODO
	public static double[] getKeyValuesRotate() {
		return new double[3];
	}

	// TOOD
	public static double[] getInitialRotation() {
		return new double[3];
	}

	/*
	 * 
	 */ 
	public static List<List<string>> getProductionInfoData() {
		return productionInfoData;
	}
}