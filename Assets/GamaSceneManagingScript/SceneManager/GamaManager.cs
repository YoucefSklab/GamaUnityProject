﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.utils;
using ummisco.gama.unity.notification;

using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System;
using System.IO;

using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using ummisco.gama.unity.topics;
using System.Globalization;
using ummisco.gama.unity.GamaAgent;
using Nextzen;
using ummisco.gama.unity.littosim;

namespace ummisco.gama.unity.SceneManager
{
    public class GamaManager : MonoBehaviour
    {


        private static GamaManager m_Instance = null;
        public static GameObject MainCamera;
        public long total = 0;
        public int totalAgents = 0;
        public int nbrMessage = 0;
        public bool readMessage = false;

        public static GamaManager Instance { get { return m_Instance; } }

        public string receivedMsg = "";
        public string clientId;
        public static MqttClient client;
        public GamaMethods gama = new GamaMethods();
        public TopicMessage currentMsg;

        public GameObject[] allObjects = null;
        public GameObject gamaManager = null;
        public GameObject targetGameObject = null;
        public GameObject topicGameObject = null;
        public object[] obj = null;

        public GameObject plane = null;

        public Transform UA_Transform;
        public Transform Def_Cote_Transform;

        public List<GameObject> objectsList = new List<GameObject>();

        public static Agent[] gamaAgentList = new Agent[5000];
        public static int nbrAgent = 0;
        
        public Material planeMaterial;
        public Material polygonMaterial;
        public Material lineMaterial;
        public Material mat;

        public Transform parentObjectTransform;

        public GameObject setTopicManager, getTotpicManager, moveTopicManager, notificationTopicManager;

        private GameObject mainTopicManager;

        private MeshCreator meshCreator = new MeshCreator();

        List<MqttMsgPublishEventArgs> msgList = new List<MqttMsgPublishEventArgs>();

        void Awake()
        {
            m_Instance = this;
            //Check if instance already exists 	
            //If instance already exists and it's not this:
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a gamaManager.
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            //DontDestroyOnLoad(gameObject);

            MqttSetting.allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            gamaManager = gameObject;
            MainCamera = GameObject.Find("MainCamera");

            // Create the Topic's manager GameObjects
            new GameObject(MqttSetting.COLOR_TOPIC_MANAGER).AddComponent<ColorTopic>();
            new GameObject(MqttSetting.POSITION_TOPIC_MANAGER).AddComponent<PositionTopic>();
            new GameObject(MqttSetting.SET_TOPIC_MANAGER).AddComponent<SetTopic>();
            new GameObject(MqttSetting.GET_TOPIC_MANAGER).AddComponent<GetTopic>();
            new GameObject(MqttSetting.MONO_FREE_TOPIC_MANAGER).AddComponent<MonoFreeTopic>();
            new GameObject(MqttSetting.MULTIPLE_FREE_TOPIC_MANAGER).AddComponent<MultipleFreeTopic>();
            new GameObject(MqttSetting.CREATE_TOPIC_MANAGER).AddComponent<CreateTopic>();
            new GameObject(MqttSetting.DESTROY_TOPIC_MANAGER).AddComponent<DestroyTopic>();
            new GameObject(MqttSetting.MOVE_TOPIC_MANAGER).AddComponent<MoveTopic>();
            new GameObject(MqttSetting.NOTIFICATION_TOPIC_MANAGER).AddComponent<NotificationTopic>();
            new GameObject(MqttSetting.PROPERTY_TOPIC_MANAGER).AddComponent<PropertyTopic>();
            (mainTopicManager = new GameObject(MqttSetting.MAIN_TOPIC_MANAGER)).AddComponent<MainTopic>();

            new GameObject(IGamaManager.CSVREADER).AddComponent<CSVReader>().transform.SetParent(gamaManager.transform);


        }


