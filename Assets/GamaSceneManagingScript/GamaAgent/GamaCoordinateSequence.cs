
using System.Collections.Generic;
using UnityEngine;

namespace ummisco.gama.unity.GamaAgent
{
    public struct GamaCoordinateSequence
    {

        public List<GamaPoint> Points { get; set; }

        public GamaCoordinateSequence(List<GamaPoint> coordinates)
        {
            this.Points = coordinates;
        }

        public override string ToString()
        {
            string listInString = "[";
            foreach (var el in Points)
            {
                listInString += string.Format("({0}, {1}, {2}),", el.x, el.y, el.z);
            }
            listInString += "]";
            return listInString;
        }

        public Vector2[] getVector2Coordinates()
        {
           
            if(Points.Count == 0) {return null;}
            Vector2[] coord = new Vector2[Points.Count];
            for (int i = 0; i < Points.Count; i++)
            {
                Vector2 vect = new Vector2(Points[i].x, Points[i].y);
                coord[i] = vect;
            }
            return coord;
        }

        public Vector3[] getVector3Coordinates()
        {
            
            if(Points.Count == 0) {return null;}
            Vector3[] coord = new Vector3[Points.Count];
            for (int i = 0; i < Points.Count; i++)
            {
                Vector3 vect = new Vector3(Points[i].x, Points[i].y, Points[i].z);
                coord[i] = vect;
            }
            return coord;
        }



    }
}