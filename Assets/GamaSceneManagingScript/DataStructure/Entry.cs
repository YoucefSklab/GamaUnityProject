using System;


namespace ummisco.gama.unity.messages
{
    [System.Xml.Serialization.XmlRoot("entry")]
    public class Entry
    {
        public string name;
        public object value;

        public Entry(string n, object v)
        {
            this.name = n;
            this.value = v;
        }

        public Entry()
        {

        }
    }

}

