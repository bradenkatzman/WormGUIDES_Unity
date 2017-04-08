using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * So far, looks like this class' objects will basically be a mirror to what the empty WormGUIDES_Unity
 * GameObject will hold during runtime
 */ 
public class RootLayoutController : MonoBehaviour {

	// the main (empty) game object for the WormGUIDES_Unity application
	private GameObject WormGUIDES_Unity;

	private Window3DController window3d;

	private ProductionInfo productionInfo;

	// class which holds all of the lineage data. All time points are loaded at app initialization
	private LineageData lineageData;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();


		initProductionInfo ();
		initLineageData ();
		initWindow3DController ();
	}

	private void initProductionInfo() {
		productionInfo = new ProductionInfo();
		List<List<string>> productionInfoData = ProductionInfoLoader.buildProductionInfo ();
		productionInfo.setProductionInfoData (productionInfoData);
	}

	// load all of the lineage data into the LineageData class
	private void initLineageData() {
		lineageData = LineageDataLoader.loadNucFiles (productionInfo, WormGUIDES_Unity);
	}

	private void initWindow3DController() {
		window3d = WormGUIDES_Unity.AddComponent<Window3DController> ();
		window3d.setLineageData (lineageData);
	}
}