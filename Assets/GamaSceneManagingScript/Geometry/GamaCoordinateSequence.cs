
using System.Collections.Generic;
using ummisco.gama.unity.datastructure;
using ummisco.gama.unity.Scene;
using UnityEngine;

namespace ummisco.gama.unity.geometry
{
    public struct GamaCoordinateSequence
    {

        public List<GamaPoint> Points { get; set; }

        public GamaCoordinateSequence(List<GamaPoint> coordinates)
        {
            this.Points = coordinates;
           // this.Points = TransformCoordinates(this.Points);
        }

        public List<GamaPoint> TransformCoordinates(List<GamaPoint> Points)
        {
            List<GamaPoint> transformedPoints = new List<GamaPoint>();
            foreach (GamaPoint p in Points)
            {
                GamaPoint newP = new GamaPoint(IGamaManager.x_axis_transform * p.x, IGamaManager.y_axis_transform * p.y, IGamaManager.z_axis_transform * p.z);
                transformedPoints.Add(newP);
            }
            return transformedPoints;
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

        public Vector2[] GetVector2Coordinates()
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

        public Vector3[] GetVector3Coordinates()
        {
            
            if(Points.Count == 0) {return null;}
            Vector3[] coord = new Vector3[Points.Count];
            for (int i = 0; i < Points.Count; i++)
            {
                Vector3 vect = new Vector3(Points[i].x, -Points[i].y, Points[i].z);
                coord[i] = vect;
            }
            return coord;
        }



    }
}