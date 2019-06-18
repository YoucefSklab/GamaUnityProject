using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cliquez : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void clic()
    {
      if( EditorUtility.DisplayDialog("Place Selection On Surface?", "Are you sure you want to place perform the changes?", "Yes, I am sure", "No, Cancele"))
        {
            EditorUtility.DisplayDialog("Your attention is required!", "Thank you! You have made the right choice.", "Close");
        }
        else
        {
            EditorUtility.DisplayDialog("Your attention is required!", "Thank you! But you have made the wrong choice.", "Close");
        }
    }
}
