using UnityEngine;
using ummisco.gama.unity.messages;

namespace ummisco.gama.unity.topics
{
    public class ColorTopic : Topic
    {

        public ColorTopicMessage topicMessage;

        private static ColorTopic m_Instance;

        public static ColorTopic Instance { get { return m_Instance; } }

        void Awake()
        {
            if (m_Instance == null) m_Instance = this;
        }

        public ColorTopic(GameObject gameObj) : base(gameObj)
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

        public void ProcessTopic(object obj)
        {
            SetAllProperties(obj);
            SendTopic();
        }

        // The method to call Game Objects methods
        //----------------------------------------
        public void SendTopic()
        {
            targetGameObject.GetComponent<Renderer>().material.color = new Color(topicMessage.red, topicMessage.green, topicMessage.blue);  // Tools.stringToColor (color);
            Debug.Log("The color is : " + topicMessage.red + " - " + topicMessage.green + " - " + topicMessage.blue);

        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (ColorTopicMessage)obj[0];
            this.targetGameObject = (GameObject)obj[1];
            this.scripts = targetGameObject.GetComponents<MonoBehaviour>();
        }

    }

}