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

    // entity label
    private TextMesh entityLabel;

	private int count;
	private string showControlPanelStr = "Show Control Panel";
	private string hideControlPanelStr = "Hide Control Panel";

	private string REG_tag;

    private int speed;
    private float lastX;
    private float lastY;
    private float rotX;
    private float rotY;
    private Vector3 currPos;
    private float prevScroll;

    private bool isSliderSelected;

    private int xScale, yScale, zScale;
    private int offsetX, offsetY, offsetZ;

    private bool complexGeometryClicked;
    private string entityLabelLineageNameRef;

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

        isSliderSelected = false;

        speed = 100;
        lastX = 0;
        lastY = 0;
        rotX = 0;
        rotY = 0;
        currPos = new Vector3(0, 0, 0);
        complexGeometryClicked = false;
        entityLabelLineageNameRef = "";
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

    public void setEntityLabel(TextMesh el)
    {
        entityLabel = el;
    }

    public void setModelVars(int xOff, int yOff, int zOff, int xS, int yS, int zS)
    {
        this.offsetX = xOff;
        this.offsetY = yOff;
        this.offsetZ = zOff;
        this.xScale = xS;
        this.yScale = yS;
        this.zScale = zS;
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
     *  - Rotation, zoom and translation is also handled here  
	 */
    void Update()
    {
        if (play)
        {
            if (count == 8)
            { // use this to render at 1/8 of the speed at which Update() is called
                if (ApplicationModel.getTime() < 360 && ApplicationModel.getTime() > 0)
                {
                    render();
                    ApplicationModel.setTime(ApplicationModel.getTime() + 1);
                    updateUIElements();
                }
                else if (ApplicationModel.getTime() == 360)
                {
                    play = false;
                    playPauseButton.GetComponentInChildren<Text>().text = "Play";
                }
                count = 0;
            }
            else
            {
                count++;
            }
        }

        // only apply mouse rotation and scroll if the slider is not currently selected
        if (!timeSlider.GetComponent<SliderScript>().isSelect())
        {
            // left click and hold - for rotation
            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                if (lastX != Input.GetAxis("Mouse X") || lastY != Input.GetAxis("Mouse Y"))
                {
                    lastX = Input.GetAxis("Mouse X");
                    lastY = Input.GetAxis("Mouse Y");

                    rotX = lastX * speed * Mathf.Deg2Rad;
                    rotY = lastY * speed * Mathf.Deg2Rad;

                    window3d.getRootEntitiesGroup().transform.Rotate(Vector3.up, -rotX, Space.World);
                    window3d.getRootEntitiesGroup().transform.Rotate(Vector3.right, rotY, Space.World);
                }
            }

            // left click, for label
            if (Input.GetMouseButtonDown(0))
            {
                // check if entity clicked
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {   
                    // first check if the label is active already
                    if (entityLabel.gameObject.activeSelf)
                    {
                        // check if this is a click on the same object
                        if (entityLabel.GetComponent<TextMesh>().text == hitInfo.transform.gameObject.name
                            || PartsList.getLineageNameByTerminalName(entityLabel.GetComponent<TextMesh>().text)  == hitInfo.transform.gameObject.name)
                        {
                            // turn off the label
                            entityLabel.gameObject.SetActive(false);
                        }
                        else
                        {
                            // new entity clicked, change the label

                            // check if this is a primitive shape or custom geometry (which are constructed in differented coordinated systems,
                            // and hence, have different rendering requirements for alignment) by seeing if the entity has childen (b/c the 
                            // custom geomtry all have a child game object which contains the mesh and the mesh collider)

                            // not doing anything for this now, but that comment explains how the complex geometry (MCS stuff - not stuff that's
                            // a single cell's body) works
                            entityLabelLineageNameRef = hitInfo.transform.gameObject.name;
                            entityLabel.GetComponent<TextMesh>().text = PartsList.getTerminalNameByLineageName(hitInfo.transform.gameObject.name);

                            // no difference here yet
                            if (hitInfo.transform.childCount == 0)
                            {
                                complexGeometryClicked = false;
                                entityLabel.transform.position = hitInfo.transform.gameObject.transform.position;
                            } else
                            {
                                // set the complex geometry flag
                                complexGeometryClicked = true;

                                entityLabel.transform.position = hitInfo.transform.gameObject.transform.position;
                            }
                        }
                    }
                    else
                    {
                        entityLabelLineageNameRef = hitInfo.transform.gameObject.name;
                        entityLabel.GetComponent<TextMesh>().text = PartsList.getTerminalNameByLineageName(hitInfo.transform.gameObject.name);
                        entityLabel.transform.position = hitInfo.transform.gameObject.transform.position;
                        entityLabel.gameObject.SetActive(true);
                    }
                }
            }


            // double click and hold - for translation
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                if (lastX != Input.GetAxis("Mouse X") || lastY != Input.GetAxis("Mouse Y"))
                {
                    float distance_to_screen = Camera.main.WorldToScreenPoint(window3d.getRootEntitiesGroup().transform.position).z;
                    Vector3 currPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
                    float step = speed * Time.deltaTime;
                    window3d.getRootEntitiesGroup().transform.position = Vector3.MoveTowards(window3d.getRootEntitiesGroup().transform.position, currPos, step);

                    lastX = Input.GetAxis("Mouse X");
                    lastY = Input.GetAxis("Mouse Y");
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

        
        // if the label is active, make it look toward the camera
        if (entityLabel.gameObject.activeSelf)
        {
            entityLabel.gameObject.transform.LookAt(PerspectiveCam.transform);
            entityLabel.gameObject.transform.Rotate(Vector3.up, 180);

            Transform entity = window3d.getRootEntitiesGroup().transform.Find(entityLabelLineageNameRef); // this will help account for terminal names which are used more than once
            if (entity != null)
            {
                 //keep the label stuck to the entity
                entityLabel.transform.position = entity.position;
            } else
            {
                // remove the label
                entityLabel.gameObject.SetActive(false);
            }
        }
    }

	private void render() {
        Quaternion rotSave = window3d.getRootEntitiesGroup().transform.rotation;
        Vector3 posSave = window3d.getRootEntitiesGroup().transform.position;

		GameObject reg = window3d.renderScene (ApplicationModel.getTime());
		reg.transform.parent = WormGUIDES_Unity.transform;
        
        window3d.getRootEntitiesGroup().transform.rotation = rotSave;
        window3d.getRootEntitiesGroup().transform.position = posSave;
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