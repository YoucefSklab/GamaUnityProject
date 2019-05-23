using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {

#if UNITY_EDITOR
  Debug.unityLogger.logEnabled = true;
#else
  Debug.logger.logEnabled = false;
#endif

    }

    // Update is called once per frame
    void Update () {
		
	}
}
