using System.Collections;
using UnityEngine;
using ummisco.gama.unity.littosim;
using ummisco.gama.unity.datastructure;
using ummisco.gama.unity.geometry;
using System.Collections.Generic;
using ummisco.gama.unity.Scene;

namespace ummisco.gama.unity.GamaAgent
{
    public class Agent : MonoBehaviour

    {
        private string _geometry;

        public string agentName { set; get; }
        public GamaCoordinateSequence agentCoordinate { set; get; }
        public GamaColor color { set; get; }
        public Vector3 rotation { set; get; }
        public Vector3 location { set; get; }
        public Vector3 scale { set; get; }
        public bool isRotate { set; get; }
        public bool isOnInputMove { set; get; }
        public Rigidbody rb { set; get; }
        public float speed { set; get; }
        public string species { set; get; }
        public int index { set; get; }
        public string nature { get; set; }
        public string geometry { get; set; }
        public List<AgentAttribute> attributes { set; get; }

        public string type { set; get; }
        public float height { get; set; }

        public bool isDrawed { get; set; }


        public Agent(string agentName)
        {
            this.agentName = agentName;
            this.isDrawed = false;
            this.height = 0.0f;
        }

        public Vector2[] ConvertVertices()
        {
            Vector3[] vertices = this.agentCoordinate.getVector3Coordinates();
            Vector2[] newVertices = new Vector2[vertices.Length];

            GameObject uiManager = GameObject.Find(IUILittoSim.UI_MANAGER);
            Canvas canvas = GameObject.Find("MapCanvas").GetComponent<Canvas>();

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vect = vertices[i];
                Vector3 p = UIManager.worldToUISpace(canvas, vect);
                Vector2 p2 = p;
                newVertices[i] = p2;
            }
            return newVertices;
        }

        public Agent()
        {
            this.isDrawed = false;
            this.height = 0.0f;
        }

        public string getCollection()
        {
            if (species != null) return species;

            switch (geometry)
            {
                case IGeometry.POLYGON:
                    return "buildings";
                case IGeometry.LINESTRING:
                    return "rouds";
                case IGeometry.POINT:
                    return "objects";
                case IGeometry.WATER:
                    return "water";
                case IGeometry.LANDUSE:
                    return "landuse";
                default:
                    return IGeometry.EARTH;
            }
        }

        public string getLayer()
        {

            switch (geometry)
            {
                case IGeometry.POLYGON:
                    return "Buildings";
                case IGeometry.LINESTRING:
                    return "Rouds";
                case IGeometry.POINT:
                    return "Objects";
                case IGeometry.WATER:
                    return "Water";
                case IGeometry.LANDUSE:
                    return "Landuse";
                default:
                    return IGeometry.EARTH;
            }
        }

        public string getAttributeValue(string atName)
        {
            foreach (AgentAttribute attr in attributes)
            {
                if (attr.name.Equals(atName))
                {
                    return attr.value;
                }
            }
            return null;
        }


        public void SetAttributes(Agent agent)
        {
            this.name = agent.agentName;
            this.agentCoordinate = agent.agentCoordinate;
            this.color = agent.color;
            this.rotation = agent.rotation;
            this.location = agent.location;
            this.scale = agent.scale;
            this.isRotate = agent.isRotate;
            this.isOnInputMove = agent.isOnInputMove;
            this.rb = agent.rb;
            this.speed = agent.speed;
            this.species = agent.species;
            this.index = agent.index;
            this.nature = agent.nature;
            this.geometry = agent.geometry;
            this.attributes = agent.attributes;
        }

        public void InitAgent(RectTransform rtParent, bool elevate, float zAxis)
        {
            SetMeshVerticesPosition(rtParent, elevate, zAxis);
        }


