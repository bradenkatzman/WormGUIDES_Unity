using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElement {
	private static string MULTICELL_TRACT = "MCS";
	private string sceneName;
	private List<string> cellNames;
	private string markerName;
	private string embryoName;
	private string imagingSource;
	private string resourceLocation;
	private int startTime;
	private int endTime;
	private string comments;
	private bool completeResourceFlag;
	private int x,y,z;

	/**
     * Constructor
     *
     * @param sn
     *         the scene name
     * @param cn
     *         the cells contained in the structure
     * @param mn
     *         the marker name
     * @param is_
     *         the imagine source
     * @param rl
     *         resource specifying the .obj file location
     * @param st
     *         the first time point in which this structure appears
     * @param et
     *         the last time point in which this structure appears
     * @param c
     *         the structure comments
     */
	public SceneElement(
		string sn,
		List<string> cn,
		string mn,
		string is_,
		string rl,
		int st,
		int et,
		string c) {

		this.sceneName = sn;
		this.cellNames = cn;
		this.markerName = mn;
		this.imagingSource = is_;
		this.resourceLocation = rl;
		this.startTime = st;
		this.endTime = et;
		this.comments = c;

		this.embryoName = "";
		this.completeResourceFlag = isResourceComplete ();

		// make sure that lineage names that start with "AB" have the proper casing
		List<string> editedNames = new List<string>();
		string lineagePrefix = "ab";
		string name;
		string namePrefix;
		foreach (string cellName in cellNames) {
			if (cellName.Length > 2) {
				namePrefix = cellName.Substring (0, 2);
				if (namePrefix.StartsWith(lineagePrefix)) {
					editedNames.Add("AB" + cellName.Substring(2));
					cellNames.Remove(cellName);
				}
			}
		}

		cellNames.AddRange(editedNames);
	}

	private bool isResourceComplete() {
		return resourceLocation.EndsWith (".obj");
	}

	public GameObject buildGeometry(int time) {
		if (completeResourceFlag) {
			return GeometryLoader.loadObj (resourceLocation);
		}

		return GeometryLoader.loadObj (resourceLocation + "_t" + time);
	}

	public void setNewCellNames(List<string> cells) {
		this.cellNames.Clear ();
		this.cellNames = cells;
	}

	public int getX() {
		return this.x;
	}

	public void setX(int x_) {
		this.x = x_;
	}

	public int getY() {
		return this.y;
	}

	public void setY(int y_) {
		this.y = y_;
	}

	public int getZ() {
		return this.z;
	}

	public void setZ(int z_) {
		this.z = z_;
	}

	public void setLocation(int x_, int y_, int z_) {
		this.x = x_;
		this.y = y_;
		this.z = z_;
	}

	public void setMarker(string marker) {
		if (marker != null) {
			this.markerName = marker;
		}
	}

	public void addCellnames(string name) {
		if (name != null) {
			this.cellNames.Add (name);
		}
	}

	public string getSceneName() {
		return this.sceneName;
	}

	public void setSceneName(string name) {
		if (name != null) {
			this.sceneName = name;
		}
	}

	public List<string> getAllCells() {
		if (cellNames.Count > 0 && cellNames[0].ToLower().Equals(MULTICELL_TRACT.ToLower())) {
			return cellNames.GetRange (1, cellNames.Count - 2);
		}

		return cellNames;
	}

	public bool isMulticellular() {
		return cellNames.Count > 1 || (cellNames.Count > 0 && cellNames[0].ToLower().Equals(MULTICELL_TRACT.ToLower()));
	}

	public bool isNoCellStructure() {
		return cellNames.Count == 0;
	}

	public bool existsAtTime(int time) {
		return this.startTime <= time && time <= this.endTime;
	}

	public string getMarkerName() {
		return this.markerName;
	}

	public string getEmbryoName() {
		return this.embryoName;
	}

	public void setEmbryoName(string name) {
		if (name != null) {
			this.embryoName = name;
		}
	}

	public string getImagingSource() {
		return this.imagingSource;
	}

	public void setImagingSource(string src) {
		if (src != null) {
			this.imagingSource = src;
		}
	}

	public string getResourceLocation() {
		return this.resourceLocation;
	}

	public void setResourceLocation(string location) {
		if (location != null) {
			this.resourceLocation = location;
		}
	}

	public int getStartTime() {
		return this.startTime;
	}

	public void setStartTime(int time) {
		if (-1 < time) {
			this.startTime = time;
		}
	}

	public int getEndTime() {
		return this.endTime;
	}

	public void setEndTime(int time) {
		if (-1 < time) {
			this.endTime = time;
		}
	}

	public string getComments() {
		return this.comments;
	}

	public void setComments(string comments_) {
		if (comments != null) {
			this.comments = comments_;
		}
	}

	public bool getCompleteResourceFlag() {
		return completeResourceFlag;
	}
}