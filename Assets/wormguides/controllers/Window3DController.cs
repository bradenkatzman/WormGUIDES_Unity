using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window3DController {

	// this is the game object that will hold all of the geometry in the scene
	private GameObject rootEntitiesGroup;

	// for rendering
	private NucleiLoader nucLdr;
	private GeometryLoader geoLdr;

	private LineageData lineageData;
	private SceneElementsList sceneElementsList;

	// subscene state parameters
	private List<string> cellNames;
	private List<GameObject> spheres;
	private List<double[]> positions;
	private List<double> diameters;

	private List<GameObject> meshes;
	private List<string> meshNames;
	private List<SceneElement> sceneElementsAtCurrentTime;
	private List<GameObject> currentSceneElementMeshes;
	private List<SceneElement> currentSceneElements;

	private int xScale;
	private int yScale;
	private int zScale;


	// helper vars
	private int X_COR_IDX = 0;
	private int Y_COR_IDX = 1;
	private int Z_COR_IDX = 2;

	// 
	public Window3DController(int xS, int yS, int zS, 
		LineageData ld, SceneElementsList elementsList) {
		this.xScale = xS;
		this.yScale = yS;
		this.zScale = zS;

		this.lineageData = ld;
		this.sceneElementsList = elementsList;

		// initialize
		spheres = new List<GameObject>();
		meshes = new List<GameObject> ();
		cellNames = new List<string> ();
		meshNames = new List<string> ();
		positions = new List<double[]> ();
		diameters = new List<double> ();
		sceneElementsAtCurrentTime = new List<SceneElement> ();
		currentSceneElementMeshes = new List<GameObject> ();
		currentSceneElements = new List<SceneElement> ();

		rootEntitiesGroup = new GameObject ();
	}


	public void setLineageData(LineageData ld) {
		
	}

	// called by RootLayoutController to render the scene
	public GameObject renderScene(int time) {
		refreshScene ();
		getSceneData (time);
		addEntities ();
		return getRootEntitiesGroup ();
	}

	private void refreshScene() {
		// clear stuff
		GameObject.Destroy(rootEntitiesGroup);
		rootEntitiesGroup = new GameObject ();
		rootEntitiesGroup.name = "Root Entities Group";

		// clear note sprites and overlays here if they are integrated at some point

		// handle orientation indicator rotation here
	}

	private void getSceneData(int time) {
		// get cell names, positions, diameters and spheres
		cellNames = new List<string>(lineageData.getNames(time));

		positions = new List<double[]>();
		foreach (double[] position in lineageData.getPositions (time)) {
			positions.Add(new double[]{ 
				position [0], 
				position [1], 
				position [2] });
		}

		diameters = new List<double>();
		foreach (double diameter in lineageData.getDiameters(time)) {
			diameters.Add (diameter);
		}

		spheres = new List<GameObject> ();


		// populate the scene elements list
		if (sceneElementsList != null) {
			meshNames = new List<string> (sceneElementsList.getSceneElementNamesAtTime (time));
		}

		if (!(currentSceneElementMeshes.Count == 0)) {
			currentSceneElementMeshes.Clear ();
			currentSceneElements.Clear ();
		}

		sceneElementsAtCurrentTime = sceneElementsList.getSceneElementsAtTime (time);
		foreach (SceneElement se in sceneElementsAtCurrentTime) {
			GameObject go = se.buildGeometry (time - 1);
			if (go != null) {
				currentSceneElementMeshes.Add (go);
				currentSceneElements.Add (se);
			}
		}

		meshes = new List<GameObject> ();
	}

	private void addEntities() {
		addCellGeometries ();
		addSceneElementGeometries ();
		foreach (GameObject sphere in spheres) {
			sphere.transform.parent = rootEntitiesGroup.transform;
		}
		foreach (GameObject mesh in meshes) {
			mesh.transform.parent = rootEntitiesGroup.transform;
		}
	}

	private void addCellGeometries() {
		Debug.Log (cellNames.Count);
		for (int i = 0; i < cellNames.Count; i++) {
			string cellName = cellNames [i];

			// size of the sphere
			float radius = (float) diameters[i] / 2.0f;

			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere.transform.localScale = new Vector3 (radius, radius, radius);

			// add rotate values to spheres

			// set the position of the sphere
			double[] position = positions[i];
			sphere.transform.position = new Vector3 (
				(float) position [X_COR_IDX] * xScale,
				(float) position [Y_COR_IDX] * yScale,
				(float) position [Z_COR_IDX] * zScale);
			
			// add sphere to list
			spheres.Add(sphere);
		}
	}

	private void addSceneElementGeometries() {
		Debug.Log (currentSceneElementMeshes.Count);
		SceneElement se;
		GameObject go;
		int idx = -1;
		for (int i = 0; i < currentSceneElements.Count; i++) {
			idx++;
			se = currentSceneElements [i];
			go = currentSceneElementMeshes [i];

			// rule and coloring stuff will happen here

			meshes.Add (go);
		}
	}

	private GameObject getRootEntitiesGroup() {
		return rootEntitiesGroup;
	}
}