using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class DefCote : MonoBehaviour
    {

        public int dike_id;
        public string type;
        public float height;
        public string status;  //  "bon" "moyen" "mauvais" 
        public int length_def_cote;
        public int meshElevation = 30;

        public Vector3 localScale = new Vector3(0.2f, 0.2f, 0.2f);


        private CanvasGroup cg;

        Ray ray;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            cg = GameObject.Find("Canvas_Tips").GetComponent<CanvasGroup>();
        }

        public void UAInit(UnityAgent unityAgent)
        {
            Agent agent = unityAgent.GetAgent();
            MeshCreator meshCreator = new MeshCreator();
            gameObject.AddComponent(typeof(MeshRenderer));
            gameObject.AddComponent(typeof(MeshFilter));

            gameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(meshElevation, agent.agentCoordinate.getVector2Coordinates());
            gameObject.GetComponent<MeshFilter>().mesh.name = "Mesh_" + agent.name;
            //gameObject.GetComponent<Renderer>().material = mat;
            gameObject.transform.localScale = localScale;
            gameObject.AddComponent<MeshCollider>();
        
            this.dike_id = 12;
            this.type = "type";
            this.height = 12f;
            this.status = "status";  //  "bon" "moyen" "mauvais" 
            this.length_def_cote = 12;
        
    }



        // Update is called once per frame
        void Update()
        {

        }

        public void showTip()
        {
            Debug.Log("Test is Test");
        }

        void OnMouseDown()
        {

            Debug.Log("This on mouse down -> " + gameObject.name);
        }

        void OnMouseOver()
        {

            Vector3 vect = worldToUISpace(GameObject.Find("Canvas_Tips").GetComponent<Canvas>(), Input.mousePosition);
            vect.z = -400f;
            GameObject.Find("Tips").GetComponent<RectTransform>().transform.position = vect;

            SetInfo();
            cg.interactable = true;
            cg.alpha = 1;
        }

        public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
        {
            Vector3 screenPos = worldPos;
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);

            return parentCanvas.transform.TransformPoint(movePos);
        }

        void OnMouseExit()
        {
            ResetSetInfo();
            cg.interactable = false;
            cg.alpha = 0;
        }

        void SetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "Dike Id: "+dike_id;
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Type: "+type;
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Height: "+height;
            GameObject.Find("attribute_4").GetComponent<Text>().text = "Status: "+status;
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Def Cote Lenght: "+length_def_cote;
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Full name: review DefCote";
        }


        void ResetSetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "Dike Id: ";
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Type: ";
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Height: ";
            GameObject.Find("attribute_4").GetComponent<Text>().text = "Status: ";
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Def Cote Lenght: ";
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Full name: ";
        }

        public Vector3 GetNewPosition()
        {
            return new Vector3(-2218, -963f, 100f);
        }

    }
}
