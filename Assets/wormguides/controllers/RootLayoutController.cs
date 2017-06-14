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

	private RulesLists rulesLists;

	// class which holds all of the lineage data. All time points are loaded at app initialization
	private LineageData lineageData;

	private SceneElementsList elementsList;

	private BillboardsList billboardsList;

	// scene rendering info
	private bool play;

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
	private GameObject Gvr_EmbryoCenter;
	private Camera PerspectiveCam;
	public Vector3 Gvr_EmbryoCenter_Transform_InitialPosition;
	public Vector3 Gvr_EmbryoCenter_Transform_InitialRotation;

	// zoom stuff
	private PinchZoomController pzc;

	// color scheme stuff
	private ColorScheme CS;

	private int count;
	private string showControlPanelStr = "Show Control Panel";
	private string hideControlPanelStr = "Hide Control Panel";

	private string REG_tag;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initPartsList ();
		initRulesList ();
		initProductionInfo ();
		initLineageData ();
		initSceneElementsList ();
		initBillboardsList ();
		initPinchZoomController ();
		initWindow3DController ();

		play = false;

		count = 0;

		this.REG_tag = "Root Entities Group";

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

	public void addCameras(GameObject Gvr_EmbryoCenter_, Camera PerspectiveCamera_,
		Vector3 Gvr_EmbryoCenter_Transform_InitialPosition_, Vector3 Gvr_EmbryoCenter_Transform_InitialRotation_) {
		this.Gvr_EmbryoCenter = Gvr_EmbryoCenter_;
		this.PerspectiveCam = PerspectiveCamera_;
		this.Gvr_EmbryoCenter_Transform_InitialPosition = Gvr_EmbryoCenter_Transform_InitialPosition_;
		this.Gvr_EmbryoCenter_Transform_InitialRotation = Gvr_EmbryoCenter_Transform_InitialRotation_;
	}

	public void setColorScheme(ColorScheme cs_) {
		this.CS = cs_;
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
		ApplicationModel.setTime((int)timeSlider.value);
		render ();
		updateUIElements ();
	}

	void onBackButtonClicked() {
		if (ApplicationModel.getTime() > 0) {
			ApplicationModel.setTime(ApplicationModel.getTime() - 1);
			render ();
			updateUIElements ();
		}
	}

	void onForwardButtonClicked() {
		if (ApplicationModel.getTime() < 360) {
			ApplicationModel.setTime(ApplicationModel.getTime() + 1);
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
			count = 8;
		}
	}

	void onSwitchCamerasClicked() {
		if (ApplicationModel.getCameraMode() == 0) {
//			Gvr_EmbryoCenter.SetActive(false);
//			Gvr_Perspective.SetActive(true);

			// switch the viewer to the location of the perspective camera
			Gvr_EmbryoCenter.transform.position = PerspectiveCam.transform.position;
			Gvr_EmbryoCenter.transform.eulerAngles = PerspectiveCam.transform.eulerAngles;
			Gvr_EmbryoCenter.transform.Find ("Head").localPosition = new Vector3 (0, 0, 0.08f);
			Gvr_EmbryoCenter.transform.Find ("Head").eulerAngles = new Vector3 (0, 0, 0);

			ApplicationModel.setCameraMode (1);
		} else if (ApplicationModel.getCameraMode() == 1) {
//			Gvr_Perspective.SetActive(false);
//			Gvr_EmbryoCenter.SetActive(true);

			Gvr_EmbryoCenter.transform.position = Gvr_EmbryoCenter_Transform_InitialPosition;
			Gvr_EmbryoCenter.transform.eulerAngles = Gvr_EmbryoCenter_Transform_InitialRotation;
			Gvr_EmbryoCenter.transform.Find ("Head").localPosition = new Vector3 (0, 0, 0.08f);
			Gvr_EmbryoCenter.transform.Find ("Head").eulerAngles = new Vector3 (0, 0, 0);


			ApplicationModel.setCameraMode (0);
		}
	}

	void onColorSchemeDropdownValueChanged() {
		this.window3d.updateColorScheme (this.ColorScheme_Dropdown.value, this.WormGUIDES_Unity);
	}

	private void initProductionInfo() {
		this.productionInfo = new ProductionInfo(ProductionInfoLoader.buildProductionInfo ());
	}

	// load all of the lineage data into the LineageData class
	private void initLineageData() {
		this.lineageData = LineageDataLoader.loadNucFiles (productionInfo, rulesLists);
	}

	private void initPartsList() {
		PartsList.initPartsList ();
	}

	private void initRulesList() {
		this.rulesLists = new RulesLists (
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTractTourNerveRingRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getLineageSpatialRelationshipsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getNeuronalCellPositionsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTissueTypesRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getDefaultMaterials ());
	}

	private void initSceneElementsList() {
		this.elementsList = new SceneElementsList (lineageData);
	}

	private void initBillboardsList() {
		this.billboardsList = new BillboardsList ();
	}

	private void initPinchZoomController() {
		pzc = this.gameObject.AddComponent<PinchZoomController> ();
		pzc.setCamera (this.PerspectiveCam);
	}

	private void initWindow3DController() {
		this.window3d = new Window3DController(
			productionInfo.getXScale(),
			productionInfo.getYScale(),
			productionInfo.getZScale(),
			lineageData,
			elementsList,
			billboardsList,
			Gvr_EmbryoCenter,
			PerspectiveCam,
			Gvr_EmbryoCenter_Transform_InitialPosition,
			Gvr_EmbryoCenter_Transform_InitialRotation,
			LineageDataLoader.getAvgXOffsetFromZero(),
			LineageDataLoader.getAvgYOffsetFromZero(),
			LineageDataLoader.getAvgZOffsetFromZero(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTractTourNerveRingRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getLineageSpatialRelationshipsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getNeuronalCellPositionsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTissueTypesRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getDefaultMaterials (),
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
			if (count == 8) { // use this to render at 1/4 of the speed at which Update() is called
				if (ApplicationModel.getTime() < 360 && ApplicationModel.getTime() > 0) {
					render ();
					ApplicationModel.setTime(ApplicationModel.getTime() + 1);
					updateUIElements ();
				} else if (ApplicationModel.getTime() == 360) {
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
		GameObject reg = window3d.renderScene (ApplicationModel.getTime());
		reg.tag = REG_tag;
		if (reg != null && ApplicationModel.getCameraMode() == 1) {
			reg.transform.rotation = ApplicationModel.getGvrHeadRot ();
		}
		reg.transform.parent = WormGUIDES_Unity.transform;
	}

	private void updateUIElements() {
		updateSlider ();
		updateTimeText ();
	}

	private void updateSlider() {
		timeSlider.value = ApplicationModel.getTime();
	}

	private void updateTimeText() {
		timeText.text = ApplicationModel.getTime().ToString ();
	}
}