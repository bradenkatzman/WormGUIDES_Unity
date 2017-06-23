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
	public GameObject Gvr_EmbryoCenter;
	public Camera PerspectiveCam;
	public Vector3 Gvr_EmbryoCenter_Transform_InitialPosition;
	public Vector3 Gvr_EmbryoCenter_Transform_InitialRotation;

	// color scheme enum
	private ColorScheme CS;

	// materials for color schemes
	public Material[] TractTour_NerveRing_rule_materials;
	public Material[] LineageSpatialRelationships_rule_materials;
	public Material[] NeuronalCellPositions_rule_materials;
	public Material[] TissueTypes_rule_materials;
	public Material[] DefaultMaterials;

	public Material TextMaterial;

	private string REG = "Root Entities Group";

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");

		// first set the selected camera mode
		if (ApplicationModel.getCameraMode () == 0) {
			//this.PerspectiveCam.enabled = false;
//			this.Gvr_Perspective.SetActive(false);
//			this.Gvr_EmbryoCenter.SetActive(true);

			// default, do nothing

		} else if (ApplicationModel.getCameraMode () == 1) {
//			this.Gvr_EmbryoCenter.SetActive(false);
//			this.Gvr_Perspective.SetActive (true);
			//this.PerspectiveCam.enabled = true;

			// move the Gvr camera to the location of the perspective game
			this.Gvr_EmbryoCenter.transform.position = PerspectiveCam.transform.position;
			this.Gvr_EmbryoCenter.transform.rotation = PerspectiveCam.transform.rotation;
		}

		CS = new ColorScheme (ColorScheme.CS.TourTract_NerveRing);
		//CS = new ColorScheme (ColorScheme.CS.LineageSpatialRelationships);

		initRootLayout ();
	}

	// TODO
	void Update() {
		// add rotation of scene based on gyroscrope if in perspective mode
		Transform reg = transform.Find(REG);
		if (reg != null && !Gvr_EmbryoCenter.transform.position.Equals (Gvr_EmbryoCenter_Transform_InitialPosition)) {
			reg.transform.rotation = Quaternion.Inverse(ApplicationModel.getGvrHeadRot ());
		}
	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
		rlc.setUIElements (this.HideShow_Control_Button, this.Control_Panel,
			this.timeSlider, this.backwardButton, this.playPauseButton, this.forwardButton,
			this.timeText, this.switchCameras, this.colorSchemeDropdown, this.ContextMenu);
		rlc.addCameras (this.Gvr_EmbryoCenter, this.PerspectiveCam, 
			this.Gvr_EmbryoCenter_Transform_InitialPosition, this.Gvr_EmbryoCenter_Transform_InitialRotation);
		rlc.setColorScheme (this.CS);
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