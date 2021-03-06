﻿using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class Land_Use : MonoBehaviour
                            , IPointerEnterHandler
                            , IPointerExitHandler
    {
        public int id;
        public string lu_name;
        public int lu_code;
        public string dist_code;
        public Color my_color;
        public int population;
        public float mean_alt;
        public string density_class;
        public int expro_cost;
        public bool is_urban_type;
        public bool is_adapted_type;
        public bool is_in_densification = false;
        public bool focus_on_me = false;
        public int meshElevation = 30;

        public Vector3 localScale = new Vector3(0.2f, 0.2f, 0.2f);


        private CanvasGroup cg;
        private Canvas canvas;
        private GameObject tips;

        Ray ray;
        RaycastHit hit;

        // Start is called before the first frame update
        void Start()
        {
            cg = GameObject.Find("Canvas_Tips").GetComponent<CanvasGroup>();
            canvas = GameObject.Find("Canvas_Tips").GetComponent<Canvas>();
            tips = GameObject.Find("Tips");

            
        }

        public void UAInit(UnityAgent unityAgent)
        {
           
        }

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
            Material myMat = Resources.Load("Materials/NewNaturelle", typeof(Material)) as Material;
            GetComponent<MeshRenderer>().material = myMat;
        }

        void OnMouseOver()
        {
            Debug.Log(" From Land_Use.cs Land_use : " + gameObject.name + " OnMouseOver Method. ");
            Vector3 vect = worldToUISpace(canvas, Input.mousePosition);
            vect.z = -4f;
            tips.GetComponent<RectTransform>().transform.position = vect;
            
            SetInfo();
            cg.interactable = true;
            cg.alpha = 1;

            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                     
            //VertsColor2();
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
            tips.GetComponent<RectTransform>().transform.position = new Vector3(-2500,400,-300);
            cg.interactable = false;
            cg.alpha = 0;

            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        }

        void SetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "ID: " + gameObject.GetComponent<Agent>().GetAttributeValue("id");// id;
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Name: " + gameObject.GetComponent<Agent>().GetAttributeValue("lu_name"); //  lu_name;
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Code: " + gameObject.GetComponent<Agent>().GetAttributeValue("lu_code"); //lu_code;
            GameObject.Find("attribute_4").GetComponent<Text>().text = "District Code: " + gameObject.GetComponent<Agent>().GetAttributeValue("dist_code");//dist_code;
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Population: " + gameObject.GetComponent<Agent>().GetAttributeValue("population"); //population;
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Mean alt: " + gameObject.GetComponent<Agent>().GetAttributeValue("mean_alt"); // mean_alt;
        }

        void ResetSetInfo()
        {
            GameObject.Find("attribute_1").GetComponent<Text>().text = "ID: ";
            GameObject.Find("attribute_2").GetComponent<Text>().text = "Name: ";
            GameObject.Find("attribute_3").GetComponent<Text>().text = "Code: ";
            GameObject.Find("attribute_4").GetComponent<Text>().text = "District Code: ";
            GameObject.Find("attribute_5").GetComponent<Text>().text = "Population: ";
            GameObject.Find("attribute_6").GetComponent<Text>().text = "Mean alt: ";
        }

        public Vector3 GetNewPosition()
        {
            return new Vector3(-2218, -963f, 100f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log(" ------------------------------>>  Exit");
          //  throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log(" ------------------------------>>  Enter ");
            // throw new System.NotImplementedException();
        }


        public void VertsColor()
        {
            Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;
            Vector3[] verticesModified = new Vector3[triangles.Length];
            int[] trianglesModified = new int[triangles.Length];
            Color32 currentColor = new Color32();
            Color32[] colors = new Color32[triangles.Length];
            for (int i = 0; i < trianglesModified.Length; i++)
            {
                // Makes every vertex unique
                verticesModified[i] = vertices[triangles[i]];
                trianglesModified[i] = i;
                // Every third vertex randomly chooses new color
              //  if (i % 3 == 0)
                {
                    currentColor = new Color(
                        Random.Range(0.0f, 1.0f),
                        Random.Range(0.0f, 1.0f),
                        Random.Range(0.0f, 1.0f),
                        1.0f
                    );
                }
                colors[i] = currentColor;
            }
            // Applyes changes to mesh
            mesh.vertices = verticesModified;
            mesh.triangles = trianglesModified;
            mesh.colors32 = colors;
        }

        public void VertsColor2()
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            int[] triangles = mesh.triangles;
            Vector3[] verts = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = mesh.uv;

            Vector3[] newVerts;
            Vector3[] newNormals;
            Vector2[] newUvs;

            int n = triangles.Length;
            newVerts = new Vector3[n];
            newNormals = new Vector3[n];
            newUvs = new Vector2[n];

            for (int i = 0; i < n; i++)
            {
                newVerts[i] = verts[triangles[i]];
                newNormals[i] = normals[triangles[i]];
                if (uvs.Length > 0)
                {
                    newUvs[i] = uvs[triangles[i]];
                }
                triangles[i] = i;
            }
            mesh.vertices = newVerts;
            mesh.normals = newNormals;
            mesh.uv = newUvs;
            mesh.triangles = triangles;

            Color[] colors = new Color[mesh.vertexCount];
            for (int i = 0; i < colors.Length; i += 3)
            {
                colors[i] = Color.red;
                colors[i + 1] = Color.green;
                colors[i + 2] = Color.blue;
            }
            mesh.colors = colors;
        }
    }
}
