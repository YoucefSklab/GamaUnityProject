using System;
using System.Xml.Serialization;

namespace ummisco.gama.unity.GamaAgent
{
    [XmlRoot("GamaPoint")]
    public class GamaPoint
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public GamaPoint()
        {

        }

         public GamaPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

         public GamaPoint(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y,z);
        }

        public UnityEngine.Vector3 toVector3D (){
            return new UnityEngine.Vector3(x,y,z);
        }
    }
}






