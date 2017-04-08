using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ProductionInfo {

	private string TRUE = "TRUE";
	private int DEFAULT_START_TIME = 1;
	private List<List<string>> productionInfoData;

	void Start () {
		
	}

	public void setProductionInfoData(List<List<string>> pid) {
		productionInfoData = pid;
	}

	// TODO
	public List<string> getNuclearInfo() {
		return new List<string> ();
	}

	/*
	 * 
	 */ 
	public bool getIsSulstonFlag() {
		return TRUE.ToLower().Equals((productionInfoData[9])[0].ToLower());
	}

	public int getTotalTimePoints() {
		return (int)Int32.Parse ((productionInfoData [10]) [0]);
	}

	public int getXScale() {
		return (int)Int32.Parse ((productionInfoData [11]) [0]);
	}

	public int getYScale() {
		return (int)Int32.Parse ((productionInfoData [12]) [0]);
	}

	public int getZScale() {
		return (int)Int32.Parse ((productionInfoData [13]) [0]);
	}

	public int getDefaultStartTime() {
		return DEFAULT_START_TIME;
	}

	// TODO
	public int getMovieTimeOffset() {
		return 0;
	}

	// TODO
	public List<string> getCellShapeData(string queryCell) {
		return new List<string> ();
	}

	// TODO
	public double[] getKeyFramesRotate() {
		return new double[3];
	}

	// TODO
	public double[] getKeyValuesRotate() {
		return new double[3];
	}

	// TOOD
	public double[] getInitialRotation() {
		return new double[3];
	}

	/*
	 * 
	 */ 
	public List<List<string>> getProductionInfoData() {
		return productionInfoData;
	}
}