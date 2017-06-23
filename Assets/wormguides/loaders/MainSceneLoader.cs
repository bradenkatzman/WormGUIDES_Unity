using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour {

	public Button InternalCameraButton;
	public Button ExternalCameraButton;

	public Text MenuCameraSelectionText;
	private string MenuCameraSelectionTextStr = "Please wait a few moments while the application loads.";

	// the index of the scene that will be built by this script
	private int scene;

	AsyncOperation async;

	private string VRModeStr = "Loading Internal View...";
	private string PerspectiveModeStr = "Loading External View...";

	private int INTERNAL_CAMERA_MODE = 0;
	private int EXTERNAL_CAMERA_MODE = 1;

	// Use this for initialization
	void Start () {
		this.scene = 1;

		PartsList.initPartsList ();
		ProductionInfo.initProductionInfo ();

		// start the loading of the main scene in the background
		StartCoroutine(LoadNewScene());

		this.InternalCameraButton.onClick.AddListener (VRModeButtonClicked);
		this.ExternalCameraButton.onClick.AddListener (PerspectiveModeButtonClicked);
	}

	void VRModeButtonClicked() {
		ApplicationModel.setCameraMode (INTERNAL_CAMERA_MODE);
		this.async.allowSceneActivation = true;

		this.InternalCameraButton.GetComponentInChildren<Text> ().text = VRModeStr;

		this.InternalCameraButton.interactable = false;
		this.ExternalCameraButton.interactable = false;

		this.MenuCameraSelectionText.text = MenuCameraSelectionTextStr;
	}

	void PerspectiveModeButtonClicked() {
		ApplicationModel.setCameraMode (EXTERNAL_CAMERA_MODE);
		this.async.allowSceneActivation = true;

		this.ExternalCameraButton.GetComponentInChildren<Text> ().text = PerspectiveModeStr;

		this.InternalCameraButton.interactable = false;
		this.ExternalCameraButton.interactable = false;

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