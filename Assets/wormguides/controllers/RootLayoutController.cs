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

	// control UI elements
	private Slider timeSlider;
	private Button backwardButton;
	private Button playPauseButton;
	private Button forwardButton;
	private Text timeText;
	private Dropdown ColorScheme_Dropdown;

	// camera stuff
	private Camera PerspectiveCam;

	// zoom stuff
	private PinchZoomController pzc;

	// color scheme stuff
	private ColorScheme CS;

	private int count;
	private string showControlPanelStr = "Show Control Panel";
	private string hideControlPanelStr = "Hide Control Panel";

	private string REG_tag;

    private int speed = 100;
    private float lastX = 0;
    private float lastY = 0;
    private float prevScroll;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initRulesList ();
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
	public void setUIElements(GameObject tcp, Slider ts, Button bb, 
		Button ppb, Button fb, Text tt, Dropdown csd) {
		this.timeSlider = ts;
		this.backwardButton = bb;
		this.playPauseButton = ppb;
		this.forwardButton = fb;
		this.timeText = tt;
		this.ColorScheme_Dropdown = csd;

		timeSlider.onValueChanged.AddListener (delegate {onSliderValueChange ();});
		backwardButton.onClick.AddListener (onBackButtonClicked);
		playPauseButton.onClick.AddListener (onPlayPauseButtonClicked);
		forwardButton.onClick.AddListener (onForwardButtonClicked);
		ColorScheme_Dropdown.onValueChanged.AddListener (delegate { onColorSchemeDropdownValueChanged(); });
	}

	public void addCameras(Camera PerspectiveCamera_) {
		this.PerspectiveCam = PerspectiveCamera_;
	}

	public void setColorScheme(ColorScheme cs_) {
		this.CS = cs_;
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

	void onSwitchCamerasClicked() {	}

	void onColorSchemeDropdownValueChanged() {
		this.window3d.updateColorScheme (this.ColorScheme_Dropdown.value, this.WormGUIDES_Unity);
	}

	// load all of the lineage data into the LineageData class
	private void initLineageData() {
		this.lineageData = LineageDataLoader.loadNucFiles (rulesLists);
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
			ProductionInfo.getXScale(),
			ProductionInfo.getYScale(),
			ProductionInfo.getZScale(),
			lineageData,
			elementsList,
			billboardsList,
			PerspectiveCam,
			LineageDataLoader.getAvgXOffsetFromZero(),
			LineageDataLoader.getAvgYOffsetFromZero(),
			LineageDataLoader.getAvgZOffsetFromZero(),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTractTourNerveRingRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getLineageSpatialRelationshipsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getNeuronalCellPositionsRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getTissueTypesRuleMaterials (),
			WormGUIDES_Unity.GetComponent<WormGUIDES_UnityApp> ().getDefaultMaterials (),
			this.CS);
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

        if (Input.GetMouseButton(0))
        {
            if (lastX != Input.GetAxis("Mouse X") || lastY != Input.GetAxis("Mouse Y"))
            {
                lastX = Input.GetAxis("Mouse X");
                lastY = Input.GetAxis("Mouse Y");

                float rotX = lastX * speed * Mathf.Deg2Rad;
                float rotY = lastY * speed * Mathf.Deg2Rad;

                window3d.getRootEntitiesGroup().transform.Rotate(Vector3.up, -rotX, Space.World);
                window3d.getRootEntitiesGroup().transform.Rotate(Vector3.right, rotY, Space.World);
            }
        }

        float currScroll = Input.GetAxis("Mouse ScrollWheel");
        if (currScroll != prevScroll)
        {
            if (currScroll > 0f)
            {
                // zoom in
                if (PerspectiveCam.transform.position.z < 0f)
                {
                    PerspectiveCam.transform.position = new Vector3(PerspectiveCam.transform.position.x, PerspectiveCam.transform.position.y,
                        PerspectiveCam.transform.position.z + 5);
                }

            }
            else if (currScroll < 0f)
            {
                // zoom out
                if (PerspectiveCam.transform.position.z > -360)
                {
                    PerspectiveCam.transform.position = new Vector3(PerspectiveCam.transform.position.x, PerspectiveCam.transform.position.y,
                        PerspectiveCam.transform.position.z - 5);
                }

            }
        }

        prevScroll = 0;
    }

	private void render() {
		GameObject reg = window3d.renderScene (ApplicationModel.getTime());
		if (reg != null && ApplicationModel.getCameraMode() == 1) {
			reg.tag = REG_tag;
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