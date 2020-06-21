using System;
using System.IO;
using System.Xml;

namespace ummisco.gama.unity.messages
{
    public static class GamaListenReplay
    {
        
        public static string BuildToListenReplay(string attributeName, object attributeValue)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.DocumentElement;
            XmlElement mapElement = doc.CreateElement(string.Empty, "map", string.Empty);
            doc.AppendChild(mapElement);

            XmlElement entryElement = doc.CreateElement(string.Empty, "entry", string.Empty);
            mapElement.AppendChild(entryElement);

            XmlElement attributeNameElement = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText textName = doc.CreateTextNode(attributeName);
            attributeNameElement.AppendChild(textName);
            entryElement.AppendChild(attributeNameElement);

            XmlElement element4 = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText textValue = doc.CreateTextNode(attributeValue + "");
            element4.AppendChild(textValue);
            entryElement.AppendChild(element4);


            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            doc.WriteTo(xmlTextWriter);

            return stringWriter.ToString();
        }

    }
}