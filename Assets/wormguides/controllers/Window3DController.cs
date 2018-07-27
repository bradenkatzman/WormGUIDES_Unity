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
	private BillboardsList billboardsList;

	// subscene state parameters
	private List<string> cellNames;
	private List<GameObject> spheres;
	private List<double[]> positions;
	private List<double> diameters;
	private List<Material> cellSphereMaterials;

	private List<GameObject> meshes;
	private List<string> meshNames;
	private List<SceneElement> sceneElementsAtCurrentTime;
	private List<GameObject> currentSceneElementMeshes;
	private List<SceneElement> currentSceneElements;
	private List<Billboard> billboardsAtCurrentTime;
	private List<GameObject> currentBillboardTextMeshes;

	private int xScale, yScale, zScale;
	private int offsetX, offsetY, offsetZ;

	private Camera PerspectiveCam;

	// helper vars
	private int X_COR_IDX = 0;
	private int Y_COR_IDX = 1;
	private int Z_COR_IDX = 2;

	// context menu
//	private bool hasAVL;
//	private Vector3 avlPos;

	/*
	 * Cells or keywords with associated color rules
	 */ 
			// 1. Tract Tour, Nerve Ring
	private string[] TractTour_NerveRing_rule_cells = new string[]{
		"ABplpaaaaaa", "ABplppaaaaa", "ABprppaaaaa", "ABprpappaap", "ABprpapppap", "ABalpapaaaa", "ABarappaaaa", "ABprpappaaa",
		"ABplpapappa", "ABprpapappa", "ABplpapaaaa", "ABprpapaaaa",  "ABplpapaapa", "ABprpapaapa",  "ABplpapaapp", "ABprpapaapp",
		"ABprpaaaaaa", "ABalppappaa", "ABarappppaa", "ABplpaapaaa", "ABprpaapaaa", "unc-86_outgrowth"};

	private bool[] TractTour_NerveRing_rule_cells_CELLONLY = new bool[]{
		false, false, false, false, false, false, false, false,
		true, true, true, true, true, true, true, true, true,
		true, true, true, true, false};

			// 2. Lineage and Spatial Relationships (color only applies to primitive cell shapes
	private string[] LineageSpatialRelationships_rule_cells = new string[] {
		"E", "MS", "D", "C", "P4", "ABal", "ABar", "ABpl", "ABpr", "Z2", "Z3"};

			// 3. Neuronal Cell Positions
	private string[] NeuronalCellPositions_keywords = new string[] {
		"sheath", "socket", "sensory", "interneuron", "motor"};
			// 4. Tissue Types
	private string[] TissueTypes_keywords = new string[] {
		"muscle", "intestinal", "marginal", "vulva", "interneuron", "motor", "sensory", "pore", "duct",
		"gland", "excretory", "hypoderm", "seam", "socket", "sheath", "valve", "arcade", "epithelium", 
		"rectal", "head"};
	// ** end cells/keywords

	// string vars
	private string[] tract_names = new string[]{
		"nerve_ring_anterior", "ventral_sensory_left", "ventral_sensory_right", "nerve_ring_left",
		"nerve_ring_left_base", "amphid_right", "amphid_left", "nerve_ring_right_base",
		"nerve_ring_right", "VNC_left", "VNC_right"};

	private string[] billboard_misc_geometry_names = new string[]{
		"Arrow"};


	/*
	 * Color Schemes
	 */ 
	private ColorScheme CS;
			// 1. Tract Tour, Nerve Ring
	private Material[] TractTour_NerveRing_rule_materials;


			// 2. Lineage and Spatial Relationships
	private Material[] LineageSpatialRelationships_rule_materials;

			// 3. Neuronal Cell Positions
	private Material[] NeuronalCellPositions_rule_materials;

			// 4. Tissue Types
	private Material[] TissueTypes_rule_materials;

			//defaults
	private Material[] DefaultMaterials;
	private int DEFAULT_MATERIAL_IDX = 0;
	private int DEFAULT_MATERIAL_TRACTS_IDX = 1;
	private int MISCELLANEOUS_GEOMETRY_MATERIAL_IDX = 2;
	// ** end color schemes

	// billboard stuff
	private static string BillboardStr = "Billboard";
	private static string SPACE = " ";

	public Window3DController(int xS, int yS, int zS, 
		LineageData ld, SceneElementsList elementsList, BillboardsList bl,
		Camera pc,
		int offX, int offY, int offZ,
		Material[] Tt_Nr_materials,
		Material[] Lsr_materials,
		Material[] ncp_materials,
		Material[] tt_materials,
		Material[] defMaterials,
		ColorScheme cs_) {
		this.xScale = xS;
		this.yScale = yS;
		this.zScale = zS;

		this.lineageData = ld;
		this.sceneElementsList = elementsList;
		this.billboardsList = bl;

		this.PerspectiveCam = pc;

		this.offsetX = offX;
		this.offsetY = offY;
		this.offsetZ = offZ;

		this.TractTour_NerveRing_rule_materials = Tt_Nr_materials;
		this.LineageSpatialRelationships_rule_materials = Lsr_materials;
		this.NeuronalCellPositions_rule_materials = ncp_materials;
		this.TissueTypes_rule_materials = tt_materials;
		this.DefaultMaterials = defMaterials;
//		this.textMaterial = tMaterial;

		this.CS = cs_;

		// temp context menu vars
		//this.hasAVL = false;

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

		// temp
//		this.hasAVL = false;
//		this.ContextMenu.SetActive (false);

		// clear note sprites and overlays here if they are integrated at some point

		// handle orientation indicator rotation here
	}

	private void getSceneData(int time) {
		// get cell names, positions, diameters and spheres
		cellNames = new List<string>(lineageData.getNames(time));

		if (CS.getColorScheme ().Equals (ColorScheme.CS.TourTract_NerveRing)) {
			cellSphereMaterials = lineageData.getMaterials (time, ColorScheme.CS.TourTract_NerveRing);
		} else if (CS.getColorScheme ().Equals (ColorScheme.CS.LineageSpatialRelationships)) {
			cellSphereMaterials = lineageData.getMaterials (time, ColorScheme.CS.LineageSpatialRelationships);
		} else if (CS.getColorScheme ().Equals (ColorScheme.CS.NeuronalCellPositions)) {
			cellSphereMaterials = lineageData.getMaterials (time, ColorScheme.CS.NeuronalCellPositions);
		} else if (CS.getColorScheme ().Equals (ColorScheme.CS.TissueTypes)) {
			cellSphereMaterials = lineageData.getMaterials (time, ColorScheme.CS.TissueTypes);
		}


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
			float[] arrow_pos = new float[3];
			// determine if this billboard has geometry instead of text
			bool miscGeometry = false;
			foreach (string name in billboard_misc_geometry_names) {
				if (name.ToLower ().Equals (b.getBillboardText ().ToLower ())) {
					b_GO = GeometryLoader.loadObj (billboardsList.getMiscGeoPathStr() + b.getBillboardText ());
					if (b_GO != null) {
						miscGeometry = true;
						if (b.getBillboardText ().ToLower ().Equals ("Arrow".ToLower ())) {
							GameObject arrow_child = b_GO.transform.GetChild (0).gameObject;
							arrow_child.transform.localScale += new Vector3 (9, 9, 9);
							arrow_child.transform.eulerAngles = new Vector3 (0, 0, 90);
							foreach (Renderer rend in b_GO.GetComponentsInChildren<Renderer>()) {
								rend.material = DefaultMaterials [MISCELLANEOUS_GEOMETRY_MATERIAL_IDX];
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
				//b_GO.GetComponentInChildren<MeshRenderer> ().material = textMaterial;
			}

			if (b.getAttachmentType ().Equals (BillboardAttachmentType.AttachmentType.Static)) {
				if (b.getBillboardText().ToLower().Equals ("Nose Tip".ToLower())) {
					float[] xyzLocation = b.getXYZLocation ();
					b_GO.transform.position = new Vector3 (
						xyzLocation [X_COR_IDX] + BillboardsList.NOSE_TIP_OFFSET_X,
						xyzLocation [Y_COR_IDX] + BillboardsList.NOSE_TIP_OFFSET_Y,
						xyzLocation [Z_COR_IDX] + BillboardsList.NOSE_TIP_OFFSET_Z
					);
				} else {
					float[] xyzLocation = b.getXYZLocation ();
					b_GO.transform.position = new Vector3 (
						xyzLocation [X_COR_IDX],
						xyzLocation [Y_COR_IDX],
						xyzLocation [Z_COR_IDX]
					);

					if (b.getBillboardText ().ToLower ().Equals ("Arrow".ToLower ())) {
						arrow_pos = xyzLocation;
					}
				}
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

//		if (hasAVL) {
//			ContextMenu.transform.position = new Vector3 (avlPos.x + 25.0f, avlPos.y - 15.0f, avlPos.z);
//			ContextMenu.SetActive (true);
//
//		}
	}

	private void addCellGeometries() {
		for (int i = 0; i < cellNames.Count; i++) {
			string cellName = cellNames [i];

			// size of the sphere
			float radius = (float) diameters[i] / 2.0f;

			// create primitive
			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);

			// add radius as scale
			sphere.transform.localScale = new Vector3 (radius, radius, radius);

			// set the position of the sphere
			double[] position = positions[i];
			sphere.transform.position = new Vector3 (
				(float) -position [X_COR_IDX] * xScale,
				(float) position [Y_COR_IDX] * yScale,
				(float) position [Z_COR_IDX] * zScale);

			sphere.transform.RotateAround (Vector3.zero, Vector3.forward, 180);

			// set cell name
			sphere.name = cellName;

			// add color
			sphere.GetComponent<Renderer> ().material = cellSphereMaterials[i];

//			if (cellName.ToLower().Equals("ABprpappaap".ToLower())) {
//				this.hasAVL = true;
//				this.avlPos = sphere.transform.position;
//			}

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
			if (CS.getColorScheme ().Equals (ColorScheme.CS.TourTract_NerveRing)) {
				for (int k = 0; k < TractTour_NerveRing_rule_cells.Length; k++) {
					if (go.name.ToLower ().Equals (TractTour_NerveRing_rule_cells [k].ToLower ()) && !TractTour_NerveRing_rule_cells_CELLONLY [k]) {
						// add the color
						hasColor = true;

						// need to add the material to all of the components
						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
							rend.material = TractTour_NerveRing_rule_materials [k];
						}
					}
				}
			} else if (CS.getColorScheme ().Equals (ColorScheme.CS.LineageSpatialRelationships)) {
				for (int k = 0; k < LineageSpatialRelationships_rule_cells.Length; k++) {
					if (go.name.ToLower ().StartsWith (LineageSpatialRelationships_rule_cells [k].ToLower ())) {
						hasColor = true;
						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
							rend.material = LineageSpatialRelationships_rule_materials [k];
						}
					}
				}
			} //else if (CS.getColorScheme ().Equals (ColorScheme.CS.NeuronalCellPositions)) {
//				for (int k = 0; !hasColor && k < NeuronalCellPositions_keywords.Length; k++) {
//					int descrMatchResults = PartsList.findDescriptionMatch (go.name, NeuronalCellPositions_keywords [k]);
//
//					if (descrMatchResults == 0) { // pure clone, give full color
//						hasColor = true;
//						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
//							rend.material = NeuronalCellPositions_rule_materials [k];
//						}
//					} else if (descrMatchResults > 0) { // parent of pure clone, give weighted avg color
//						hasColor = true;
//						string weightedHex = WeightedAvgHexcode.computeWeightedAverageHexcode(
//							new string[]{ColorUtility.ToHtmlStringRGBA(NeuronalCellPositions_rule_materials[k].color),
//								ColorUtility.ToHtmlStringRGBA(DefaultMaterials[DEFAULT_MATERIAL_IDX].color)},
//							new float[]{(1.0f/(float)(descrMatchResults + 1)),
//								(((float)(descrMatchResults))/((float)(descrMatchResults + 1)))});
//						Color weightedColor = new Color ();
//						bool success = ColorUtility.TryParseHtmlString (weightedHex, out weightedColor);
//						if (success) {
//							foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
//								go.GetComponent<Renderer> ().material.color = weightedColor;
//							}
//						} else {
//							Debug.Log ("failed with " + weightedHex);
//						}
//					}
//				}
//			} else if (CS.getColorScheme ().Equals (ColorScheme.CS.TissueTypes)) {
//				for (int k = 0; !hasColor && k < TissueTypes_keywords.Length; k++) {
//					int descrMatchResults = PartsList.findDescriptionMatch (go.name, TissueTypes_keywords [k]);
//
//					if (descrMatchResults == 0) { // pure clone, give full color
//						hasColor = true;
//						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
//							rend.material = TissueTypes_rule_materials [k];
//						}
//					} else if (descrMatchResults > 0) { // parent of pure clone, give weighted avg color
//						hasColor = true;
//						string weightedHex = WeightedAvgHexcode.computeWeightedAverageHexcode(
//							new string[]{ColorUtility.ToHtmlStringRGBA(TissueTypes_rule_materials[k].color),
//								ColorUtility.ToHtmlStringRGBA(DefaultMaterials[DEFAULT_MATERIAL_IDX].color)},
//							new float[]{(1.0f/(float)(descrMatchResults + 1)),
//								(((float)(descrMatchResults))/((float)(descrMatchResults + 1)))});
//						Color weightedColor = new Color ();
//						bool success = ColorUtility.TryParseHtmlString (weightedHex, out weightedColor);
//						if (success) {
//							foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
//								go.GetComponent<Renderer> ().material.color = weightedColor;
//							}
//						} else {
//							Debug.Log ("failed with " + weightedHex);
//						}
//					}
//				}
//			}


			if (!hasColor) {
				bool isTract = false;
				for (int k = 0; k < tract_names.Length; k++) {
					if (go.name.ToLower ().Equals (tract_names [k].ToLower ())) {
						isTract = true;
						foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
							rend.material = DefaultMaterials [DEFAULT_MATERIAL_TRACTS_IDX];
						}
						break;
					}
				}
				if (!isTract) {
					foreach(Renderer rend in go.GetComponentsInChildren<Renderer>()) {
						rend.material = DefaultMaterials[DEFAULT_MATERIAL_IDX];
					}
				}
			}

			meshes.Add (go);
		}
	}

	public void updateColorScheme(int ColorScheme_IDX, GameObject WormGUIDES_Unity) {
		this.CS.setColorScheme (ColorScheme_IDX);

		GameObject reg = renderScene (ApplicationModel.getTime());
		
		reg.transform.parent = WormGUIDES_Unity.transform;
	}

	public GameObject getRootEntitiesGroup() {
		return rootEntitiesGroup;
	}

	public void Update() {
		foreach (GameObject b_GO in currentBillboardTextMeshes) {
			//Debug.Log ("looking toward VR cam");
			b_GO.transform.LookAt (PerspectiveCam.transform);
			b_GO.transform.Rotate(new Vector3(0, 180, 0));
		}
	}
}