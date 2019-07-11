using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Nextzen.VectorData;
using ummisco.gama.unity.littosim;

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

            for(int i=0; i < vertices.Length; i++)
            {
                Vector3 vect = vertices[i];
                Vector3 p = uiManager.GetComponent<UIManager>().worldToUISpace(canvas, vect);
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
                case IGeometry.POINTS:
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
                case IGeometry.POINTS:
                    return "Objects";
                case IGeometry.WATER:
                    return "Water";
                case IGeometry.LANDUSE:
                    return "Landuse";
                default:
                    return IGeometry.EARTH;
            }
        }
        
    }
}

