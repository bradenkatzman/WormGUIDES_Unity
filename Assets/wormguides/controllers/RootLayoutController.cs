using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLayoutController : MonoBehaviour {

	// the main (empty) game object for the WormGUIDES_Unity application
	private GameObject WormGUIDES_Unity;

	private Window3DController window3d;

	// class which holds all of the lineage data. All time points are loaded at app initialization
	private LineageData lineageData;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initLineageData ();
		initWindow3DController ();
	}

	// load all of the lineage data into the LineageData class
	private void initLineageData() {

	}

	private void initWindow3DController() {
		window3d = WormGUIDES_Unity.AddComponent<Window3DController> ();
		window3d.setLineageData (lineageData);setLineageData
	}
}