using UnityEngine;
using ummisco.gama.unity.messages;
using System.Reflection;



namespace ummisco.gama.unity.topics
{
    public class GetTopic : Topic
    {

        protected string valueIs = "";
        public GetTopicMessage topicMessage;

        public GetTopic(GameObject gameObj) : base(gameObj)
        {

        }

        // Use this for initialization
        public override void Start()
        {

        }

        // Update is called once per frame
        public override void Update()
        {

        }

        public void ProcessTopic(object[] obj)
        {
            this.SetAllProperties(obj);
            if (targetGameObject != null)
            {
                string attribute = topicMessage.attribute;
                obj[2] = (object)GetValueToSend(attribute);

                Debug.Log("Method called and returned -> " + obj[2]);
            }
        }

        // The method to call Game Objects methods
        //----------------------------------------
        public string GetValueToSend(string attibute)
        {

            FieldInfo[] fieldInfoGet = targetGameObject.GetComponent(scripts[0].GetType()).GetType().GetFields();
            string msgReplay = "";

            foreach (FieldInfo fi in fieldInfoGet)
            {
                if (fi.Name.Equals(attibute))
                {
                    UnityEngine.Component ob = (UnityEngine.Component)targetGameObject.GetComponent(scripts[0].GetType());
                    msgReplay = fi.GetValue(ob).ToString();
                }
            }
            Debug.Log("To return this -> " + msgReplay);
            return msgReplay;
        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (GetTopicMessage)obj[0];
            this.targetGameObject = (GameObject)obj[1];
            this.valueIs = (string)obj[2];
            this.scripts = targetGameObject.GetComponents<MonoBehaviour>();
        }
    }
}