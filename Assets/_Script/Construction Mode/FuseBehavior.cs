﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBehavior : MonoBehaviour {



	//list of GameObjects this object can be fused to along with corresponding fuse locations
	private FuseAttributes fuseInfo;
	//private Dictionary<string, Quaternion> fuseRotations;
	public bool isFused;
	private GameObject assignedButton;

	// Use this for initialization
	void Awake () {

	}

	public void setFuseTo(FuseAttributes fuseAtts) {
		fuseInfo = fuseAtts;

	}

	public void setButtonTo(GameObject newButton) {
		assignedButton = newButton;
		
	}

	public Vector3 getFuseLocation(string fuseTo) {
		return fuseInfo.getFuseLocation(fuseTo);
	}

	public Quaternion getFuseRotation(string fuseTo) {
		return fuseInfo.getFuseRotation(fuseTo);
	}

	public Quaternion[] getAcceptableRotations(string fuseTo) {
		return fuseInfo.getAcceptableRotations(fuseTo);
	}

	public string[] getAcceptableLocations(string fuseTo) {
		return fuseInfo.getAcceptableLocations();
	}

	public bool fused() {
		return isFused;
	}

	public void fuse(string fuseTo) {
		GameObject parent = gameObject.transform.parent.gameObject;
		string newFuseToName = fuseTo;

        Debug.Log("FuseBehavior now moving object to " + fuseInfo.getFuseLocation(newFuseToName) + "!");
        parent.transform.position = fuseInfo.getFuseLocation(newFuseToName);
        Debug.Log("FuseBehavior moved object to fuse location!");
        Debug.Log("Now setting rotation to fuseRotation!");
        parent.transform.rotation = fuseInfo.getFuseRotation(newFuseToName);
        Debug.Log("FuseBehavior successfully set fuseRotation!");
		//Destroy(gameObject.GetComponent<SelectBehavior>());

		isFused = true;
		parent.GetComponent<IsFused>().isFused = true;
		//FuseBehavior[] fuseBehaviors = parent.transform.GetComponentsInChildren<FuseBehavior>();
		//for (int i = 0; i < fuseBehaviors.Length; i++) {
		//	fuseBehaviors[i].isFused = true;
		//}
		int uses = assignedButton.GetComponent<Uses>().numUses;

		if(uses > 1) {
			assignedButton.GetComponent<Uses>().numUses = uses - 1;
		} else if(uses <= 1) {
			assignedButton.transform.GetComponent<Button>().interactable = false;
		}
		assignedButton = null;

	}
}
