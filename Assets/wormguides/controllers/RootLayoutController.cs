using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLayoutController : MonoBehaviour {

	// the main (empty) game object for the WormGUIDES_Unity application
	private GameObject WormGUIDES_Unity;

	private Window3DController window3d;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initWindow3DController ();
	}

	private void initWindow3DController() {
		window3d = WormGUIDES_Unity.AddComponent<Window3DController> ();
	}
}