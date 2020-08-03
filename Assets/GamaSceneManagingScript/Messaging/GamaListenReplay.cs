using System;
using System.IO;
using System.Xml;

namespace ummisco.gama.unity.messages
{
    public static class GamaListenReplay
    {
       public static string BuildToListenReplay(string attributeName, string attributeValue)
        {
            XmlDoc xmlDoc = new XmlDoc();

            xmlDoc.AddXmlElement(attributeName, (string) attributeValue);
			
            xmlDoc.doc.WriteTo(xmlDoc.xmlTextWriter);

            return xmlDoc.stringWriter.ToString();
        }

        public static string BuildToListenReplay(string attributeName, int attributeValue)
        {
            XmlDoc xmlDoc = new XmlDoc();

            xmlDoc.AddXmlElement(attributeName,  attributeValue);

            xmlDoc.doc.WriteTo(xmlDoc.xmlTextWriter);

            return xmlDoc.stringWriter.ToString();
        }

        public static string BuildIntToListenReplay(string attributeName, int attributeValue)
        {
            XmlDoc xmlDoc = new XmlDoc();

            xmlDoc.AddXmlElement(attributeName, attributeValue);

            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

            xmlDoc.doc.WriteTo(xmlTextWriter);

            return stringWriter.ToString();
        }

    }

	public class XmlDoc
	{
        public XmlDocument doc = new XmlDocument();
        public XmlElement mapElement;
        public XmlElement entryElement;

        public StringWriter stringWriter;
        public XmlTextWriter xmlTextWriter;

        public XmlDoc()
		{
            stringWriter = new StringWriter();
            xmlTextWriter = new XmlTextWriter(stringWriter);

            NewXmlDocument();
            NewMapElement();
            NewEntryElement();
			  
            doc.AppendChild(mapElement);
			mapElement.AppendChild(entryElement);
        }
				
        public void NewXmlDocument()
		{
            doc = new XmlDocument();
        }

        public XmlElement CreateElement(string eltName)
        {
            return doc.CreateElement(string.Empty, eltName, string.Empty);
        }

        public void NewMapElement()
        {
            mapElement = CreateElement("map");
        }

        public void NewEntryElement()
        {
            entryElement = CreateElement("entry");
        }

		public void AddXmlElement(string attributeName, string attributeValue)
		{
			XmlElement attributeNameElement = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText textName = doc.CreateTextNode(attributeName);
            attributeNameElement.AppendChild(textName);
            entryElement.AppendChild(attributeNameElement);

            XmlElement attributeValueElement = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText textValue = doc.CreateTextNode(attributeValue);
            attributeValueElement.AppendChild(textValue);
            entryElement.AppendChild(attributeValueElement);
        }

        public void AddXmlElement(string attributeName, int attributeValue)
        {
            XmlElement attributeNameElement = doc.CreateElement(string.Empty, "string", string.Empty);
            XmlText textName = doc.CreateTextNode(attributeName);
            attributeNameElement.AppendChild(textName);
            entryElement.AppendChild(attributeNameElement);

            XmlElement attributeValueElement = doc.CreateElement(string.Empty, "int", string.Empty);
            XmlText textValue = doc.CreateTextNode(attributeValue+"");

        
			
            attributeValueElement.AppendChild(textValue);
            entryElement.AppendChild(attributeValueElement);
        }


    }
}