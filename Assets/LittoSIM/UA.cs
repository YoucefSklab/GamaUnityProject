using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class UA : MonoBehaviour
    {

        public string ua_name;
        public int ua_code;
        public int population;
        public string classe_densite;
        public int cout_expro;
        public string fullNameOfUAname;
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
            this.ua_name = agent.agentName;
            this.ua_code = 12;
            this.population = 12;
            this.cout_expro = 12;
            this.fullNameOfUAname = agent.agentName + "_FULLNAME_" + 12;
            this.classe_densite = agent.agentName + "_CLASSE_DENSITE_" + 12;
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
            GameObject.Find("attribute_1").GetComponent<Text>().text = "Name: " + ua_name;
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Code: " + ua_code;
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Population: " + population;
            GameObject.Find("attribute_4").GetComponent<Text>().text = "Classe densité: " + classe_densite;
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Cout expro: " + cout_expro;
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Full name: " + fullNameOfUAname;

        }


        void ResetSetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "Name: ";
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Code: ";
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Population: ";
            GameObject.Find("attribute_4").GetComponent<Text>().text = "Classe densité: ";
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Cout expro: ";
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Full name: ";

        }



        public Vector3 GetNewPosition()
        {
            return new Vector3(-2218, -963f, 100f);
        }

    }
}
