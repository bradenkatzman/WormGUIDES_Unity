using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window3DController : MonoBehaviour {

	private int time;

	private bool play;
	private bool pause;

	// for rendering
	private NucleiLoader nucLdr;
	private ShapesLoader shapesLdr;

	private LineageData lineageData;

	void Start () {
		time = 360;
		play = false;
		pause = true;

		// just render one time point for now
		renderScene();
	}

	void Update() {
		// check if we're in play mode
		if (play) {

		}
	}

	private void renderScene() {
		getSceneData ();
	}

	private void getSceneData() {

	}

	public void setLineageData(LineageData ld) {
		this.lineageData = ld;
	}
}
