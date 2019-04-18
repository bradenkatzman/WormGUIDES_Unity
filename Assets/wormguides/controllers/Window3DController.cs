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

	private int xScale, yScale, zScale;
	private int offsetX, offsetY, offsetZ;

	private Camera PerspectiveCam;

	// helper vars
	private int X_COR_IDX = 0;
	private int Y_COR_IDX = 1;
	private int Z_COR_IDX = 2;

    // this is dual-functioning as a list of entities whose click-to-label functionality should be suppressed, but that should be updated
    private List<string> tract_names;

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
    

	public Window3DController(int xS, int yS, int zS, 
		LineageData ld, SceneElementsList elementsList,
		Camera pc,
		int offX, int offY, int offZ,
		Material[] defMaterials,
		ColorScheme cs_) {

        this.tract_names = new List<string>(new string[] {"ceh-37_amphid_left", "ceh-37_amphid_right", "lim-4_outgrowth_sibd_bundle_left", "lim-4_outgrowth_sibd_bundle_right",
            "lim-4_outgrowth_riv_bundle_right", "lim-4_outgrowth_riv_bundle_left", "lim-4_outgrowth_late", "unc-86_outgrowth",
            "nerve_ring", "nerve_ring_left_base", "nerve_ring_right_base", "amphid_left", "amphid_right",
            "nerve_ring_ventral_right", "nerve_ring_ventral_left", "vnc_left", "vnc_right", "amphid_tip_left", "amphid_tip_right", "pharynx", "embryo"});

        this.xScale = xS;
		this.yScale = yS;
		this.zScale = zS;

		this.lineageData = ld;
		this.sceneElementsList = elementsList;

		this.PerspectiveCam = pc;

		this.offsetX = offX;
		this.offsetY = offY;
		this.offsetZ = offZ;

		this.DefaultMaterials = defMaterials;

		this.CS = cs_;

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
        for (int i = 0; i < sceneElementsAtCurrentTime.Count; i++)
        {
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
	}

	private void addEntities() {
		addCellGeometries ();
		addSceneElementGeometries ();

        // add transform and mesh colliders to objects
        foreach (GameObject sphere in spheres) {
            sphere.transform.parent = rootEntitiesGroup.transform;
        }

		foreach (GameObject mesh in meshes) {
            // if this mesh is a tract, we won't put a collider on it
            if (!tract_names.Contains(mesh.name.ToLower())) {
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


            // add mesh colliders
            //MeshCollider meshc = sphere.AddComponent(typeof(MeshCollider)) as MeshCollider;
            //meshc.sharedMesh = sphere.GetComponent<MeshFilter>().sharedMesh;

            // set cell name
            sphere.name = cellName;

            // add color
            Dictionary<string, int> rulesDict = CS.getCurrentColorSchemeDict();
            bool hasColor = false;
            int colorIdx;

            // check if the entity has color
            if (rulesDict.TryGetValue(sphere.name.ToLower(), out colorIdx)) {
//                sphere.GetComponent<Renderer>().material.SetFloat("_Mode", 2);
//                sphere.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                sphere.GetComponent<Renderer>().GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                sphere.GetComponent<Renderer>().material.SetInt("_ZWrite", 0);
//                sphere.GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
//                sphere.GetComponent<Renderer>().material.EnableKeyword("_ALPHABLEND_ON");
//                sphere.GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                sphere.GetComponent<Renderer>().material.renderQueue = 3000;

                sphere.GetComponent<Renderer>().material.color = CS.getColorByIndex(colorIdx);
                hasColor = true;
            }
       
            if (!hasColor)
            {
                sphere.GetComponent<Renderer>().material = DefaultMaterials[DEFAULT_MATERIAL_IDX];

                // also turn off the mesh and sphere collider so that it is not clickable
                sphere.GetComponent<SphereCollider>().enabled = false;
                //sphere.GetComponent<MeshCollider>().enabled = false;
                
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
            Dictionary<string, int> rulesDict = CS.getCurrentColorSchemeDict();
            bool hasColor = false;
            int colorIdx;

            // iterate over rules and apply where applicable
            if (rulesDict.TryGetValue(go.name.ToLower(), out colorIdx))
            {
                foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
                {
                    // the following code snippets set the shader type to "Fade" because the pharynx and embryo 
                    // are large and hence contain other entities that should still be visible if these structures
                    // have a less than 1.0 transparency value
                    if (go.name.ToLower().Equals("embryo") || go.name.ToLower().Equals("pharynx") || go.name.ToLower().Equals("hypoderm"))
                    {
                        rend.material.SetFloat("_Mode", 2);
                        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        rend.material.SetInt("_ZWrite", 0);
                        rend.material.DisableKeyword("_ALPHATEST_ON");
                        rend.material.EnableKeyword("_ALPHABLEND_ON");
                        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        rend.material.renderQueue = 3000;
                    }

                    rend.material.color = CS.getColorByIndex(colorIdx);
                    hasColor = true;
                }
            }

			if (!hasColor) {
				bool isTract = false;
                if (tract_names.Contains(go.name.ToLower())) { 
					isTract = true;
					foreach (Renderer rend in go.GetComponentsInChildren<Renderer>()) {
						rend.material = DefaultMaterials [DEFAULT_MATERIAL_TRACTS_IDX];
					}
				}
				if (!isTract) {
					foreach(Renderer rend in go.GetComponentsInChildren<Renderer>()) {
						rend.material = DefaultMaterials[DEFAULT_MATERIAL_IDX];
					}
				}

                // turn off the mesh collider so that it can't be clicked
                foreach (MeshCollider mc in go.GetComponentsInChildren<MeshCollider>())
                {
                    mc.enabled = false;
                }
                
			}

            // the following code snippet effectively turns OFF backface culling by duplicating the triangles with
            // flipped normals. We do this for specific meshes that are large so that you see the inside if the
            // camera is inside the mesh
            if (go.name.ToLower().Equals("embryo") || go.name.ToLower().Equals("pharynx") || go.name.ToLower().Equals("hypoderm"))
            {
                go.AddComponent<MakeMeshDoubleSided>();
                
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






// -----------------------------------------------------------------------------------------------------------------------------------------
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