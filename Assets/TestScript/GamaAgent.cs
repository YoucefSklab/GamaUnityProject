using System.IO;
using System.Xml;
using System.Xml.Linq;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.Scene;
using UnityEngine;

public class GamaAgent : MonoBehaviour
{

    public int intVar = 0;
    public string stringVar = "";
    public float floatVar = 0.0f;
    public bool boolVar = false;

    [SerializeField]
    public int strength
    {
        get { return m_strength; }
        set
        {
            m_strength = value;
            string msg = GamaListenReplay.BuildToListenReplay("strength", strength);
            GamaManager.connector.Publish("setexp", msg);
            Debug.Log("------ > " + msg);
        }
    }
    public int m_strength = 0;

    void Start()
    {
     
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 250, 200, 20), "Send to listen"))
          {
            strength += 2;
        }
    }




        void Update()
    {

        var nbrObject = GameObject.FindObjectsOfType(typeof(GameObject)).Length;
       // Debug.Log("The total of agents is " + nbrObject);
        GameObject target = null;
        bool isMouseDrag = false;
        Vector3 screenPosition = new Vector3();
        Vector3 offset = new Vector3();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null)
            {
                isMouseDrag = true;
                Debug.Log("target position :" + target.transform.position);
                //Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDrag = false;
        }

        if (isMouseDrag)
        {
            //track mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }

    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}