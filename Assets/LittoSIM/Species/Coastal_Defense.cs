using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class Coastal_Defense : MonoBehaviour
    {
        public int coast_def_id;
        public string type;
        public string district_code;
        public Color color;
        public float height = 40;
        public bool ganivelle;
        public float alt = 0;
        public string status;
        public int length_coast_def;

        public Vector3 localScale = new Vector3(0.2f, 0.2f, 0.2f);
        private CanvasGroup cg;

        Ray ray;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            cg = GameObject.Find("Canvas_Tips").GetComponent<CanvasGroup>();
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
            GameObject.Find("attribute_1").GetComponent<Text>().text = "coast_def_id: " + coast_def_id;
            GameObject.Find("attribute_2").GetComponent<Text>().text = "type: " + type;
            GameObject.Find("attribute_3").GetComponent<Text>().text = "district_code: " + district_code;
            GameObject.Find("attribute_4").GetComponent<Text>().text = "height: " + height;
            GameObject.Find("attribute_5").GetComponent<Text>().text = "ganivelle: " + ganivelle;
            GameObject.Find("attribute_6").GetComponent<Text>().text = "alt: " + alt;
        }


        void ResetSetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "coast_def_id: ";
            GameObject.Find("attribute_2").GetComponent<Text>().text = "type: ";
            GameObject.Find("attribute_3").GetComponent<Text>().text = "district_code: ";
            GameObject.Find("attribute_4").GetComponent<Text>().text = "height: ";
            GameObject.Find("attribute_5").GetComponent<Text>().text = "ganivelle: ";
            GameObject.Find("attribute_6").GetComponent<Text>().text = "alt: ";

        }



        public Vector3 GetNewPosition()
        {
            return new Vector3(-2218, -963f, 100f);
        }

    }
}