        public void SetMeshVerticesPosition(RectTransform rtParent, bool elevate, float zAxis)
        {

            MeshCreator meshCreator = new MeshCreator();
            MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
            MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
            
            for (var i = 0; i < meshFilter.mesh.vertices.Length; i++)
            {
                //  Debug.Log(" ------->>>>>>>      Vertices " + i + " position is: " + meshFilter.mesh.vertices[i]);

                // vertices[i] = new Vector3(vert.x + diffX[i], -(vert.y + diffY[i]), vert.z);
                //vertices[i] = new Vector3(location.x - vert.x, location.y - vert.y, vert.z);
                //vertices[i] += Vector3.up * Time.deltaTime;
            }


            float elvation = elevate ? this.height : 0;
            elvation = 40;
            //transform.localPosition = new Vector3(this.location.x, -this.location.y, zAxis);

            // transform.position = new Vector3(this.location.x, this.location.y, zAxis);



            meshFilter.mesh.Clear();
            meshFilter.mesh = meshCreator.CreateMesh2(elvation, this.agentCoordinate.getVector2Coordinates());


            meshFilter.mesh.name = "CustomMesh";

            Material mat = new Material(Shader.Find("Specular"));
            //mat.color = agent.color.getColorFromGamaColor();
            mat.color = Color.blue;
            meshRenderer.material = mat;
            //meshCollider.sharedMesh = meshFilter.mesh;

            //transform.localPosition = new Vector3(this.location.x, this.location.y);

            Vector3 newPosition = SceneManager.worldEnveloppeCanvas.transform.InverseTransformPoint(new Vector3(this.location.x, -this.location.y));
            newPosition = UIManager.worldToUISpace(SceneManager.worldEnveloppeCanvas, new Vector3(this.location.x, -this.location.y));
           
            newPosition = SceneManager.worldEnveloppeRT.InverseTransformPoint(new Vector3(this.location.x, -this.location.y));

            Debug.Log("Corresponding world position is : " + newPosition);

            transform.SetParent(SceneManager.worldEnveloppeRT);
            transform.localPosition = newPosition;

            GameObject objet = new GameObject("Test");

            meshRenderer = (MeshRenderer)objet.AddComponent(typeof(MeshRenderer));
            meshFilter = (MeshFilter)objet.AddComponent(typeof(MeshFilter));

            objet.AddComponent<RectTransform>();

            objet.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            objet.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            objet.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
           // objet.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
            
            objet.transform.SetParent(SceneManager.worldEnveloppeRT);
            objet.transform.localPosition = new Vector3(this.location.x, -this.location.y);
            Vector3 oldPosition = objet.transform.position;

           
            meshFilter.mesh = meshCreator.CreateMesh2(elvation, this.agentCoordinate.getVector2Coordinates());

            objet.transform.position = oldPosition;
            meshRenderer.transform.localPosition = oldPosition;
            meshRenderer.transform.position = oldPosition;
            

            Debug.Log(" The canvas localPosition center is : " + SceneManager.worldEnveloppeCanvas.transform.localPosition);
            Debug.Log(" The canvas position center is : " + SceneManager.worldEnveloppeCanvas.transform.position);
            Debug.Log(" The canvas position center is : " + SceneManager.worldEnveloppeRT.localPosition);
            Debug.Log(" The canvas position center is : " + SceneManager.worldEnveloppeRT.position);
            Debug.Log(" The canvas position center is : " + SceneManager.worldEnveloppeRT.anchoredPosition);
            Debug.Log(" The canvas position center is : " + SceneManager.worldEnveloppeRT.anchoredPosition3D);

            //transform.localPosition = new Vector3(this.location.x, -this.location.y);
            //transform.position = newPosition;
            // RectTransform rt = (gameObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();
            /*
           rt.SetParent(rtParent);
           rt.anchoredPosition = new Vector3(this.location.x, -this.location.y, zAxis);
           rt.anchorMin = new Vector2(0, 1);
           rt.anchorMax = new Vector2(0, 1);
           rt.pivot = new Vector2(0, 1);
           rt.sizeDelta = new Vector2(10, 10);
           */

        }

        public void SetMeshVerticesPositionOld(bool elevate, float zAxis)
        {

            MeshCreator meshCreator = new MeshCreator();
            MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
            MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));


            for (var i = 0; i < meshFilter.mesh.vertices.Length; i++)
            {
                //  Debug.Log(" ------->>>>>>>      Vertices " + i + " position is: " + meshFilter.mesh.vertices[i]);

                // vertices[i] = new Vector3(vert.x + diffX[i], -(vert.y + diffY[i]), vert.z);
                //vertices[i] = new Vector3(location.x - vert.x, location.y - vert.y, vert.z);
                //vertices[i] += Vector3.up * Time.deltaTime;
            }


            float elvation = elevate ? this.height : 0;
            //transform.localPosition = new Vector3(this.location.x, -this.location.y, zAxis);

            transform.position = new Vector3(this.location.x, -this.location.y, zAxis);
















            meshFilter.mesh.Clear();
            meshFilter.mesh = meshCreator.CreateMesh2(elvation, this.agentCoordinate.getVector2Coordinates());


            meshFilter.mesh.name = "CustomMesh";

            Material mat = new Material(Shader.Find("Specular"));
            //mat.color = agent.color.getColorFromGamaColor();
            mat.color = Color.blue;
            meshRenderer.material = mat;
            //meshCollider.sharedMesh = meshFilter.mesh;






            Vector3[] vertices;
            vertices = meshFilter.mesh.vertices;
            Matrix4x4 localToWorld = transform.localToWorldMatrix;

            float[] diffX = new float[meshFilter.mesh.vertices.Length];
            float[] diffY = new float[meshFilter.mesh.vertices.Length];

            // compute diff
            for (var i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                diffX[i] = location.x - v.x;
                diffY[i] = location.y - v.y;
            }

            //transform.localPosition = new Vector3(this.location.x, this.location.y);

            for (var i = 0; i < vertices.Length; i++)
            {
                Debug.Log("Vertices " + i + " position is: " + vertices[i]);

                Vector3 world_v = localToWorld.MultiplyPoint3x4(vertices[i]);
                //Vector2 newP = new Vector2(0, 0);
                // world_v = transform.TransformPoint(vertices[i]);
                //world_v = new Vector3(world_v.x - 800, world_v.y - 800, world_v.z);


                // RectTransformUtility.ScreenPointToLocalPointInRectangle(SceneManager.worldEnveloppeRT, world_v, SceneManager.worldEnveloppeCanvas.worldCamera, out newP);
                world_v = Camera.main.transform.TransformPoint(world_v);
                vertices[i] = world_v;

                // vertices[i] = new Vector3(vert.x + diffX[i], -(vert.y + diffY[i]), vert.z);
                //vertices[i] = new Vector3(location.x - vert.x, location.y - vert.y, vert.z);
                //vertices[i] += Vector3.up * Time.deltaTime;
            }

            meshFilter.mesh.SetVertices(vertices);
            meshFilter.mesh.Optimize();










            for (var i = 0; i < meshFilter.mesh.vertices.Length; i++)
            {
                Debug.Log("New Vertices " + i + " position is: " + meshFilter.mesh.vertices[i]);
                //Vector3 vert = vertices[i];
                //vertices[i] = new Vector3(vert.x, -vert.y, vert.z);

            }

            // assign the local vertices array into the vertices array of the Mesh.
            // mesh.vertices = vertices;
            // mesh.RecalculateBounds();


        }
    }
}

