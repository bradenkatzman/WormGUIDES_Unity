using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel  {

	// 0 == VR, 1 == Perspective
	private static int cameraMode = 0;

	public static void setCameraMode(int mode) {
		cameraMode = mode;
	}

	public static int getCameraMode() {
		return cameraMode;
	}
}