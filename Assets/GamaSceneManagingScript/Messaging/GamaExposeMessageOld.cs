using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using ummisco.gama.unity.datastructure;

namespace ummisco.gama.unity.messages
{

    [XmlRoot("map")]
    [XmlInclude(typeof(ExposeEntry))]
    public class GamaExposeMessageOld 
    {
        [XmlElement("entry")]
        public List<XmlElement> listAttributes { set; get; }
               

        public GamaExposeMessageOld()
        {

        }               

    }

}

