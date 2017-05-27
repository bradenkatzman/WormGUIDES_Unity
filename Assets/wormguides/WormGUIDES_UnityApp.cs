﻿using System.Collections;
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
	public Material[] DefaultMaterials;

	public Material TextMaterial;

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");

		CS = new ColorScheme (ColorScheme.CS.TourTract_NerveRing);
		//CS = new ColorScheme (ColorScheme.CS.LineageSpatialRelationships);
		initRootLayout ();
	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
		rlc.setUIElements (this.HideShow_Control_Button, this.Control_Panel,
			this.timeSlider, this.backwardButton, this.playPauseButton, this.forwardButton,
			this.timeText, this.switchCameras, this.colorSchemeDropdown, this.ContextMenu);
		rlc.addCameras (this.GvrMain, this.PerspectiveCam);
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

	public Material[] getDefaultMaterials() {
		return this.DefaultMaterials;
	}

	public Material getTextMaterial() {
		return this.TextMaterial;
	}
} 