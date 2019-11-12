using System.Collections.Generic;
using System.Xml.Serialization;
using ummisco.gama.unity.datastructure;
using ummisco.gama.unity.geometry;

namespace ummisco.gama.unity.GamaAgent
{
    [System.Serializable]
    [XmlRoot("msi.gama.extensions.messaging.GamaMessage")]
    [XmlInclude(typeof(Content))]
    [XmlInclude(typeof(AgentAttribute))]
    public class UnityAgent
    {
        private readonly string agentName;

        public string unread { get; set; }
        public string sender { get; set; }
        public string receivers { get; set; }
        public int emissionTimeStamp { get; set; }
        public Content contents { get; set; }



        public UnityAgent(string agentName)
        {
            this.agentName = agentName;
        }

        public UnityAgent()
        {

        }

        public Agent GetAgent()
        {
            Agent agent = new Agent(this.contents.agentName)
            {
                IsDrawed = false,
                IsRotate = false,
                Species = this.contents.species,
                Location = this.contents.location.toVector3D(),
                Geometry = this.contents.geometryType,
                Height = this.contents.height,
                Color = this.contents.color,
                AgentCoordinate = GetCoordinateSequence(),
                Attributes = this.contents.attributes
            };

            return agent;
        }

        public GamaCoordinateSequence GetCoordinateSequence()
        {
            List<GamaPoint> listPoints = new List<GamaPoint>();

            foreach (GamaPoint point in contents.vertices)
            {
                listPoints.Add(point);
            }

            GamaCoordinateSequence coordinateSequence = new GamaCoordinateSequence(listPoints);
            ajustPointsList(coordinateSequence.Points);
            return coordinateSequence;
        }

        public void ajustPointsList(List<GamaPoint> list)
        {
            if ((this.contents.geometryType.Equals(IGeometry.POLYGON)) & (list.Count >= 2))
            {
                list.Remove(list[list.Count - 1]);
            }

        }
    }

    [XmlRoot("UnityAgent")]
    public class Content
    {
        [XmlElement("agentName")]
        public string agentName { get; set; }
        [XmlElement("species")]
        public string species { get; set; }
        [XmlElement("geometryType")]
        public string geometryType { get; set; }

        public List<GamaPoint> vertices { get; set; }
        public GamaColor color { get; set; }
        [XmlElement("height")]
        public float height { get; set; }
        [XmlElement("location")]
        public GamaPoint location { get; set; }

        public List<AgentAttribute> attributes { set; get; }


        public Content()
        {

        }
    }







}

