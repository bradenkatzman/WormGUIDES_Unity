﻿using System.Collections;
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
	private BillboardsList billboardsList;

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
	private List<Billboard> billboardsAtCurrentTime;
	private List<GameObject> currentBillboardTextMeshes;

	private int xScale, yScale, zScale;
	private int offsetX, offsetY, offsetZ;

	private GameObject GvrMain;
	private Camera PerspectiveCam;

	// helper vars
	private int X_COR_IDX = 0;
	private int Y_COR_IDX = 1;
	private int Z_COR_IDX = 2;

	// list of the cells that have color rules associated with them
	private string[] rule_cells = new string[]{
		"ABplpaaaaaa", "ABplppaaaaa", "ABprppaaaaa", "ABprpappaap", "ABprpapppap", "ABalpapaaaa", "ABarappaaaa", "ABprpappaaa",
		"ABplpapappa", "ABprpapappa", "ABplpapaaaa", "ABprpapaaaa",  "ABplpapaapa", "ABprpapaapa",  "ABplpapaapp", "ABprpapaapp",
		"ABprpaaaaaa", "ABalppappaa", "ABarappppaa", "ABplpaapaaa", "ABprpaapaaa"};

	private bool[] rule_cells_CELLONLY = new bool[]{
		false, false, false, false, false, false, false, false,
		true, true, true, true, true, true, true, true, true,
		true, true, true, true};

	private string[] tract_names = new string[]{
		"nerve_ring_anterior", "ventral_sensory_left", "ventral_sensory_right", "nerve_ring_left",
		"nerve_ring_left_base", "amphid_right", "amphid_left", "nerve_ring_right_base",
		"nerve_ring_right", "VNC_left", "VNC_right"};

	private string[] billboard_misc_geometry_names = new string[]{
		"Arrow"};


	private Material[] rule_materials;
	private int DEFAULT_MATERIAL_IDX = 21;
	private int DEFAULT_MATERIAL_TRACTS_IDX = 22;
	private int MISCELLANEOUS_GEOMTRY_IDX = 23;
	private static string BillboardStr = "Billboard";
	private static string SPACE = " ";

	// 
	public Window3DController(int xS, int yS, int zS, 
		LineageData ld, SceneElementsList elementsList, BillboardsList bl,
		GameObject vrCam, Camera persCam,
		int offX, int offY, int offZ,
		Material[] materials) {
		this.xScale = xS;
		this.yScale = yS;
		this.zScale = zS;

		this.lineageData = ld;
		this.sceneElementsList = elementsList;
		this.billboardsList = bl;

		this.GvrMain = vrCam;
		this.PerspectiveCam = persCam;

		this.offsetX = offX;
		this.offsetY = offY;
		this.offsetZ = offZ;

		this.rule_materials = materials;

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
		billboardsAtCurrentTime = new List<Billboard> ();
		currentBillboardTextMeshes = new List<GameObject> ();

		rootEntitiesGroup = new GameObject ();
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
		for (int i = 0; i < sceneElementsAtCurrentTime.Count; i++) {
			SceneElement se = sceneElementsAtCurrentTime [i];
			GameObject go = se.buildGeometry (time - 1);
			if (go != null) {
				go.transform.RotateAround (Vector3.zero, Vector3.forward, 180);
				go.transform.Translate (new Vector3 (offsetX, -offsetY, (-offsetZ * zScale)));
				currentSceneElementMeshes.Add (go);
				currentSceneElements.Add (se);
			} else {
				//Debug.Log (se.getSceneName () + " has no geometry. Removing name from meshNames");
				if (meshNames.Contains (se.getSceneName ())) {
					meshNames.Remove (se.getSceneName ());
				} else if (se.getAllCells ().Count != 0 && meshNames.Contains(se.getAllCells () [0])) {
					meshNames.Remove (se.getAllCells () [0]);
				}
			}
		}

		meshes = new List<GameObject> ();

		if (!(currentBillboardTextMeshes.Count == 0)) {
			currentBillboardTextMeshes.Clear ();
		}

		billboardsAtCurrentTime = billboardsList.getBillboardsAtTime (time, cellNames);
		for (int i = 0; i < billboardsAtCurrentTime.Count; i++) {
			Billboard b = billboardsAtCurrentTime [i];

			GameObject b_GO = null;
			// determine if this billboard has geometry instead of text
			bool miscGeometry = false;
			foreach (string name in billboard_misc_geometry_names) {
				if (name.ToLower ().Equals (b.getBillboardText ().ToLower ())) {
					b_GO = GeometryLoader.loadObj (billboardsList.getMiscGeoPathStr() + b.getBillboardText ());
					if (b_GO != null) {
						miscGeometry = true;

						if (b.getBillboardText () == "Arrow") {
							GameObject arrow_child = b_GO.transform.GetChild (0).gameObject;
							arrow_child.transform.localScale += new Vector3 (9, 9, 9);
							arrow_child.transform.eulerAngles = new Vector3 (0, 0, 90);
							foreach (Renderer rend in b_GO.GetComponentsInChildren<Renderer>()) {
								rend.material = rule_materials [MISCELLANEOUS_GEOMTRY_IDX];
							}
						}
					}
				}
			}

			// create text billboard if standard billboard
			if (!miscGeometry) {
				b_GO = new GameObject ();
				b_GO.name = b.getBillboardText () + SPACE + BillboardStr;
				TextMesh tm = b_GO.AddComponent<TextMesh> ();
				tm.text = b.getBillboardText ();
				tm.fontSize = billboardsList.getDefaultFontSize ();
			}

			if (b.getAttachmentType ().Equals (BillboardAttachmentType.AttachmentType.Static)) {
				float[] xyzLocation = b.getXYZLocation ();
				b_GO.transform.position = new Vector3 (
					xyzLocation[X_COR_IDX],
					xyzLocation[Y_COR_IDX],
					xyzLocation[Z_COR_IDX]
				);
			} else if (b.getAttachmentType ().Equals (BillboardAttachmentType.AttachmentType.Cell)) {
				// find the position of the attachment cell
				int idx = cellNames.IndexOf(billboardsAtCurrentTime[i].getAttachmentCell());
				double[] position = positions [idx];
				b_GO.transform.position = new Vector3 (
					((float) -position [X_COR_IDX] * xScale) + billboardsList.getXOffset(),
					((float) position [Y_COR_IDX] * yScale) + billboardsList.getYOffset(),
					((float) position [Z_COR_IDX] * zScale) + billboardsList.getZOffset());
				b_GO.transform.RotateAround (Vector3.zero, Vector3.forward, 180);
				b_GO.transform.RotateAround (b_GO.transform.position, Vector3.forward, 180);
				//b_GO.transform.Rotate(new Vector3(0,0,180));
			}

			currentBillboardTextMeshes.Add (b_GO);
		}
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
		foreach (GameObject textMesh in currentBillboardTextMeshes) {
			textMesh.transform.parent = rootEntitiesGroup.transform;
		}
	}

	private void addCellGeometries() {
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
				(float) -position [X_COR_IDX] * xScale,
				(float) position [Y_COR_IDX] * yScale,
				(float) position [Z_COR_IDX] * zScale);

			sphere.transform.RotateAround (Vector3.zero, Vector3.forward, 180);

			sphere.name = cellName;

			// add color
			bool hasColor = false;
			for (int k = 0; k < rule_cells.Length; k++) {
				if (sphere.name.ToLower ().Equals (rule_cells[k].ToLower ())) {
					// add the color
					hasColor = true;
					sphere.GetComponent<Renderer>().material = rule_materials[k];
				}
			}

			if (!hasColor) {
				sphere.GetComponent<Renderer>().material = rule_materials[DEFAULT_MATERIAL_IDX];
			}

			// add sphere to list
			spheres.Add(sphere);
		}
	}

	private void addSceneElementGeometries() {
		SceneElement se;
		GameObject go;
		for (int i = 0; i < currentSceneElements.Count; i++) {
			se = currentSceneElements [i];
			go = currentSceneElementMeshes [i];

			go.name = meshNames [i];

			// rule and coloring stuff will happen here

			// add color
			bool hasColor = false;
			for (int k = 0; k < rule_cells.Length; k++) {
				if (go.name.ToLower ().Equals (rule_cells[k].ToLower ()) && !rule_cells_CELLONLY[k]) {
					// add the color
					hasColor = true;

					// need to add the material to all of the components
					foreach(Renderer rend in go.GetComponentsInChildren<Renderer>()) {
							rend.material = rule_materials[k];
					}
				}
			}

			if (!hasColor) {
				bool isTract = false;
				for (int k = 0; k < tract_names.Length; k++) {
					if (go.name.ToLower ().Equals (tract_names [k].ToLower ())) {
						isTract = true;
						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
							rend.material = rule_materials [DEFAULT_MATERIAL_TRACTS_IDX];
						}
						break;
					}
				}
				if (!isTract) {
					foreach(Renderer rend in go.GetComponentsInChildren<Renderer>()) {
						rend.material = rule_materials[DEFAULT_MATERIAL_IDX];
					}
				}
			}

			meshes.Add (go);
		}
	}

	private GameObject getRootEntitiesGroup() {
		return rootEntitiesGroup;
	}


	public void Update() {
		// if there are billboards, continuously make them face the active camera
		foreach (GameObject b_GO in currentBillboardTextMeshes) {
			// make billboard front facing
			if (GvrMain.activeSelf) {
				//Debug.Log ("looking toward VR cam");
				b_GO.transform.LookAt (GvrMain.transform);
			} else if (PerspectiveCam.enabled) {
				//Debug.Log ("looking toward Perspective Cam");
				b_GO.transform.LookAt (PerspectiveCam.transform);
			}

			b_GO.transform.Rotate(new Vector3(0, 180, 0));
		}
	}
}