using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoomController : MonoBehaviour {

	private float perspectiveZoomSpeed;

	// camera stuff
	private Camera PerspectiveCam;

	void Start() {
		this.perspectiveZoomSpeed = 1.5f;
	}

	public void setCamera(Camera pc) {
		this.PerspectiveCam = pc;
	}

	void Update () {
		// make sure start up complete and in perspective mode
		if (PerspectiveCam.enabled) {

			// if there are two touches on the device
			if (Input.touchCount == 2) {
				Debug.Log ("Got two touch");

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

				// check if zoom in or zoom out
				if (deltaMagDiff > 0.0f) {
					PerspectiveCam.transform.Translate (PerspectiveCam.transform.forward * -perspectiveZoomSpeed);
				} else if (deltaMagDiff < 0.0f) {
					PerspectiveCam.transform.Translate (PerspectiveCam.transform.forward * perspectiveZoomSpeed);
				}
			} else {
				//Debug.Log (Input.touchCount);
			}
		}
	}
}