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
            map msg = new map("strength", strength);

            string message = MsgSerialization.ToXML(msg);

           // GamaManager.connector.Publish("setexp", message);

            // ----------------------------------___-
           
            XmlDocument doc = new XmlDocument();

            //(1) the xml declaration is recommended, but not mandatory
          
            XmlElement root = doc.DocumentElement;
            
            //(2) string.Empty makes cleaner code
            XmlElement element1 = doc.CreateElement(string.Empty, "map", string.Empty);
            doc.AppendChild(element1);

            XmlElement element2 = doc.CreateElement(string.Empty, "entry", string.Empty);
            element1.AppendChild(element2);

            XmlElement element3 = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText text1 = doc.CreateTextNode("strength");
            element3.AppendChild(text1);
            element2.AppendChild(element3);

            XmlElement element4 = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText text2 = doc.CreateTextNode(strength+"");
            element4.AppendChild(text2);
            element2.AppendChild(element4);
            

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            
            doc.WriteTo(xmlTextWriter);

            GamaManager.connector.Publish("setexp", stringWriter.ToString());

            Debug.Log("------ > " + stringWriter.ToString());

            //--------------------------------------
        }
    }
    public int m_strength = 0;

    void Start()
    {
        



    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 25, 200, 20), "Ajouter"))
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