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
	public Button HideShow_TimeControl_Button;

	// Time control UI elements
	public GameObject TimeControl_Panel;
	public Slider timeSlider;
	public Button backwardButton;
	public Button playPauseButton;
	public Button forwardButton;
	public Text timeText;
	public Button switchCameras;

	// camera stuff
	public GameObject GvrMain;
	public Camera PerspectiveCam;

	// materials
	public Material[] rule_materials;

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");
		initRootLayout ();
	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
		rlc.setUIElements (HideShow_TimeControl_Button, TimeControl_Panel, timeSlider, backwardButton, playPauseButton, forwardButton, timeText, switchCameras);
		rlc.addCameras (GvrMain, PerspectiveCam);
	}

	public GameObject getWormGUIDES_Unity() {
		return this.gameObject;
	}

	public Material[] getRuleMaterials() {
		return this.rule_materials;
	}
} 