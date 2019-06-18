using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.littosim;
using UnityEngine;
using UnityEngine.EventSystems;

// TODO this class in no longer needed as the UI element are hiden by the mask defined on the MapCanvas.
public class CheckIfContainedInCanvas : MonoBehaviour
{

    public Canvas canvas;

    // Use this for initialization
    [ExecuteInEditMode]
    void Start()
    {
        canvas = GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().mapCanvas;

    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("OnPointerEnter called.");
    }


    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("OnPointerDownDelegate called.");
    }

    // Update is called once per frame
    void Update()
    {

        /*
        Vector3[] v = getCanvasWorldCorners();
        Vector3[] corners = new Vector3[2];
        corners[0] = v[0];
        corners[1] = v[2];
        Vector3 p = gameObject.transform.position;

        isInCanvas(corners,p);
          */

      //  checkUpdate();


    }


    // TODO: toDelete 
    public void checkUpdate()
    {
        if (Input.GetMouseButtonDown(0)){
           
            if (gameObject.transform.tag == "clickableCube"){
                Debug.Log("Ceci est un test sur le map panel -> "+gameObject.transform.tag);
            }
        }
    }

    Vector3[] getCanvasWorldCorners()
    {
       // canvas = GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().mapCanvas;
        Vector3[] v = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(v);  
        return v;
    }

    public void isInCanvas(Vector3[] corners, Vector3 p)
    {
        Vector3 p1 = corners[0];
        Vector3 p2 = corners[1];

        if (CheckPointInCanvas(p1, p2, p))
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("Yes - -- " + gameObject.name);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("No - -- " + gameObject.name);
        }       
    }


    bool CheckPointInCanvas(Vector3 p1, Vector3 p2, Vector3 p)
    {
        if (p.x > p1.x && p.x < p2.x && p.y > p1.y && p.y < p2.y)
            return true;

        return false;
    }   

}
