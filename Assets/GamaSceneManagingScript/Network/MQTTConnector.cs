using UnityEngine;
using System.Collections;
using uPLibrary.Networking.M2Mqtt;
using System;
using ummisco.gama.unity.utils;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ummisco.gama.unity.Network
{ 
    public class MQTTConnector : MonoBehaviour
    {
        public string clientId; 
        public MqttClient client;

        // Server parameters
        public static string SERVER_URL = "localhost";
        public static int SERVER_PORT = 1883;

        //public static string SERVER_URL = "195.221.248.15";
        //public static int SERVER_PORT = 1935;
        public static string DEFAULT_USER = "gama_demo";
        public static string DEFAULT_PASSWORD = "gama_demo";

        //public static string SERVER_URL = "iot.eclipse.org";
        //public static int SERVER_PORT = 1935;

        List<MqttMsgPublishEventArgs> msgList = new List<MqttMsgPublishEventArgs>();

        public void Connect()
        {
            var timestamp = DateTime.Now.ToFileTime();
            clientId = Guid.NewGuid().ToString() + timestamp;
            client = new MqttClient(SERVER_URL, SERVER_PORT, false, null);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // client.Connect(clientId, MqttSetting.DEFAULT_USER, MqttSetting.DEFAULT_PASSWORD);
            client.Connect(clientId);
        }

        public void Subscribe(string topic)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        public void Publish(string topic, string message)
        {
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }
                     
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            msgList.Add(e);
        }

        public MqttMsgPublishEventArgs getNextMessage()
        {
            if (hasNextMessage())
            {
                MqttMsgPublishEventArgs msg = msgList[0];
                msgList.Remove(msg);
                return msg;
            }
            else
            {
                return null;
            }
           
        }

        public bool hasNextMessage()
        {
            if (msgList.Count > 0) return true;
            return false;
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public static List<string> getTopicsInList()
        {
            List<string> topicsList = new List<string>();
            topicsList.Add(IMQTTConnector.MAIN_TOPIC);
            topicsList.Add(IMQTTConnector.MONO_FREE_TOPIC);
            topicsList.Add(IMQTTConnector.MULTIPLE_FREE_TOPIC);
            topicsList.Add(IMQTTConnector.POSITION_TOPIC);
            topicsList.Add(IMQTTConnector.COLOR_TOPIC);
            topicsList.Add(IMQTTConnector.REPLAY_TOPIC);
            topicsList.Add(IMQTTConnector.DEFAULT_TOPIC);
            topicsList.Add(IMQTTConnector.SET_TOPIC);
            topicsList.Add(IMQTTConnector.GET_TOPIC);
            topicsList.Add(IMQTTConnector.MOVE_TOPIC);
            topicsList.Add(IMQTTConnector.PROPERTY_TOPIC);
            topicsList.Add(IMQTTConnector.NOTIFICATION_TOPIC);
            topicsList.Add(IMQTTConnector.CREATE_TOPIC);
            topicsList.Add(IMQTTConnector.DESTROY_TOPIC);
            topicsList.Add("listdata");
            return topicsList;
        }


        public void initTopics()
        {
            List<string> topicsList = getTopicsInList();

            foreach(string topic in topicsList)
            {
                Subscribe(topic);
            }
        }

    }
}
