using UnityEngine;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.Network;
using ummisco.gama.unity.geometry;
using ummisco.gama.unity.Scene;
using System;

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
            SetAllProperties(obj);
            SendTopic();
            Debug.Log("Topic processed");
        }


        // The method to call Game Objects methods
        //----------------------------------------
        [System.Obsolete]
        public void SendTopic()
        {
            GameObject agentCreator = GameObject.Find("AgentCreator");
            Agent agent = unityAgent.GetAgent();
            GameObject obj;
            
            if(agent.Geometry.Equals(IGeometry.POLYGON) || agent.Geometry.Equals(IGeometry.Polygon))
            {
                obj = agentCreator.GetComponent<AgentCreator>().CreateGenericPolygonAgent(agent, true, "Building", 0);

                System.Type MyScriptType = SceneManager.LoadSpeciesScript(agent.Species);

                if (MyScriptType != null)
                    obj.AddComponent(MyScriptType);
            }

            if (agent.Geometry.Equals(IGeometry.LINESTRING) || agent.Geometry.Equals(IGeometry.LineString))
            {
                obj = agentCreator.GetComponent<AgentCreator>().CreateGenericLineAgent(agent, 3f, "Road", 0);
            }

            if (agent.Geometry.Equals(IGeometry.POINT) || agent.Geometry.Equals(IGeometry.Point))
            {
                
                obj = agentCreator.GetComponent<AgentCreator>().CreateGenericPointAgent(agent, 10f, "Point", 50);
                             
                System.Type MyScriptType = SceneManager.LoadSpeciesScript(agent.Species);
               
                if(MyScriptType != null)
                    obj.AddComponent(MyScriptType);

            }
           




            //GameObject objectManager = getGameObjectByName(IMQTTConnector.GAMA_MANAGER_OBJECT_NAME, UnityEngine.Object.FindObjectsOfType<GameObject>());
            //Debug.Log("The content is: " + topicMessage.contents.ToString());

            // Agent gamaAgent = UtilXml.getAgent((XmlNode[])topicMessage.contents);
            /*
            Agent gamaAgent = unityAgent.GetAgent();
            GamaManager.gamaAgentList[GamaManager.nbrAgent] = gamaAgent;
            
            GamaManager.nbrAgent++;
            Debug.Log(" -->   New agent received ! " + gamaAgent.agentName);
            */

        }

        public override void SetAllProperties(object args)
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