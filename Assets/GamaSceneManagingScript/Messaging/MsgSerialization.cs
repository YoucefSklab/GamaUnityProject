using System.IO;
using System.Xml.Serialization;
using System.Xml;



namespace ummisco.gama.unity.messages
{
    public static class MsgSerialization
    {

        public static TopicMessage FromXML(string aciResponseData)
        {
            TopicMessage msg;
            using (TextReader sr = new StringReader(aciResponseData))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TopicMessage));
                TopicMessage result = (TopicMessage)serializer.Deserialize(sr);
                msg = result;
            }
            return msg;
        }


        public static SetTopicMessage SesSetTopicMsg(string aciResponseData)
        {
            SetTopicMessage msg;
            using (TextReader sr = new StringReader(aciResponseData))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SetTopicMessage));
                SetTopicMessage result = (SetTopicMessage)serializer.Deserialize(sr);
                msg = result;
            }
            return msg;
        }

        public static object FromXML(string aciResponseData, object classDeserialization)
        {
            object msg;
            using (TextReader sr = new StringReader(aciResponseData))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(classDeserialization.GetType());

                object result = (object)serializer.Deserialize(sr);
                msg = result;
            }
            return msg;
        }

        public static ItemAttributes MsgItemDeserialization(string aciResponseData)
        {
            ItemAttributes msg;
            using (TextReader sr = new StringReader(aciResponseData))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ItemAttributes));
                ItemAttributes result = (ItemAttributes)serializer.Deserialize(sr);
                msg = result;
            }
            return msg;
        }

        public static string FromXML(string aciResponseData, string att)
        {
            using (TextReader sr = new StringReader(aciResponseData))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TopicMessage));
                TopicMessage msg = (TopicMessage)serializer.Deserialize(sr);
                switch (att)
                {
                    case IMessagingConcept.UNREAD:
                        return msg.unread;
                    case IMessagingConcept.SENDER:
                        return msg.sender;
                    case IMessagingConcept.RECEIVERS:
                        return msg.receivers;
                    case IMessagingConcept.CONTENTS:
                        return msg.contents;
                    case IMessagingConcept.EMISSION_TIMESTAMP:
                        return msg.emissionTimeStamp;
                    default:
                        return null;
                }
            }
        }


        public static string ToXML(GamaReponseMessage msgResponseData)
        {
            XmlSerializer serializer = new XmlSerializer(msgResponseData.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {           //using (StringWriter writer = new StringWriter ()) 

                // removes namespace
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);


                serializer.Serialize(writer, msgResponseData, xmlns);

                return stream.ToString();
                //return writer.ToString ();
            }

        }


        public static string ToXML(object msgResponseData)
        {

            XmlSerializer serializer = new XmlSerializer(msgResponseData.GetType());

            using (StringWriter writer = new StringWriter())
            {

                serializer.Serialize(writer, msgResponseData);

                return writer.ToString();
            }
        }

        public static string SerializationPlainXml(object msgResponseData)
        {

            XmlSerializer serializer = new XmlSerializer(msgResponseData.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                //using (StringWriter writer = new StringWriter ()) 
                // removes namespace
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);

                serializer.Serialize(writer, msgResponseData, xmlns);
                return stream.ToString();
            }
        }
    }
}