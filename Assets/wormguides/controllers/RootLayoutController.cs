using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * So far, looks like this class' objects will basically be a mirror to what the empty WormGUIDES_Unity
 * GameObject will hold during runtime
 */ 
public class RootLayoutController : MonoBehaviour {

	// the main (empty) game object for the WormGUIDES_Unity application
	private GameObject WormGUIDES_Unity;

	private Window3DController window3d;

	private ProductionInfo productionInfo;

	// class which holds all of the lineage data. All time points are loaded at app initialization
	private LineageData lineageData;

	private SceneElementsList elementsList;

	private BillboardsList billboardsList;

	// scene rendering info
	private int time;
	private bool play;
//	private bool pause;

	// hide/show time control panel button
	private Button HideShow_Control_Button;

	// control UI elements
	private GameObject Control_Panel;
	private Slider timeSlider;
	private Button backwardButton;
	private Button playPauseButton;
	private Button forwardButton;
	private Text timeText;
	private Button switchCameras;
	private Dropdown ColorScheme_Dropdown;

	// context menu
	private GameObject ContextMenu;

	// camera stuff
	private GameObject GvrMain;
	private Camera PerspectiveCam;

	// color scheme stuff
	private ColorScheme CS;

	private int count;
	private string showControlPanelStr = "Show Control Panel";
	private string hideControlPanelStr = "Hide Control Panel";

	private Gyroscope gyro;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initProductionInfo ();
		initLineageData ();
		initSceneElementsList ();
		initBillboardsList ();
		initWindow3DController ();

		play = false;
//		pause = true;
		time = 360;

		count = 0;

		render ();
	}

	//
	public void setUIElements(Button hs, GameObject tcp, Slider ts, Button bb, 
		Button ppb, Button fb, Text tt, Button sc, Dropdown csd, GameObject cm) {
		this.HideShow_Control_Button = hs;
		this.Control_Panel = tcp;
		this.timeSlider = ts;
		this.backwardButton = bb;
		this.playPauseButton = ppb;
		this.forwardButton = fb;
		this.timeText = tt;
		this.switchCameras = sc;
		this.ColorScheme_Dropdown = csd;
		this.ContextMenu = cm;

		HideShow_Control_Button.onClick.AddListener (onHideShowTimeControlPanelClicked);
		timeSlider.onValueChanged.AddListener (delegate {onSliderValueChange ();});
		backwardButton.onClick.AddListener (onBackButtonClicked);
		playPauseButton.onClick.AddListener (onPlayPauseButtonClicked);
		forwardButton.onClick.AddListener (onForwardButtonClicked);
		switchCameras.onClick.AddListener (onSwitchCamerasClicked);
		ColorScheme_Dropdown.onValueChanged.AddListener (delegate { onColorSchemeDropdownValueChanged(); });
	}

	public void addCameras(GameObject GvrMain_, Camera PerspectiveCam_) {
		this.GvrMain = GvrMain_;
		this.PerspectiveCam = PerspectiveCam_;
	}

	public void setColorScheme(ColorScheme cs_) {
		this.CS = cs_;
	}

	public void setGyro(Gyroscope gyro_) {
		this.gyro = gyro_;
	}

	public void onHideShowTimeControlPanelClicked() {
		if (Control_Panel.activeSelf) {
			Control_Panel.SetActive(false);
			HideShow_Control_Button.GetComponentInChildren<Text> ().text = showControlPanelStr;
		} else {
			Control_Panel.SetActive (true);
			HideShow_Control_Button.GetComponentInChildren<Text> ().text = hideControlPanelStr;
		}
	}

	public void onSliderValueChange() {
		time = (int) timeSlider.value;
		render ();
		updateUIElements ();
	}

	void onBackButtonClicked() {
		if (time > 0) {
			time--;
			render ();
			updateUIElements ();
		}
	}

	void onForwardButtonClicked() {
		if (time < 360) {
			time++;
			render ();
			updateUIElements ();
		}
	}

	void onPlayPauseButtonClicked() {
		if (play) {
			play = false;
			playPauseButton.GetComponentInChildren<Text> ().text = "Play";
		} else {
			play = true;
			playPauseButton.GetComponentInChildren<Text> ().text = "Pause";
			count = 3;
		}
	}

	void onSwitchCamerasClicked() {
		if (GvrMain.activeSelf) {
			GvrMain.SetActive(false);
			PerspectiveCam.enabled = true;
		} else if (PerspectiveCam.enabled) {
			PerspectiveCam.enabled = false;
			GvrMain.SetActive(true);
		}
	}

	void onColorSchemeDropdownValueChanged() {
		window3d.updateColorScheme (this.ColorScheme_Dropdown.value, this.WormGUIDES_Unity, this.gyro.attitude);
	}

	private void initProductionInfo() {
		productionInfo = new ProductionInfo(ProductionInfoLoader.buildProductionInfo ());
	}

	// load all of the lineage data into the LineageData class
	private void initLineageData() {
		lineageData = LineageDataLoader.loadNucFiles (productionInfo, WormGUIDES_Unity);
	}

	private void initSceneElementsList() {
		elementsList = new SceneElementsList (lineageData);
	}

	private void initBillboardsList() {
		billboardsList = new BillboardsList ();
	}

	private void initWindow3DController() {
		window3d = new Window3DController(
			productionInfo.getXScale(),
			productionInfo.getYScale(),
			productionInfo.getZScale(),
			lineageData,
			elementsList,
			billboardsList,
			GvrMain,
			PerspectiveCam,
			LineageDataLoader.getAvgXOffsetFromZero(),
			LineageDataLoader.getAvgYOffsetFromZero(),
			LineageDataLoader.getAvgZOffsetFromZero(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp>().getTractTourNerveRingRuleMaterials(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp>().getLineageSpatialRelationshipsRuleMaterials(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp>().getDefaultMaterials(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp>().getTextMaterial(),
			this.CS,
			this.ContextMenu);
	}

	/*
	 * Update is called once per frame
	 *  - Here we check the state of the app (play, pause, iterate forward, iterate backward)
	 *    and render the scene accordingly
	 */ 
	void Update() {
		if (play) {
			if (count == 3) { // use this to render at 1/4 of the speed at which Update() is called
				if (time < 360 && time > 0) {
					render ();
					time++;
					updateUIElements ();
				} else if (time == 360) {
					play = false;
					playPauseButton.GetComponentInChildren<Text> ().text = "Play";
				}
				count = 0;
			} else {
				count++;
			}
		}

		window3d.Update ();
	}

	private void render() {
		GameObject reg = window3d.renderScene (time);
		if (!GvrMain.activeSelf && PerspectiveCam.enabled) {
			reg.transform.rotation = gyro.attitude;
		}
		reg.transform.parent = WormGUIDES_Unity.transform;
	}

	private void updateUIElements() {
		updateSlider ();
		updateTimeText ();
	}

	private void updateSlider() {
		timeSlider.value = time;
	}

	private void updateTimeText() {
		timeText.text = time.ToString ();
	}
}