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


    private string[] tract_names = new string[]{
        "nerve_ring_anterior", "ventral_sensory_left", "ventral_sensory_right", "nerve_ring_left",
        "nerve_ring_left_base", "amphid_right", "amphid_left", "nerve_ring_right_base",
        "nerve_ring_right", "VNC_left", "VNC_right"};

    /*
	 * Color Schemes
	 */
    private ColorScheme CS;
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

        //List<List<string>> test1 = UrlParser.parseUrlRules("http://scene.wormguides.org/wormguides/testurlscript?/set/saavl-n$@+#ffff0000/sibd-n$@+#ffe6ccff/avg-n$@+#ffffff00/rip-n$@+#ff99cc99/rih-n$@+#ff99cc99/smdd-n$@+#ff4d66cc/aiy-n$@+#ffff9900/rig-n$@+#fff90557/pvt-n$@+#ffffffff/dva-n$@+#ffffb366/ala-n$@+#ff01c501/aim-n$@+#ffffff66/aial-n$@+#ff8066cc/afd-n$@+#ff4de6e2/rim-n$@+#ffb399ff/avd-n$@+#ffff9980/CEH-37=Amphid=Multicellular=Structure-H+#ff994d66/rmdd-n$@+#ff664db3/rmg-n$@+#ffffffff/ada-n$@+#ffffffff/aiz-n$@+#ffffffff/bdu-n$@+#ffffffff/smbd-n$@+#ffffffff/pha-n$@+#ffffffff/hsn-n$@+#ffffffff/phb-n$@+#ffffffff/Embryo=Outline-M+#4a00f2ff/Hypoderm-M+#033381aa/view/time=360/rX=28.0/rY=-4.0/rZ=0.0/tX=-14.0/tY=18.0/scale=2.75/dim=0.3/browser/");
        //List <List<string>> test2 = UrlParser.parseUrlRules();

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
                go.name = se.getSceneName();
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
				 
					float[] xyzLocation = b.getXYZLocation ();
					b_GO.transform.position = new Vector3 (
						xyzLocation [X_COR_IDX],
						xyzLocation [Y_COR_IDX],
						xyzLocation [Z_COR_IDX]
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

        // add transform and mesh colliders to objects
        foreach (GameObject sphere in spheres) {
            MeshCollider meshc = sphere.AddComponent(typeof(MeshCollider)) as MeshCollider;
            meshc.sharedMesh = sphere.GetComponent<MeshFilter>().sharedMesh;

            sphere.transform.parent = rootEntitiesGroup.transform;
        }
		foreach (GameObject mesh in meshes) {
            if (mesh.name != "Embryo")
            {
                if (mesh.transform.Find("tube1") != null)
                {
                    // replace the name
                    mesh.transform.Find("tube1").name = mesh.name;

                    // add the collider
                    MeshCollider meshc = mesh.transform.Find(mesh.name).gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                    meshc.sharedMesh = mesh.transform.Find(mesh.name).gameObject.GetComponent<MeshFilter>().sharedMesh;
                }
                else if (mesh.transform.Find("foo") != null)
                {
                    mesh.transform.Find("foo").name = mesh.name;
                    MeshCollider meshc = mesh.transform.Find(mesh.name).gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                    meshc.sharedMesh = mesh.transform.Find(mesh.name).gameObject.GetComponent<MeshFilter>().sharedMesh;
                }
            }

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
            foreach (List<string> rule in CS.getCurrentRulesList())
            {
               
               if (sphere.name.ToLower().Equals(rule[0].ToLower()) ||
                    PartsList.getTerminalNameByLineageName(sphere.name).ToLower().StartsWith(rule[0].ToLower()))
                {
                    //Debug.Log("Applying color " + rule[3] + " to '" + rule[0] + "'");
                    Color c = new Color();
                    if (ColorUtility.TryParseHtmlString(("#" + rule[3]), out c))
                    {
                        sphere.GetComponent<Renderer>().material.SetColor("_Color", c);
                    } else
                    {
                        Debug.Log("color didn't work");
                    }
                }

                if (rule[2].Contains("A"))
                {
                   if (rule[0].ToLower().StartsWith(sphere.name.ToLower())) {
                        Color c = new Color();
                        if (ColorUtility.TryParseHtmlString(("#" + rule[3]), out c))
                        {
                            sphere.GetComponent<Renderer>().material.SetColor("_Color", c);
                        }
                    }
                }

                if (rule[2].Contains("D"))
                {
                    // apply descendants
                    if (sphere.name.ToLower().StartsWith(rule[0].ToLower())) {
                        Color c = new Color();
                        if (ColorUtility.TryParseHtmlString(("#" + rule[3]), out c))
                        {
                            sphere.GetComponent<Renderer>().material.SetColor("_Color", c);
                        }
                    }
                }
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

			// add color
			bool hasColor = false;

            // iterate over rules and apply where applicable
            foreach (List<string> rule in CS.getCurrentRulesList())
            {
                if (go.name.ToLower().Equals("embryo")) { continue;  }
                if (go.name.ToLower().Equals(rule[0].ToLower()) || 
                    PartsList.getTerminalNameByLineageName(go.name).ToLower().StartsWith(rule[0].ToLower()))
                {
                    hasColor = true;
                    Color c = new Color();
                    if (ColorUtility.TryParseHtmlString(("#" + rule[3]), out c))
                    {
                        foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
                        {
                            rend.material.SetColor("_Color", c);
                        }
                    }
                }
            }


                //

                //if (CS.getColorScheme ().Equals (ColorScheme.CS.TourTract_NerveRing)) {
				//for (int k = 0; k < TractTour_NerveRing_rule_cells.Length; k++) {
					//if (go.name.ToLower ().Equals (TractTour_NerveRing_rule_cells [k].ToLower ())) {
						// add the color
						//hasColor = true;

						// need to add the material to all of the components
						//foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
						//	rend.material = TractTour_NerveRing_rule_materials [k];
						//}
					//}
				//}
			//} else if (CS.getColorScheme ().Equals (ColorScheme.CS.LineageSpatialRelationships)) {
			//	for (int k = 0; k < LineageSpatialRelationships_rule_cells.Length; k++) {
              //      if (go.name.ToLower().Equals("embryo")) { continue; }

			//		if (go.name.ToLower ().StartsWith (LineageSpatialRelationships_rule_cells [k].ToLower ())) {
			//			hasColor = true;
			//			foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
			//				rend.material = LineageSpatialRelationships_rule_materials [k];
			//			}
			//		}
			//	}
			//} //else if (CS.getColorScheme ().Equals (ColorScheme.CS.NeuronalCellPositions)) {
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
	
	}
}