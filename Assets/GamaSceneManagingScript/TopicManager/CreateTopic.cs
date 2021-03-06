﻿using UnityEngine;
using ummisco.gama.unity.messages;
using System.Xml;
using ummisco.gama.unity.GamaConcepts;
using ummisco.gama.unity.Scene;
using ummisco.gama.unity.Behaviour;
using ummisco.gama.unity.utils.converter;

namespace ummisco.gama.unity.topics
{
    public class CreateTopic : Topic
    {

        public CreateTopicMessage topicMessage;
        public GameObject newObject;
        public Color objectColor;

        public CreateTopic(CreateTopicMessage topicMessage, GameObject gameObj) : base(gameObj)
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
            //Debug.Log ("Order received. Let's create the object ");
            SetAllProperties(obj);
            SendTopic();
        }

        // The method to call Game Objects methods
        //----------------------------------------
        public void SendTopic()
        {

            GameObject objectManager = GameObject.Find(IGamaManager.GAMA_MANAGER);
            switch (topicMessage.type)
            {
                case IGamaConcept.CAPSULE:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
                case IGamaConcept.CUBE:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case IGamaConcept.CYLINDER:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case IGamaConcept.PLANE:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    break;
                case IGamaConcept.QUAD:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    break;
                case IGamaConcept.SPHERE:
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                default:
                    Debug.Log("Object's type not specified. So, Object with type Sphere will be created as default object! ");
                    newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
            }

            // Set the name of the game object
            //--------------------------------
            newObject.name = topicMessage.objectName;
            newObject.AddComponent<AgentBehaviour>();

            // Set the position to the new GameObject
            //---------------------------------------
            XmlNode[] positionNode = (XmlNode[])topicMessage.position;
            Vector3 objectPosition = ConvertType.Vector3FromXmlNode(positionNode, IGamaConcept.GAMA_POINT_CLASS_NAME);
            newObject.transform.position = objectPosition;

            Debug.Log(" Game Object position is " + objectPosition);


            XmlNode[] colorNode = (XmlNode[])topicMessage.color;
            objectColor = ConvertType.RgbColorFromXmlNode(colorNode, IGamaConcept.GAMA_RGB_COLOR_CLASS);

            objectColor.a = 1.0f;

            Renderer rend = newObject.GetComponent<Renderer>();
            rend.material.color = objectColor;

          
       

            GameObject aeentCreator = GameObject.Find(IGamaManager.AGENT_CREATOR);
            aeentCreator.AddComponent<AgentCreator>().AddAgentToContexte("GenericSpecies", newObject);
         


        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (CreateTopicMessage)obj[0];
        }
    }
}