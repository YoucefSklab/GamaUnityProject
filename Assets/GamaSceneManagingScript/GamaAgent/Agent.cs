using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Nextzen.VectorData;

namespace ummisco.gama.unity.GamaAgent
{
    public class Agent 
    
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

