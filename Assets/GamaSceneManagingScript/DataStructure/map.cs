using System;


namespace ummisco.gama.unity.messages
{
    [System.Xml.Serialization.XmlRoot("map")]
    public class map
    {
        public Entry entry;

        public map(string name, object value)
        {

            this.entry = new Entry(name, value);
        }

        public map()
        {

        }

        public void SetParameters(string name, object value)
        {
            this.entry = new Entry(name, value);
        }
    }

    class entry
    {
        public string name;
        public object value;

        public entry(string n, object v)
        {
            this.name = n;
            this.value = v;
        }
    }

}

