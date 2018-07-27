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

	// Time control UI elements
	public GameObject Control_Panel;
	public Slider timeSlider;
	public Button backwardButton;
	public Button playPauseButton;
	public Button forwardButton;
	public Text timeText;
	public Dropdown colorSchemeDropdown;

	// camera stuff
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

	private string REG = "Root Entities Group";

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");

        PartsList.initPartsList();
        ProductionInfo.initProductionInfo();

        // first set the selected camera mode
//        if (ApplicationModel.getCameraMode () == 0) {
			//this.PerspectiveCam.enabled = false;
//			this.Gvr_Perspective.SetActive(false);
//			this.Gvr_EmbryoCenter.SetActive(true);

			// default, do nothing

//		} else if (ApplicationModel.getCameraMode () == 1) {
//			this.Gvr_EmbryoCenter.SetActive(false);
//			this.Gvr_Perspective.SetActive (true);
			//this.PerspectiveCam.enabled = true;

			// move the Gvr camera to the location of the perspective game
			//this.Gvr_EmbryoCenter.transform.position = PerspectiveCam.transform.position;
			//this.Gvr_EmbryoCenter.transform.rotation = PerspectiveCam.transform.rotation;
//		}

		CS = new ColorScheme (ColorScheme.CS.TourTract_NerveRing);
		//CS = new ColorScheme (ColorScheme.CS.LineageSpatialRelationships);

		initRootLayout ();
	}

	// TODO
	void Update() {

	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
		rlc.setUIElements (this.Control_Panel,
			this.timeSlider, this.backwardButton, this.playPauseButton, this.forwardButton,
			this.timeText, this.colorSchemeDropdown);
		rlc.addCameras (this.PerspectiveCam);
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