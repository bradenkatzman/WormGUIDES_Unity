using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveCameraCapture : MonoBehaviour {

	public string File_Path;
	private string frame_str = "Frame";
	private string undrscr = "_";
	private string PNG = ".png";

	private bool capturing;
	private string CAPTURE_KEY = "p";
	private string REG_tag = "Root Entities Group";
	private string five_pad = "00000";
	private string four_pad = "0000";
	private string three_pad = "000";

	public float[] keyFramesRotate;
	public float[] keyValuesRotate;

	public int count;

	// Use this for initialization
	void Start () {
		this.capturing = false;
		count = 1;
	}
	
	// Update is called once per frame
	void Update () {
		// check for capture start and stop
		if (Input.GetKeyDown (CAPTURE_KEY)) {
			//Debug.Log ("Capture key pressed");
			if (count < 10) {
				Application.CaptureScreenshot (File_Path + frame_str + undrscr + five_pad + count.ToString () + PNG);
			} else if (count < 100) {
				Application.CaptureScreenshot (File_Path + frame_str + undrscr + four_pad + count.ToString () + PNG);
			} else {
				Application.CaptureScreenshot (File_Path + frame_str + undrscr + three_pad + count.ToString () + PNG);
			}
			count++;


//			if (this.capturing) {
//				this.capturing = false;
//			} else {
//				this.capturing = true;
//			}
		}

		// apply rotation
		GameObject REG = GameObject.FindWithTag(REG_tag);
		if (REG != null) {
			float newRotate = computeInterpolatedValue();

			// make quaterion --> apply around x axis because this is approx AP
			REG.transform.eulerAngles = new Vector3(newRotate, 0.0f, 0.0f);
		}
	}

	private float computeInterpolatedValue() {
		int time = ApplicationModel.getTime ();

		if (time <= (int)keyFramesRotate [0]) return keyValuesRotate [0];

		if (time >= (int)keyFramesRotate [keyFramesRotate.Length - 1]) return keyValuesRotate [keyValuesRotate.Length - 1];

		int i;
		for (i = 0; i < keyFramesRotate.Length; i++) {
			if ((int)keyFramesRotate[i] == time) return keyValuesRotate [i];

			if ((int)keyFramesRotate [i] > time) break;
		}

		// interpolate between values at i and i-1
		float alpha = (((float)time - keyFramesRotate[i - 1]) / (keyFramesRotate[i] - keyFramesRotate[i - 1]));
		float value = keyValuesRotate [i] * alpha + keyValuesRotate [i - 1] * (1 - alpha);
		return value;
	}
}
