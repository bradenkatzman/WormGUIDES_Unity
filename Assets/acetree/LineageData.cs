using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineageData {

	private List<Frame> timeFrames;
	private List<string> allCellNames;
	private double[] xyzScale;
	private bool isSulston;
	private bool finishedConstruction;

	public LineageData () {
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
	  
	// 
	public void shiftAllPositions(int x, int y, int z) {
		foreach (Frame timeFrame in timeFrames) {
			timeFrame.shiftPositions(x, y, z);
		}
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

//		return timeFrames [0].getNames ();
		return timeFrames [internalTimeIdx].getNames ();
	}

	/*
	 * 
	 */ 
	public double[][] getPositions(int time) {
		int internalTimeIdx = time - 1;
		if (internalTimeIdx >= getNumberOfTimePoints () || internalTimeIdx < 0) {
			double[][] blank = new double[1][];
			blank [0] = new double[3];
			return blank;
		}

		//return timeFrames [0].getPositions ();
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

//		return timeFrames [0].getDiameters ();
		return timeFrames [internalTimeIdx].getDiameters ();
	}

	public List<Material> getMaterials(int time, ColorScheme.CS cs) {
		int internalTimeIdx = time - 1;
		if (internalTimeIdx >= getNumberOfTimePoints () || internalTimeIdx < 0) {
			return new List<Material>();
		}

		return timeFrames [internalTimeIdx].getMaterialsByColorSchemIDX (cs);
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
		private List<Material> TractTour_NerveRing_materials;
		private List<Material> LineageSpatialRelationships_materials;
		private List<Material> NeuronalCellPositions_materials;
		private List<Material> TissueTypes_materials;

		public Frame() {
			names = new List<string>();
			positions = new List<double[]>();
			diameters = new List<double>();
			TractTour_NerveRing_materials = new List<Material>();
			LineageSpatialRelationships_materials = new List<Material>();
			NeuronalCellPositions_materials = new List<Material>();
			TissueTypes_materials = new List<Material>();
		}

		// 
		public void shiftPositions(int x, int y, int z) {
			for (int i = 0; i < positions.Count; i++) {
				double[] pos = positions [i];
				positions[i] = new double[] {pos[0] - x, pos[1] - y, pos[2] - z};
			}
		}

		// 
		public void addName(string name) {
			names.Add (name);
		}

		// 
		public void addPosition(double[] position) {
			positions.Add (position);
		}

		// 
		public void addDiameter(double diameter) {
			diameters.Add (diameter);
		}

		// 
		public string[] getNames() {
			return names.ToArray ();
		}


		// 
		public double[][] getPositions() {
			double[][] copy = new double[positions.Count][];
			for (int i = 0; i < positions.Count; i++) {
				copy [i] = new double[3];
				for (int j = 0; j < 3; j++) {
					copy [i] [j] = (positions [i]) [j];
				}
			}

			return copy;
		}

		// 
		public double[] getDiameters() {
			double[] copy = new double[diameters.Count];
			for (int i = 0; i < diameters.Count; i++) {
				copy [i] = diameters [i];
			}

			return copy;
		}

		public List<Material> getMaterialsByColorSchemIDX(ColorScheme.CS cs) {
			if (cs.Equals(ColorScheme.CS.TourTract_NerveRing)) {
				return this.TractTour_NerveRing_materials;
			} else if (cs.Equals(ColorScheme.CS.LineageSpatialRelationships)) {
				return this.LineageSpatialRelationships_materials;
			}
			return new List<Material> ();
		}
	}
}