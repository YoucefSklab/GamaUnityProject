namespace ummisco.gama.unity.datastructure
{
   // [XmlRoot("entry")]
    public class ExposeEntry
    {

       
       // [XmlElement] 
        public string attribute { get; set; }
       // [XmlElement] 
        public string value { get; set; }
       
        
        public ExposeEntry(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }
    }
}






