using System;
using System.Xml.Serialization;

namespace ummisco.gama.unity.datastructure
{
    [XmlRoot("AgentAttribute")]
    public class AgentAttribute
    {
        [XmlElement("type")]
        public string type { get; set; }
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("value")]
        public string value { get; set; }

        public AgentAttribute()
        {

        }

        public AgentAttribute(string type, string name, string value)
        {
            this.type = type;
            this.name = name;
            this.value = value;
        }        
    }
}






