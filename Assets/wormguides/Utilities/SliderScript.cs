using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public bool isSelected;

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    // Use this for initialization
    void Start () {
        isSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isSelect()
    {
        return this.isSelected;
    }
}
