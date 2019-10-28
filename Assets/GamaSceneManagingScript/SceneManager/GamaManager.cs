using System.Collections.Generic;
using UnityEngine;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.notification;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using System.Linq;
using ummisco.gama.unity.topics;

using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.littosim;
using ummisco.gama.unity.files;
using ummisco.gama.unity.Network;

namespace ummisco.gama.unity.Scene
{
    public class GamaManager : MonoBehaviour
    {
        private static GamaManager m_Instance;
        public static GameObject MainCamera;
        public static GamaManager Instance { get { return m_Instance; } }

        public string receivedMsg = "";
                   
        public GameObject gamaManager;
        public GameObject targetGameObject;
        public GameObject topicGameObject;
        public object[] obj;

        public Transform Land_Use_Transform;
        public Transform Coastal_Defense_Transform;

        public Material matGreen;
        public Material matGreenLighter;
        public Material matBlue;
        public Material matBlueLighter;
        public Material matRed;
        public Material matRedLighter;
        public Material matYellow;
        public Material matYellowLighter;
        public Material matWhite;

        public Transform parentObjectTransform;

        private GameObject mainTopicManager;
        private GameObject agentCreator;

        public static Dictionary<string, string> agentsTopicDic = new Dictionary<string, string>();

        public static MQTTConnector connector;
        public static SceneManager sceneManager;
                
        void Awake()
        {
            m_Instance = this;
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);

            gamaManager = gameObject;
            MainCamera = GameObject.Find("MainCamera");

            // Create the Topic's manager GameObjects
            new GameObject(IMQTTConnector.COLOR_TOPIC_MANAGER).AddComponent<ColorTopic>();
            new GameObject(IMQTTConnector.POSITION_TOPIC_MANAGER).AddComponent<PositionTopic>();
            new GameObject(IMQTTConnector.SET_TOPIC_MANAGER).AddComponent<SetTopic>();
            new GameObject(IMQTTConnector.GET_TOPIC_MANAGER).AddComponent<GetTopic>();
            new GameObject(IMQTTConnector.MONO_FREE_TOPIC_MANAGER).AddComponent<MonoFreeTopic>();
            new GameObject(IMQTTConnector.MULTIPLE_FREE_TOPIC_MANAGER).AddComponent<MultipleFreeTopic>();
            new GameObject(IMQTTConnector.CREATE_TOPIC_MANAGER).AddComponent<CreateTopic>();
            new GameObject(IMQTTConnector.DESTROY_TOPIC_MANAGER).AddComponent<DestroyTopic>();
            new GameObject(IMQTTConnector.MOVE_TOPIC_MANAGER).AddComponent<MoveTopic>();
            new GameObject(IMQTTConnector.NOTIFICATION_TOPIC_MANAGER).AddComponent<NotificationTopic>();
            new GameObject(IMQTTConnector.PROPERTY_TOPIC_MANAGER).AddComponent<PropertyTopic>();

            new GameObject(IMQTTConnector.MQTT_CONNECTOR).AddComponent<MQTTConnector>();
            new GameObject(IMQTTConnector.SCENE_MANAGER).AddComponent<SceneManager>();
            
            (mainTopicManager = new GameObject(IMQTTConnector.MAIN_TOPIC_MANAGER)).AddComponent<MainTopic>();

            new GameObject(IGamaManager.CSVREADER).AddComponent<CSVReader>().transform.SetParent(gamaManager.transform);
            new GameObject(IGamaManager.AGENT_CREATOR).AddComponent<AgentCreator>();

        }

        // Use this for initialization
        void Start()
        {
            sceneManager = GameObject.Find(IMQTTConnector.SCENE_MANAGER).GetComponent<SceneManager>();
            connector = GameObject.Find(IMQTTConnector.MQTT_CONNECTOR).GetComponent<MQTTConnector>();
            connector.Connect();
            connector.InitTopics();
            connector.Subscribe("littosim");

            agentCreator = GameObject.Find("AgentCreator");

            //agentCreator.GetComponent<AgentCreator>().CreateLine();
        }


        void FixedUpdate()
        {
            HandleMessage();
        }


