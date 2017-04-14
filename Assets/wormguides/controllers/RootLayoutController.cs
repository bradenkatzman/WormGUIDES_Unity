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

	// scene rendering info
	private int time;
	private bool play;
	private bool pause;

	// UI elements
	private Slider timeSlider;
	private Button backwardButton;
	private Button playPauseButton;
	private Button forwardButton;
	private Text timeText;

	void Start () {
		this.WormGUIDES_Unity = this.GetComponent<WormGUIDES_UnityApp> ().getWormGUIDES_Unity ();

		initProductionInfo ();
		initLineageData ();
		initSceneElementsList ();
		initWindow3DController ();

		play = false;
		pause = true;
		time = 360;

		render ();
	}

	//
	public void setUIElements(Slider ts, Button bb, Button ppb, Button fb, Text tt) {
		this.timeSlider = ts;
		this.backwardButton = bb;
		this.playPauseButton = ppb;
		this.forwardButton = fb;
		this.timeText = tt;

		timeSlider.onValueChanged.AddListener (delegate {onSliderValueChange ();});
		backwardButton.onClick.AddListener (onBackButtonClicked);
		playPauseButton.onClick.AddListener (onPlayPauseButtonClicked);
		forwardButton.onClick.AddListener (onForwardButtonClicked);
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
		}
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

	private void initWindow3DController() {
		window3d = new Window3DController(
			productionInfo.getXScale(),
			productionInfo.getYScale(),
			productionInfo.getZScale(),
			lineageData,
			elementsList,
			LineageDataLoader.getAvgXOffsetFromZero(),
			LineageDataLoader.getAvgYOffsetFromZero(),
			LineageDataLoader.getAvgZOffsetFromZero());
	}

	/*
	 * Update is called once per frame
	 *  - Here we check the state of the app (play, pause, iterate forward, iterate backward)
	 *    and render the scene accordingly
	 */ 
	void Update() {
		if (play) {
			if (time < 360 && time > 0) {
				render ();
				time++;
				updateUIElements ();
			}
		}
	}

	private void render() {
		window3d.renderScene (time).transform.parent = WormGUIDES_Unity.transform;
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