        // Use this for initialization
        void Start()
        {
          

            //MqttSetting.SERVER_URL = "localhost";
            //MqttSetting.SERVER_PORT = 1883;
            var timestamp = DateTime.Now.ToFileTime();
            clientId = Guid.NewGuid().ToString() + timestamp;
            client = new MqttClient(MqttSetting.SERVER_URL, MqttSetting.SERVER_PORT, false, null);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // client.Connect(clientId, MqttSetting.DEFAULT_USER, MqttSetting.DEFAULT_PASSWORD);
            client.Connect(clientId);

            client.Subscribe(new string[] { MqttSetting.MAIN_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.MONO_FREE_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.MULTIPLE_FREE_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.POSITION_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.COLOR_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.GET_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.SET_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.MOVE_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.PROPERTY_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.CREATE_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.DESTROY_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { MqttSetting.NOTIFICATION_TOPIC }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            client.Subscribe(new string[] { "littosim" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            client.Publish("littosim", System.Text.Encoding.UTF8.GetBytes(client.ClientId));

        }


        void FixedUpdate()
        {
            handleMessage();
        }


        public void handleMessage()
        {
           // if(readMessage)
            while(msgList.Count > 0)
            {
                MqttMsgPublishEventArgs e = msgList[0];
                if (!MqttSetting.getTopicsInList().Contains(e.Topic))
                {
                    Debug.Log("-> The Topic '" + e.Topic + "' doesn't exist in the defined list. Please check! (the message will be deleted!)");
                    msgList.Remove(e);
                    return;
                }

                receivedMsg = System.Text.Encoding.UTF8.GetString(e.Message);
               // allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

                Debug.Log("-> Received Message is : " + receivedMsg);

                switch (e.Topic)
                {
                    case MqttSetting.MAIN_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log(totalAgents+ "  -> Topic to deal with is : " + MqttSetting.MAIN_TOPIC);
                        totalAgents++;

                        DateTime currentDate = DateTime.Now;

                        UnityAgent unityAgent = (UnityAgent)MsgSerialization.deserialization(receivedMsg, new UnityAgent());

                        DateTime NextDate = DateTime.Now;

                        long elapsedTicks = NextDate.Ticks - currentDate.Ticks;
                        total += elapsedTicks;
                        TimeSpan elapsedSpan = new TimeSpan(total);

                        // Debug.Log(" Elapsed time is : "+ elapsedTicks);
                        // Debug.Log("  Elapsed time in seconds: " + elapsedSpan.TotalSeconds);
                        // Debug.Log(" minutes : "+elapsedSpan.TotalMinutes);

                        /*
                        Debug.Log("-*----------------- - >  Deserialization: Sender is: " + unityAgent.sender);

                        Debug.Log("-*----------------- - >  Deserialization: Agent Name is:  " + unityAgent.contents.agentName);

                        Debug.Log("-*----------------- - >  Deserialization: Agent geometry is:  " + unityAgent.contents.geometryType);

                        Debug.Log("-*----------------- - >  Deserialization: Agent vertices are:  " + unityAgent.contents.vertices.Count);
                        Debug.Log("-*----------------- - >  Deserialization: Agent first vertex is:  " + unityAgent.contents.vertices[0].ToString());

                        Debug.Log("-*----------------- - >  Deserialization: Agent color is:  " + unityAgent.contents.color.ToString());

                        Debug.Log("-*----------------- - >  Deserialization: Agent height is:  " + unityAgent.contents.height);

                        Debug.Log("-*----------------- - >  Deserialization: Agent emissionTimeStamp is:  " + unityAgent.emissionTimeStamp);
                        */

                        // ----------------------------
                        // ------------------------------------
                        
                        GameObject newGameObject;
                        Agent agent = unityAgent.GetAgent();
                        newGameObject = new GameObject(agent.agentName);

                        newGameObject.AddComponent(typeof(MeshRenderer));
                        newGameObject.AddComponent(typeof(MeshFilter));
                        newGameObject.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
                        newGameObject.AddComponent<MeshCollider>();

                        if (agent.species.Equals("UA"))
                        {
                            newGameObject.GetComponent<Transform>().SetParent(UA_Transform);
                          
                            newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(50, agent.agentCoordinate.getVector2Coordinates());
                            //mat.color = agent.color.getColorFromGamaColor();
                            newGameObject.GetComponent<Renderer>().material = planeMaterial;

                            Material mat1 = new Material(Shader.Find("Standard"));
                            mat1.color =agent.color.getColorFromGamaColor();
                            newGameObject.GetComponent<Renderer>().material = mat1;
                            //meshRenderer.materials = materials;

                            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                            newGameObject.AddComponent<UA>();
                            newGameObject.GetComponent<UA>().ua_name = agent.agentName + "_";
                            newGameObject.GetComponent<UA>().ua_code = 12;
                            newGameObject.GetComponent<UA>().population = 12;
                            newGameObject.GetComponent<UA>().cout_expro = 12;
                            newGameObject.GetComponent<UA>().fullNameOfUAname = agent.agentName + "_FULLNAME_" + 12;
                            newGameObject.GetComponent<UA>().classe_densite = agent.agentName + "_CLASSE_DENSITE_" + 12;
                        }
                        else if (agent.species.Equals("def_cote"))
                        {
                            newGameObject.GetComponent<Transform>().SetParent(Def_Cote_Transform);

                            newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(50, agent.agentCoordinate.getVector2Coordinates());
                            //mat.color = agent.color.getColorFromGamaColor();
                            newGameObject.GetComponent<Renderer>().material = lineMaterial;
                            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                            newGameObject.AddComponent<DefCote>();
                            newGameObject.GetComponent<DefCote>().dike_id = 13;
                            newGameObject.GetComponent<DefCote>().type = "type";
                            newGameObject.GetComponent<DefCote>().height = 30;
                            newGameObject.GetComponent<DefCote>().status = "status";
                            newGameObject.GetComponent<DefCote>().length_def_cote = 13;
                            newGameObject.GetComponent<DefCote>().meshElevation = 30;
                        }
                        else
                        {
                            //newGameObject.GetComponent<Transform>().SetParent(parentObjectTransform);
                            newGameObject.GetComponent<Transform>().SetParent(UA_Transform);
                            newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(10, agent.agentCoordinate.getVector2Coordinates());
                            //mat.color = agent.color.getColorFromGamaColor();
                            newGameObject.GetComponent<Renderer>().material = planeMaterial;
                            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }







                        // - ---------------------------------


                        topicGameObject = gameObject;
                        //GamaMessage gamaMessage = (GamaMessage)MsgSerialization.deserialization(receivedMsg, new GamaMessage());
                        //targetGameObject = GameObject.Find(gamaMessage.receivers);
                        targetGameObject = GameObject.Find(unityAgent.receivers);

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + unityAgent.receivers + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { unityAgent, targetGameObject };
                        mainTopicManager.GetComponent(MqttSetting.MAIN_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.MONO_FREE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.MONO_FREE_TOPIC);
                        MonoFreeTopicMessage monoFreeTopicMessage = (MonoFreeTopicMessage)MsgSerialization.deserialization(receivedMsg, new MonoFreeTopicMessage());
                        targetGameObject = GameObject.Find(monoFreeTopicMessage.objectName);
                        obj = new object[] { monoFreeTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + monoFreeTopicMessage.objectName + "). Please check your code! ");
                            break;
                        }
                        Debug.Log("The message is to " + monoFreeTopicMessage.objectName + " about the methode " + monoFreeTopicMessage.methodName + " and attribute " + monoFreeTopicMessage.attribute);
                        GameObject.Find(MqttSetting.MONO_FREE_TOPIC_MANAGER).GetComponent(MqttSetting.MONO_FREE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.MULTIPLE_FREE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.MULTIPLE_FREE_TOPIC);

                        MultipleFreeTopicMessage multipleFreetopicMessage = (MultipleFreeTopicMessage)MsgSerialization.deserialization(receivedMsg, new MultipleFreeTopicMessage());
                        targetGameObject = GameObject.Find(multipleFreetopicMessage.objectName);
                        obj = new object[] { multipleFreetopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + multipleFreetopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(MqttSetting.MULTIPLE_FREE_TOPIC_MANAGER).GetComponent(MqttSetting.MULTIPLE_FREE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.POSITION_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.POSITION_TOPIC);

                        PositionTopicMessage positionTopicMessage = (PositionTopicMessage)MsgSerialization.deserialization(receivedMsg, new PositionTopicMessage());
                        targetGameObject = GameObject.Find(positionTopicMessage.objectName);
                        obj = new object[] { positionTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + positionTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }
                        else
                        {
                            GameObject.Find(MqttSetting.POSITION_TOPIC_MANAGER).GetComponent(MqttSetting.POSITION_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        }

                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.MOVE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.MOVE_TOPIC);
                        Debug.Log("-> the message is : " + receivedMsg);
                        MoveTopicMessage moveTopicMessage = (MoveTopicMessage)MsgSerialization.deserialization(receivedMsg, new MoveTopicMessage());
                        Debug.Log("-> the position to move to is : " + moveTopicMessage.position);
                        Debug.Log("-> the speed is : " + moveTopicMessage.speed);
                        Debug.Log("-> the object to move is : " + moveTopicMessage.objectName);
                        targetGameObject = GameObject.Find(moveTopicMessage.objectName);
                        obj = new object[] { moveTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + moveTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(MqttSetting.MOVE_TOPIC_MANAGER).GetComponent(MqttSetting.MOVE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.COLOR_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.COLOR_TOPIC);

                        ColorTopicMessage colorTopicMessage = (ColorTopicMessage)MsgSerialization.deserialization(receivedMsg, new ColorTopicMessage());
                        targetGameObject = GameObject.Find(colorTopicMessage.objectName);
                        obj = new object[] { colorTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + colorTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(MqttSetting.COLOR_TOPIC_MANAGER).GetComponent(MqttSetting.COLOR_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.GET_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.GET_TOPIC);
                        string value = null;

                        GetTopicMessage getTopicMessage = (GetTopicMessage)MsgSerialization.deserialization(receivedMsg, new GetTopicMessage());
                        targetGameObject = GameObject.Find(getTopicMessage.objectName);


                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + getTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { getTopicMessage, targetGameObject, value };

                        GameObject.Find(MqttSetting.GET_TOPIC_MANAGER).GetComponent(MqttSetting.GET_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        sendReplay(clientId, "GamaAgent", getTopicMessage.attribute, (string)obj[2]);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.SET_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.SET_TOPIC);

                        SetTopicMessage setTopicMessage = (SetTopicMessage)MsgSerialization.deserialization(receivedMsg, new SetTopicMessage());
                        // Debug.Log("-> Target game object name: " + setTopicMessage.objectName);
                        Debug.Log("-> Message: " + receivedMsg);
                        targetGameObject = GameObject.Find(setTopicMessage.objectName);

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + setTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { setTopicMessage, targetGameObject };

                        GameObject.Find(MqttSetting.SET_TOPIC_MANAGER).GetComponent(MqttSetting.SET_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.PROPERTY_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.PROPERTY_TOPIC);

                        try
                        {

                        }
                        catch (Exception er)
                        {
                            Debug.Log("Error : " + er.Message);
                        }

                        PropertyTopicMessage propertyTopicMessage = (PropertyTopicMessage)MsgSerialization.deserialization(receivedMsg, new PropertyTopicMessage());
                        Debug.Log("-> Target game object name: " + propertyTopicMessage.objectName);
                        targetGameObject = GameObject.Find(propertyTopicMessage.objectName);

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + propertyTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { propertyTopicMessage, targetGameObject };

                        GameObject.Find(MqttSetting.PROPERTY_TOPIC_MANAGER).GetComponent(MqttSetting.PROPERTY_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;

                    case MqttSetting.CREATE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.CREATE_TOPIC);
                        // Debug.Log("-> Message: " + receivedMsg);
                        CreateTopicMessage createTopicMessage = (CreateTopicMessage)MsgSerialization.deserialization(receivedMsg, new CreateTopicMessage());
                        obj = new object[] { createTopicMessage };

                        GameObject.Find(MqttSetting.CREATE_TOPIC_MANAGER).GetComponent(MqttSetting.CREATE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.DESTROY_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.DESTROY_TOPIC);

                        DestroyTopicMessage destroyTopicMessage = (DestroyTopicMessage)MsgSerialization.deserialization(receivedMsg, new DestroyTopicMessage());
                        obj = new object[] { destroyTopicMessage };

                        if (topicGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + destroyTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(MqttSetting.DESTROY_TOPIC_MANAGER).GetComponent(MqttSetting.DESTROY_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case MqttSetting.NOTIFICATION_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.NOTIFICATION_TOPIC);

                        NotificationTopicMessage notificationTopicMessage = (NotificationTopicMessage)MsgSerialization.deserialization(receivedMsg, new NotificationTopicMessage());
                        obj = new object[] { notificationTopicMessage };


                        if (topicGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + notificationTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(MqttSetting.NOTIFICATION_TOPIC_MANAGER).GetComponent(MqttSetting.NOTIFICATION_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        //------------------------------------------------------------------------------
                        break;
                    default:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + MqttSetting.DEFAULT_TOPIC);
                        //------------------------------------------------------------------------------
                        break;
                }

                msgList.Remove(e);
            }

            checkForNotifications();
            GameObject mapBuilder = GameObject.Find("MapBuilder");
            //GameObject mapBuilder = GameObject.Find("MapBuilder");
            //regionMap = (RegionMap) FindObjectOfType(typeof(RegionMap));
            //GameObject mapBuilder  = (GameObject) FindObjectOfType(typeof(MapBuilder));

            if (mapBuilder != null)
            {
                mapBuilder.GetComponent<RegionMap>().SendMessage("DrawNewAgents");
            }
            else
            {
                Debug.Log("No such Object. Sorry");
            }


        }



        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            msgList.Add(e);
            receivedMsg = System.Text.Encoding.UTF8.GetString(e.Message);
            //    Debug.Log(">  New Message received on topic : " + e.Topic);
            //    Debug.Log(">  content is :" + e.Message);
        }



        void OnGUI()
        {
            if (GUI.Button(new Rect(20, 1, 100, 20), "Quitter!"))
            {
                client.Disconnect();
                Application.Quit();
            }
        }


        public void tester()
        {
            client.Publish("Gama", System.Text.Encoding.UTF8.GetBytes("Good, Bug fixed -> Sending from Unity3D!!! Good"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }


        public void sendGotBoxMsg()
        {

            GamaReponseMessage msg = new GamaReponseMessage(clientId, "GamaAgent", "Got a Box notification", DateTime.Now.ToString());

            string message = MsgSerialization.msgSerialization(msg);
            client.Publish("Gama", System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            //client.Publish ("Gama", System.Text.Encoding.UTF8.GetBytes ("Good, Another box2"));
        }

        // à revoir en utilisant publishMessage
        public void sendReplay(string sender, string receiver, string fieldName, string fieldValue)
        {

            ReplayMessage msg = new ReplayMessage(sender, receiver, "content not set", fieldName, fieldValue, DateTime.Now.ToString());
            string message = MsgSerialization.serialization(msg);

            publishMessage(message, MqttSetting.REPLAY_TOPIC);
        }

        void OnDestroy()
        {
            m_Instance = null;
        }

        // Update is called once per frame
        void Update()
        {

        }

        // The method to call Game Objects methods
        //----------------------------------------
        public void sendMessageToGameObject(GameObject gameObject, string methodName, Dictionary<object, object> data)
        {

            int size = data.Count;
            List<object> keyList = new List<object>(data.Keys);

            System.Reflection.MethodInfo info = gameObject.GetComponent("PlayerController").GetType().GetMethod(methodName);
            ParameterInfo[] par = info.GetParameters();


            for (int j = 0; j < par.Length; j++)
            {
                System.Reflection.ParameterInfo par1 = par[j];

                //   Debug.Log("->>>>>>>>>>>>>>--> parametre Name >>=>>=>>=  " + par1.Name);
                //   Debug.Log("->>>>>>>>>>>>>>--> parametre Type >>=>>=>>=  " + par1.ParameterType);

            }

            switch (size)
            {
                case 0:
                    gameObject.SendMessage(methodName);
                    break;
                case 1:
                    gameObject.SendMessage(methodName, convertParameter(data[keyList.ElementAt(0)], par[0]));
                    break;

                default:
                    object[] obj = new object[size + 1];
                    int i = 0;
                    foreach (KeyValuePair<object, object> pair in data)
                    {
                        obj[i] = pair.Value;
                        i++;
                    }
                    gameObject.SendMessage(methodName, obj);
                    break;
            }

        }

        public object convertParameter(object val, ParameterInfo par)
        {
            object propValue = Convert.ChangeType(val, par.ParameterType);
            return propValue;
        }




        public void addObjectToList(GameObject obj)
        {
            objectsList.Add(obj);
        }

        public void removeObjectFromList(GameObject obj)
        {
            objectsList.Remove(obj);
        }


        public void checkForNotifications()
        {

            if (NotificationRegistry.notificationsList.Count >= 1)
            {
                foreach (NotificationEntry el in NotificationRegistry.notificationsList)
                {
                    if (!el.isSent)
                    { // TODO Implement a mecanism of notification frequency! 
                        if (el.notify)
                        {
                            string msg = getReplayNotificationMessage(el);
                            publishMessage(msg, MqttSetting.NOTIFY_MSG);
                            el.notify = false;
                            el.isSent = true;
                            Debug.Log("------------------>>>>  Notification " + el.notificationId + " is sent");
                        }
                    }
                }
            }
        }

        public string getReplayNotificationMessage(NotificationEntry el)
        {
            NotificationMessage msg = new NotificationMessage("Unity", el.agentId, "Contents Not set", DateTime.Now.ToString(), el.notificationId);
            string message = MsgSerialization.serializationPlainXml(msg);
            return message;
        }

        public void publishMessage(string message, string topic)
        {
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        
    }
}