using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoomController : MonoBehaviour {

	private float perspectiveZoomSpeed;

	private bool initialized;

	// camera stuff
	private GameObject GvrMain;
	private Camera PerspectiveCam;

	void Start() {
		this.initialized = false;
		this.perspectiveZoomSpeed = 0.5f;
	}

	public void setCameras(GameObject vrc, Camera pc) {
		this.GvrMain = vrc;
		this.PerspectiveCam = pc;
		this.initialized = true;
	}
		
	void Update () {
		// make sure start up complete and in perspective mode
		if (initialized && PerspectiveCam.enabled) {

			// if there are two touches on the device
			if (Input.touchCount == 2) {

				// store the touches
				Touch tZero = Input.GetTouch (0);
				Touch tOne = Input.GetTouch (1);

				// find the position in the previous frame of each touch
				Vector2 tZeroPrevPos = tZero.position - tZero.deltaPosition;
				Vector2 tOnePrevPos = tOne.position - tOne.deltaPosition;

				// find the mag of the vector (distance) between the touches in each frame
				float prevTouchDeltaMag = (tZeroPrevPos - tOnePrevPos).magnitude;
				float tDeltaMag = (tZero.position - tOne.position).magnitude;

				// find the difference in the distances between each frame
				float deltaMagDiff = prevTouchDeltaMag - tDeltaMag;


			}
		}
	}
}