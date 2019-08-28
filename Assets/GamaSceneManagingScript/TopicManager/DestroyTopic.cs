using UnityEngine;
using ummisco.gama.unity.messages;



namespace ummisco.gama.unity.topics
{
    public class DestroyTopic : Topic
    {

        public DestroyTopicMessage topicMessage;
        public GameObject objectToDestroy;

        public DestroyTopic(DestroyTopicMessage topicMessage, GameObject gameObj) : base(gameObj)
        {
            this.topicMessage = topicMessage;
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
            // Debug.Log ("Order received. Let's destroy the GameObject ");
            Destroy(GameObject.Find(topicMessage.objectName));
        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (DestroyTopicMessage)obj[0];
        }
    }
}