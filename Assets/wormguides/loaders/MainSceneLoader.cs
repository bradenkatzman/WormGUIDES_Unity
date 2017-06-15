using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour {

	public Button VRModeButton;
	public Button PerspectiveModeButton;

	public Text MenuCameraSelectionText;
	private string MenuCameraSelectionTextStr = "Please wait a few moments while the application loads.";

	// the index of the scene that will be built by this script
	private int scene;

	AsyncOperation async;

	private string VRModeStr = "Loading 360 Mode...";
	private string PerspectiveModeStr = "Loading Persp. Mode...";

	private int VR_MODE = 0;
	private int PERSPECTIVE_MODE = 1;

	// Use this for initialization
	void Start () {
		this.scene = 1;

		// start the loading of the main scene in the background
		StartCoroutine(LoadNewScene());

		this.VRModeButton.onClick.AddListener (VRModeButtonClicked);
		this.PerspectiveModeButton.onClick.AddListener (PerspectiveModeButtonClicked);
	}

	void VRModeButtonClicked() {
		ApplicationModel.setCameraMode (VR_MODE);
		this.async.allowSceneActivation = true;

		this.VRModeButton.GetComponentInChildren<Text> ().text = VRModeStr;

		this.VRModeButton.interactable = false;
		this.PerspectiveModeButton.interactable = false;

		this.MenuCameraSelectionText.text = MenuCameraSelectionTextStr;
	}

	void PerspectiveModeButtonClicked() {
		ApplicationModel.setCameraMode (PERSPECTIVE_MODE);
		this.async.allowSceneActivation = true;

		this.PerspectiveModeButton.GetComponentInChildren<Text> ().text = PerspectiveModeStr;

		this.VRModeButton.interactable = false;
		this.PerspectiveModeButton.interactable = false;

		this.MenuCameraSelectionText.text = MenuCameraSelectionTextStr;
	}

	// load the scene by index
	IEnumerator LoadNewScene() {
		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		this.async = SceneManager.LoadSceneAsync (scene);
		this.async.allowSceneActivation = false;
		yield return async;
	}
}