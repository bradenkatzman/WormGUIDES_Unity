using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel  {

	// 0 == INTERNAL_CAMERA_MODE, 1 == EXTERNAL_CAMERA_MODE
	private static int cameraMode = 0;

	private static int time = 360;

	private static int NUM_COLOR_SCHEMES = 4;

	private static Quaternion Gvr_Head_Rot = Quaternion.identity;

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

	public static int getNumColorSchemes() {
		return NUM_COLOR_SCHEMES;
	}

	public static void setGvrHeadRot(Quaternion q) {
		Gvr_Head_Rot = q;
	}

	public static Quaternion getGvrHeadRot() {
		return Gvr_Head_Rot;
	}
}