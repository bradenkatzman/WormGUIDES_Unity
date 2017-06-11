using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel  {

	// 0 == VR, 1 == Perspective
	private static int cameraMode = 0;

	private static int time = 360;

	public static void setCameraMode(int mode) {
		cameraMode = mode;
	}

	public static int getCameraMode() {
		return cameraMode;
	}

	public static void setTime(int t) {
		time = t;
	}

	public static int getTime() {
		return time;
	}
}