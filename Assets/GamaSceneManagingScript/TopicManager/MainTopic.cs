using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.utils;
using System.Reflection;
using System.Linq;
using System;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.Scene;
using ummisco.gama.unity.Network;

namespace ummisco.gama.unity.topics
{
    public class MainTopic : Topic
    {

        public UnityAgent unityAgent;
        public GameObject newObject;
        public Color objectColor;


        public MainTopic(UnityAgent unityAgent, GameObject gameObj) : base(gameObj)
        {
            this.unityAgent = unityAgent;
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
            setAllProperties(obj);
            sendTopic();

        }


        // The method to call Game Objects methods
        //----------------------------------------
        public void sendTopic()
        {

            GameObject objectManager = getGameObjectByName(IMQTTConnector.GAMA_MANAGER_OBJECT_NAME, UnityEngine.Object.FindObjectsOfType<GameObject>());
            //Debug.Log("The content is: " + topicMessage.contents.ToString());

            // Agent gamaAgent = UtilXml.getAgent((XmlNode[])topicMessage.contents);
            /*
            Agent gamaAgent = unityAgent.GetAgent();
            GamaManager.gamaAgentList[GamaManager.nbrAgent] = gamaAgent;
            
            GamaManager.nbrAgent++;
            Debug.Log(" -->   New agent received ! " + gamaAgent.agentName);
            */

        }

        public override void setAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.unityAgent = (UnityAgent)obj[0];
        }


        public GameObject getGameObjectByName(string objectName, GameObject[] allObjects)
        {
            foreach (GameObject gameObj in allObjects)
            {
                if (gameObj.activeInHierarchy)
                {
                    if (objectName.Equals(gameObj.name))
                    {
                        return gameObj;
                    }
                }
            }
            return null;
        }





    }




}