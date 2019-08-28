using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ummisco.gama.unity.datastructure;
using UnityEngine;

namespace ummisco.gama.unity.messages
{
    public class GamaExposeMessage
    {

        public string messageContent;
        //public List<ExposeEntry> listAttributes; //{ set; get; }
        public Dictionary<string, string> attributesList = new Dictionary<string, string>();


        public GamaExposeMessage(string message)
        {
            this.messageContent = message;
            ParseMessage();
        }


        public void ParseMessage()
        {

            string xml = @messageContent;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNodeList titles = xmlDoc.GetElementsByTagName("entry");

            foreach (XmlNode elt in titles)
            {
                XmlNodeList childNodes = elt.ChildNodes;

                XmlNode attributeNode = childNodes[0];
                XmlNode valueNode = childNodes[1];

                attributesList.Add(attributeNode.InnerText, valueNode.InnerText);
            }
        }
    }
}

