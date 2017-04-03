using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/*
 * Main script for WormGUIDES_Unity
 * 
 * This script is run at startup and facilitates construction
 * of a WormGUIDES_Unity instance
 * */
public class WormGUIDES_UnityApp : MonoBehaviour {

	private RootLayoutController rlc;

	void Start() {
		Debug.Log("Starting WormGUIDES_Unity application");
		initRootLayout ();
	}
		
	private void initRootLayout() {
		rlc = this.gameObject.AddComponent<RootLayoutController> ();
	}

	public GameObject getWormGUIDES_Unity() {
		return this.gameObject;
	}
} 