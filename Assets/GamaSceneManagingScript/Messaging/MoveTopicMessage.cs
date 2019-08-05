using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ummisco.gama.unity.datastructure;
using ummisco.gama.unity.GamaAgent;

namespace ummisco.gama.unity.messages
{

	[System.Xml.Serialization.XmlRoot ("ummisco.gama.unity.messages.MoveTopicMessage")]
	public class MoveTopicMessage : TopicMessage
	{

        [XmlElement("position")]
        public GamaPoint position { set; get; }
        [XmlElement("speed")]
        public float speed { set; get; }
        [XmlElement("smoothMove")]
        public bool smoothMove { set; get; }

		public MoveTopicMessage()
		{

		}

		public MoveTopicMessage (string unread, string sender, string receivers, string contents, string emissionTimeStamp, string objectName, GamaPoint position, float speed, bool smoothMove) : base (unread, sender, receivers, contents, objectName, emissionTimeStamp)
		{
			this.position = position;
			this.speed = speed;
			this.smoothMove = smoothMove;
		}


	}

}