        public void HandleMessage()
        {                   

           while (connector.HasNextMessage())
            {
                MqttMsgPublishEventArgs e = connector.GetNextMessage();
                /*
                if (!IMQTTConnector.getTopicsInList().Contains(e.Topic))
                {
                    Debug.Log("-> The Topic '" + e.Topic + "' doesn't exist in the defined list. Please check! (the message will be deleted!)");
                    msgList.Remove(e);
                    return;
                }
                */

                receivedMsg = System.Text.Encoding.UTF8.GetString(e.Message);

                //     Debug.Log("-> Received Message is : " + receivedMsg);

                if (agentsTopicDic.Keys.Contains(e.Topic)){

                    // DO Not DELETE
                    /*
                    string serialisedObject = new XStream().ToXml(receivedMsg);
                    GamaExposeMessage deserialisedObject = (GamaExposeMessage) new XStream().FromXml(serialisedObject);
                    */
                    //Debug.Log("The topic is : " + e.Topic);
                    GamaExposeMessage exposeMessage = new GamaExposeMessage(receivedMsg);

                    sceneManager.SetAttribute(agentsTopicDic[e.Topic], exposeMessage.attributesList);
                }
                else
                switch (e.Topic)
                {
                    //case "listdata":
                    //    break;
                    case IMQTTConnector.MAIN_TOPIC:
                        //------------------------------------------------------------------------------
                          Debug.Log("  -> Topic to deal with is : " + IMQTTConnector.MAIN_TOPIC);
                       
                        UnityAgent unityAgent = (UnityAgent)MsgSerialization.FromXML(receivedMsg, new UnityAgent());
                        Agent agent = unityAgent.GetAgent();

                        switch (agent.species)
                        {
                            case IUILittoSim.LAND_USE:
                                agent.height = 10;
                                //agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matGreen, IUILittoSim.LAND_USE_ID, true, ILittoSimConcept.LAND_USE_TAG, -60);
                                agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matRed, IUILittoSim.LAND_USE_ID, true, ILittoSimConcept.LAND_USE_TAG, -60);
                                break;
                            case IUILittoSim.COASTAL_DEFENSE:
                                agent.height = 10;
                                //agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Coastal_Defense_Transform, lineMaterial, IUILittoSim.COASTAL_DEFENSE_ID, true, ILittoSimConcept.COASTAL_DEFENSE_TAG);
                                agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matYellow, IUILittoSim.COASTAL_DEFENSE_ID, true, ILittoSimConcept.COASTAL_DEFENSE_TAG, -80);
                                break;
                            case IUILittoSim.DISTRICT:
                                agent.height = 10;
                                agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matBlue, IUILittoSim.DISTRICT_ID, true, ILittoSimConcept.DISTRICT_TAG, -40);
                                break;
                            case IUILittoSim.FLOOD_RISK_AREA:
                                agent.height = 10;
                                agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matRed, IUILittoSim.FLOOD_RISK_AREA_ID, true, ILittoSimConcept.FLOOD_RISK_AREA_TAG, -100);
                                break;
                            case IUILittoSim.PROTECTED_AREA:
                                agent.height = 10;
                                agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, matGreenLighter, IUILittoSim.PROTECTED_AREA_ID, true, ILittoSimConcept.PROTECTED_AREA_TAG, -100);
                                break;
                            case IUILittoSim.ROAD:
                                agent.height = 0;
                                //agentCreator.GetComponent<AgentCreator>().CreateAgent(agent, Land_Use_Transform, mat, IUILittoSim.ROAD_ID, false);
                                agentCreator.GetComponent<AgentCreator>().CreateLineAgent(agent, Land_Use_Transform, matWhite, IUILittoSim.ROAD_ID, false, 10f, ILittoSimConcept.ROAD_TAG, -151);
                                break;
                            default:
                                targetGameObject = GameObject.Find(unityAgent.receivers);
                                if (targetGameObject == null)
                                {
                                    Debug.LogError(" Sorry, requested gameObject is null (" + unityAgent.receivers + "). Please check you code! ");
                                    break;
                                }
                                else
                                {
                                    Debug.Log("Generic Object creation : "+ unityAgent.contents.agentName);
                                    obj = new object[] { unityAgent, targetGameObject };
                                    mainTopicManager.GetComponent<MainTopic>().ProcessTopic(obj);
                                    //mainTopicManager.GetComponent(IMQTTConnector.MAIN_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                                }
                                break;
                        }

                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.MONO_FREE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.MONO_FREE_TOPIC);
                        MonoFreeTopicMessage monoFreeTopicMessage = (MonoFreeTopicMessage)MsgSerialization.FromXML(receivedMsg, new MonoFreeTopicMessage());
                        targetGameObject = GameObject.Find(monoFreeTopicMessage.objectName);
                        obj = new object[] { monoFreeTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + monoFreeTopicMessage.objectName + "). Please check your code! ");
                            break;
                        }
                        //    Debug.Log("The message is to " + monoFreeTopicMessage.objectName + " about the methode " + monoFreeTopicMessage.methodName + " and attribute " + monoFreeTopicMessage.attribute);
                        GameObject.Find(IMQTTConnector.MONO_FREE_TOPIC_MANAGER).GetComponent(IMQTTConnector.MONO_FREE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.MULTIPLE_FREE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.MULTIPLE_FREE_TOPIC);

                        MultipleFreeTopicMessage multipleFreetopicMessage = (MultipleFreeTopicMessage)MsgSerialization.FromXML(receivedMsg, new MultipleFreeTopicMessage());
                        targetGameObject = GameObject.Find(multipleFreetopicMessage.objectName);
                        obj = new object[] { multipleFreetopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + multipleFreetopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(IMQTTConnector.MULTIPLE_FREE_TOPIC_MANAGER).GetComponent(IMQTTConnector.MULTIPLE_FREE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.POSITION_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.POSITION_TOPIC);

                        PositionTopicMessage positionTopicMessage = (PositionTopicMessage)MsgSerialization.FromXML(receivedMsg, new PositionTopicMessage());
                        targetGameObject = GameObject.Find(positionTopicMessage.objectName);
                        obj = new object[] { positionTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + positionTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }
                        else
                        {
                            GameObject.Find(IMQTTConnector.POSITION_TOPIC_MANAGER).GetComponent(IMQTTConnector.POSITION_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        }

                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.MOVE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.MOVE_TOPIC);
                        Debug.Log("-> the message is : " + receivedMsg);
                        MoveTopicMessage moveTopicMessage = (MoveTopicMessage)MsgSerialization.FromXML(receivedMsg, new MoveTopicMessage());
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

                        GameObject.Find(IMQTTConnector.MOVE_TOPIC_MANAGER).GetComponent(IMQTTConnector.MOVE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.COLOR_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.COLOR_TOPIC);

                        ColorTopicMessage colorTopicMessage = (ColorTopicMessage)MsgSerialization.FromXML(receivedMsg, new ColorTopicMessage());
                        targetGameObject = GameObject.Find(colorTopicMessage.objectName);
                        obj = new object[] { colorTopicMessage, targetGameObject };

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + colorTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(IMQTTConnector.COLOR_TOPIC_MANAGER).GetComponent(IMQTTConnector.COLOR_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.GET_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.GET_TOPIC);
                        string value = null;

                        GetTopicMessage getTopicMessage = (GetTopicMessage)MsgSerialization.FromXML(receivedMsg, new GetTopicMessage());
                        targetGameObject = GameObject.Find(getTopicMessage.objectName);


                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + getTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { getTopicMessage, targetGameObject, value };

                        GameObject.Find(IMQTTConnector.GET_TOPIC_MANAGER).GetComponent(IMQTTConnector.GET_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        SendReplay(connector.clientId, "GamaAgent", getTopicMessage.attribute, (string)obj[2]);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.SET_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.SET_TOPIC);

                        SetTopicMessage setTopicMessage = (SetTopicMessage)MsgSerialization.FromXML(receivedMsg, new SetTopicMessage());
                        // Debug.Log("-> Target game object name: " + setTopicMessage.objectName);
                        Debug.Log("-> Message: " + receivedMsg);
                        targetGameObject = GameObject.Find(setTopicMessage.objectName);

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + setTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { setTopicMessage, targetGameObject };

                        GameObject.Find(IMQTTConnector.SET_TOPIC_MANAGER).GetComponent(IMQTTConnector.SET_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.PROPERTY_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.PROPERTY_TOPIC);

                        try
                        {

                        }
                        catch (Exception er)
                        {
                            Debug.Log("Error : " + er.Message);
                        }

                        PropertyTopicMessage propertyTopicMessage = (PropertyTopicMessage)MsgSerialization.FromXML(receivedMsg, new PropertyTopicMessage());
                        Debug.Log("-> Target game object name: " + propertyTopicMessage.objectName);
                        targetGameObject = GameObject.Find(propertyTopicMessage.objectName);

                        if (targetGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + propertyTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        obj = new object[] { propertyTopicMessage, targetGameObject };

                        GameObject.Find(IMQTTConnector.PROPERTY_TOPIC_MANAGER).GetComponent(IMQTTConnector.PROPERTY_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;

                    case IMQTTConnector.CREATE_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.CREATE_TOPIC);
                        // Debug.Log("-> Message: " + receivedMsg);
                        CreateTopicMessage createTopicMessage = (CreateTopicMessage)MsgSerialization.FromXML(receivedMsg, new CreateTopicMessage());
                        obj = new object[] { createTopicMessage };

                        GameObject.Find(IMQTTConnector.CREATE_TOPIC_MANAGER).GetComponent(IMQTTConnector.CREATE_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.DESTROY_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.DESTROY_TOPIC);

                        DestroyTopicMessage destroyTopicMessage = (DestroyTopicMessage)MsgSerialization.FromXML(receivedMsg, new DestroyTopicMessage());
                        obj = new object[] { destroyTopicMessage };

                        if (topicGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + destroyTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(IMQTTConnector.DESTROY_TOPIC_MANAGER).GetComponent(IMQTTConnector.DESTROY_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);
                        //------------------------------------------------------------------------------
                        break;
                    case IMQTTConnector.NOTIFICATION_TOPIC:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.NOTIFICATION_TOPIC);

                        NotificationTopicMessage notificationTopicMessage = (NotificationTopicMessage)MsgSerialization.FromXML(receivedMsg, new NotificationTopicMessage());
                        obj = new object[] { notificationTopicMessage };


                        if (topicGameObject == null)
                        {
                            Debug.LogError(" Sorry, requested gameObject is null (" + notificationTopicMessage.objectName + "). Please check you code! ");
                            break;
                        }

                        GameObject.Find(IMQTTConnector.NOTIFICATION_TOPIC_MANAGER).GetComponent(IMQTTConnector.NOTIFICATION_TOPIC_SCRIPT).SendMessage("ProcessTopic", obj);

                        //------------------------------------------------------------------------------
                        break;
                    default:
                        //------------------------------------------------------------------------------
                        Debug.Log("-> Topic to deal with is : " + IMQTTConnector.DEFAULT_TOPIC);
                        //------------------------------------------------------------------------------
                        break;
                }
               
            }
            CheckForNotifications();
        }



          

        void OnGUI()
        {
            if (GUI.Button(new Rect(20, 1, 100, 20), "Quitter!"))
            {
                connector.Disconnect();
                #if UNITY_EDITOR
                                UnityEditor.EditorApplication.isPlaying = false;
                #else
                        Application.Quit ();
                #endif
            }
        }

        public void Tester()
        {
            connector.Publish("Gama", "Good, Bug fixed -> Sending from Unity3D!!! Good");
        }

        public void SendGotBoxMsg()
        {
            GamaReponseMessage msg = new GamaReponseMessage(connector.clientId, "GamaAgent", "Got a Box notification", DateTime.Now.ToString());
            string message = MsgSerialization.ToXML(msg);
            connector.Publish("Gama", message);
        }

        // à revoir en utilisant publishMessage
        public void SendReplay(string sender, string receiver, string fieldName, string fieldValue)
        {
            ReplayMessage msg = new ReplayMessage(sender, receiver, "content not set", fieldName, fieldValue, DateTime.Now.ToString());
            string message = MsgSerialization.ToXML(msg);
            connector.Publish(IMQTTConnector.REPLAY_TOPIC, message);
        }

        void OnDestroy()
        {
            m_Instance = null;
        }

        // Update is called once per frame
        void Update()
        {

        }
                       

        public void CheckForNotifications()
        {
            if (NotificationRegistry.notificationsList.Count >= 1)
            {
                foreach (NotificationEntry el in NotificationRegistry.notificationsList)
                {
                    if (!el.isSent)
                    { // TODO Implement a mecanism of notification frequency!
                        if (el.notify)
                        {
                            string msg = GetReplayNotificationMessage(el);
                            connector.Publish(IMQTTConnector.NOTIFY_MSG, msg);
                            el.notify = false;
                            el.isSent = true;
                            Debug.Log("------------------>>>>  Notification " + el.notificationId + " is sent");
                        }
                    }
                }
            }
        }


        public string GetReplayNotificationMessage(NotificationEntry el)
        {
            NotificationMessage msg = new NotificationMessage("Unity", el.agentId, "Contents Not set", DateTime.Now.ToString(), el.notificationId);
            string message = MsgSerialization.SerializationPlainXml(msg);
            return message;
        }

        public void SetWorldEnvelope(object args)
        {
            GameObject.Find(IGamaManager.SCENE_MANAGER).GetComponent<SceneManager>().SetWorldEnvelope(args);
        }



        public void SubscribeToTopic(object args)
        {
            object[] obj = (object[])args;
            string agentName = (string)obj[0];
            string topic = (string)obj[1];
            connector.Subscribe(topic);
            agentsTopicDic.Add(topic, agentName);
        }

        public void InitGenericScene(object args)
        {      

            GameObject.Find(IGamaManager.SCENE_MANAGER).GetComponent<SceneManager>().SetGenericScene(true);
            GameObject.Find(IGamaManager.SCENE_MANAGER).GetComponent<SceneManager>().CreateEnveloppe();
            GameObject.Find(IGamaManager.SCENE_MANAGER).GetComponent<SceneManager>().SetWorldEnvelope(args);
        
        }
              
    }
}