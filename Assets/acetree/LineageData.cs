using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineageData : MonoBehaviour {

	private List<Frame> timeFrames;
	private List<string> allCellNames;
	private double[] xyzScale;
	private bool isSulston;

	void Start () {
		timeFrames = new List<Frame> ();	
	}

	// THIS BLOCK USED AS INITIALIZATION METHODS SINCE START() CANNOT TAKE PARAMETERS
	public void setAllCellNames(List<string> names) {
		this.allCellNames = names;
	}

	public void setXYZScale(double xScale, double yScale, double zScale) {
		xyzScale = new double[] { xScale, yScale, zScale };
	}
	// END INITIALIZATION METHODS

	// TODO
	public void shiftAllPositions(int x, int y, int z) {

	}

	// ACCESSOR METHODS

	/*
	 * 
	 */
	public List<string> getAllCellNames() {
		return this.allCellNames;
	}

	/*
	 * 
	 */ 
	public string[] getNames(int time) {
		int internalTimeIdx = time - 1;
		if (internalTimeIdx >= getNumberOfTimePoints () || internalTimeIdx < 0) {
			return new string[1];
		}

		return timeFrames [internalTimeIdx].getNames ();
	}

	/*
	 * 
	 */ 
	public double[,] getPositions(int time) {
		int internalTimeIdx = time - 1;
		if (internalTimeIdx >= getNumberOfTimePoints () || internalTimeIdx < 0) {
			return new double[1, 3];
		}

		return timeFrames [internalTimeIdx].getPositions ();
	}

	/*
	 * 
	 */ 
	public double[] getDiameters(int time) {
		int internalTimeIdx = time - 1;
		if (internalTimeIdx >= getNumberOfTimePoints () || internalTimeIdx < 0) {
			return new double[1];
		}

		return timeFrames [internalTimeIdx].getDiameters ();
	}

	/*
	 *
	 */ 
	public bool isSulstonMode() {
		return isSulston;
	}

	/*
	 * 
	 */ 
	public int getNumberOfTimePoints() {
		return timeFrames.Count;
	}

	// END ACCESSOR METHODS


	// MUTATOR METHODS

	/*
	 * 
	 */ 
	public void addTimeFrame() {
		Frame frame = new Frame ();
		timeFrames.Add (frame);
	}

	/*
	 * 
	 */ 
	public void addNucleus(int time, string name, double x, double y, double z, double diameter) {
		if (time <= getNumberOfTimePoints ()) {
			int idx = time - 1;

			Frame frame = timeFrames [idx];
			frame.addName (name);
			frame.addPosition (new double[] { x, y, z });
			frame.addDiameter (diameter);

			if (!allCellNames.Contains (name)) {
				allCellNames.Add (name);
			}
		}
	}

	/*
	 * TODO
	 */ 
	public int getFirstOccurenceOf(string name) {
		return 0;
	}

	/*
	 * 
	 */ 
	public int getLastOccurentOf(string name) {
		return 0;
	}

	/*
	 * TODO
	 */ 
	public bool isCellName(string name) {
		return false;
	}

	/*
	 * 
	 */
	public void setIsSulstonModeFlag(bool isSuls) {
		this.isSulston = isSuls;
	}

	/*
	 * 
	 */
	public double[] getXYZScale() {
		return this.xyzScale;
	}
	// END MUTATOR METHODS


	/*
	* One timeframe of nuclei lineage data
	 */
	private class Frame {
		private List<string> names;
		private List<double[]> positions;
		private List<double> diameters;

		public Frame() {
			names = new List<string>();
			positions = new List<double[]>();
			diameters = new List<double>();
		}

		// TODO
		public void shiftPositions(int x, int y, int z) {

		}

		// TODO
		public void addName(string name) {

		}

		// TODO
		public void addPosition(double[] position) {

		}

		// TODO
		public void addDiameter(double diameter) {

		}

		// TODO
		public string[] getNames() {
			return new string[1];
		}


		// TODO
		public double[,] getPositions() {
			return new double[1, 3];
		}


		// TODO
		public double[] getDiameters() {
			return new double[1];
		}
	}
}