using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneLoader : MonoBehaviour {

	// the index of the scene that will be built by this script
	private int scene;

	// Use this for initialization
	void Start () {
		this.scene = 1;

		// start the loading of the main scene in the background
		StartCoroutine(LoadNewScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
	IEnumerator LoadNewScene() {

		// This line waits for 3 seconds before executing the next line in the coroutine.
		// This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
		yield return new WaitForSeconds (3);

		// Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
		AsyncOperation async = Application.LoadLevelAsync (scene);

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}
}