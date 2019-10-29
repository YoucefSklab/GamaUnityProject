
using System;
using System.Xml.Serialization;
using UnityEngine;

namespace ummisco.gama.unity.datastructure
{
    [XmlRoot("color")]
    public struct GamaColor
    {

        public float value { get; set; }
        public float falpha { get; set; }
        public string name { get; set; }

        public GamaColor(float value, float falpha, string name)
        {
            this.value = value;
            this.falpha = falpha;
            this.name = name;
        }


        public override string ToString(){
            return string.Format("({0}, {1}, {2})", value, falpha,name);
        }

        public Color GetRgb()
        {
            Color newColor = new Color();

            var bigint = (int)this.value;
            var r = (bigint >> 16) & 255;
            var g = (bigint >> 8) & 255;
            var b = bigint & 255;
            var alpha = Convert.ToInt32(falpha);

            newColor.r = r;
            newColor.b = b;
            newColor.g = g;
            newColor.a = alpha;

            return newColor;
        }


    }
}