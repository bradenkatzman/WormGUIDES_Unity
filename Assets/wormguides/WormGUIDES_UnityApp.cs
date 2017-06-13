using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Main script for WormGUIDES_Unity
 * 
 * This script is run at startup and facilitates construction
 * of a WormGUIDES_Unity instance
 * */
public class WormGUIDES_UnityApp : MonoBehaviour {

	private RootLayoutController rlc;

	// hide/show time control button
	public Button HideShow_Control_Button;

	// Time control UI elements
	public GameObject Control_Panel;
	public Slider timeSlider;
	public Button backwardButton;
	public Button playPauseButton;
	public Button forwardButton;
	public Text timeText;
	public Button switchCameras;
	public Dropdown colorSchemeDropdown;

	// context menu
	public GameObject ContextMenu;

	// camera stuff
	public GameObject GvrMain;
	public Camera PerspectiveCam;

	// color scheme enum
	private ColorScheme CS;

	// materials for color schemes
	public Material[] TractTour_NerveRing_rule_materials;
	public Material[] LineageSpatialRelationships_rule_materials;
	public Material[] NeuronalCellPositions_rule_materials;
	public Material[] TissueTypes_rule_materials;
	public Material[] DefaultMaterials;

	public Material TextMaterial;

	// rotation
	private Gyroscope gyro;

	private string REG = "Root Entities Group";

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");

		// first set the selected camera mode
		if (ApplicationModel.getCameraMode () == 0) {
			this.PerspectiveCam.enabled = false;
			this.GvrMain.SetActive(true);
		} else if (ApplicationModel.getCameraMode () == 1) {
			this.GvrMain.SetActive(false);
			this.PerspectiveCam.enabled = true;
		}

		// enable and set the 
		Input.gyro.enabled = true;
		this.gyro = Input.gyro;

		CS = new ColorScheme (ColorScheme.CS.TourTract_NerveRing);
		//CS = new ColorScheme (ColorScheme.CS.LineageSpatialRelationships);
		initRootLayout ();
	}

	void Update() {
		// add rotation of scene based on gyroscrope if in perspective mode
		Transform regTransform = transform.Find(REG);
		if (regTransform != null && !GvrMain.activeSelf && PerspectiveCam.enabled && gyro.enabled) {
			//Debug.Log ("rotating with: " + gyro.attitude.ToString ());
			regTransform.transform.rotation = gyro.attitude;
		}
	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
		rlc.setUIElements (this.HideShow_Control_Button, this.Control_Panel,
			this.timeSlider, this.backwardButton, this.playPauseButton, this.forwardButton,
			this.timeText, this.switchCameras, this.colorSchemeDropdown, this.ContextMenu);
		rlc.addCameras (this.GvrMain, this.PerspectiveCam);
		rlc.setColorScheme (this.CS);
		rlc.setGyro (this.gyro);
	}

	public GameObject getWormGUIDES_Unity() {
		return this.gameObject;
	}

	public Material[] getTractTourNerveRingRuleMaterials() {
		return this.TractTour_NerveRing_rule_materials;
	}

	public Material[] getLineageSpatialRelationshipsRuleMaterials() {
		return this.LineageSpatialRelationships_rule_materials;
	}

	public Material[] getNeuronalCellPositionsRuleMaterials() {
		return this.NeuronalCellPositions_rule_materials;
	}

	public Material[] getTissueTypesRuleMaterials() {
		return this.TissueTypes_rule_materials;
	}

	public Material[] getDefaultMaterials() {
		return this.DefaultMaterials;
	}

	public Material getTextMaterial() {
		return this.TextMaterial;
	}
} 