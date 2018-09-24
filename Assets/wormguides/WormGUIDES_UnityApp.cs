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
	public Material[] DefaultMaterials;

	public Material TextMaterial;

	private string REG = "Root Entities Group";

    public TextMesh entityLabel;

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");

        // -1 sets the target frame rate to configure to the optimal rate based on the platform. This is useful for optimizing
        // perfomance across different browers. See more here: https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html

        // using other values to test performance
        Application.targetFrameRate = 1;

        PartsList.initPartsList();
        ProductionInfo.initProductionInfo();


		CS = new ColorScheme (0);
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
        rlc.setEntityLabel(entityLabel);
	}

	public GameObject getWormGUIDES_Unity() {
		return this.gameObject;
	}

	public Material[] getDefaultMaterials() {
		return this.DefaultMaterials;
	}

	public Material getTextMaterial() {
		return this.TextMaterial;
	}
} 