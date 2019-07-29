using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.littosim;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {
    public Camera miniMapCamera;
    public Camera mainCamera;
    private Canvas uiCanvas;
    private GameObject uiManager;


    // Use this for initialization
    void Start () {
        uiManager = GameObject.Find("UIManager");
        uiCanvas = GameObject.Find("Main_Canvas").GetComponent<Canvas>();

    }
	
	// Update is called once per frame
	void Update () {
        //miniMapCamera.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.localPosition.z);
        //miniMapCamera.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -800));
        Vector3 p = uiManager.GetComponent<UIManager>().worldToUISpace(uiCanvas, Input.mousePosition);
        miniMapCamera.transform.position = new Vector3(p.x, p.y, -1050);

    }
}