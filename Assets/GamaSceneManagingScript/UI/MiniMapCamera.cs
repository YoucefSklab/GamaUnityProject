using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {
    public Camera miniMapCamera;
    public Camera mainCamera;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      
        miniMapCamera.transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.localPosition.z);
     //   miniMapCamera.GetComponent<RectTransform>().anchoredPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

      //  miniMapCamera.transform.localPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.localPosition.z));

    }
}