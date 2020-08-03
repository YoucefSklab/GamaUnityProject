using UnityEngine;
using System.Collections;
using uPLibrary.Networking.M2Mqtt;
using System;
using ummisco.gama.unity.utils;
using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ummisco.gama.unity.Network {
    public class MQTTConnector: MonoBehaviour {
        public string clientId;
        public MqttClient client;

        // Server parameters
        public static string SERVER_URL = "localhost";
        public static int SERVER_PORT = 1883;

        //public static string SERVER_URL = "vmpams.ird.fr";
        //public static int SERVER_PORT = 1935;

        //public static string SERVER_URL = "195.221.248.15";
        //public static int SERVER_PORT = 1935;
        public static string DEFAULT_USER = "gama_demo";
        public static string DEFAULT_PASSWORD = "gama_demo";

        //public static string SERVER_URL = "iot.eclipse.org";
        //public static int SERVER_PORT = 1935;

        List<MqttMsgPublishEventArgs> msgList = new List<MqttMsgPublishEventArgs>();

        public void Connect(string serverUrl, int serverPort, string userId, string password) {

            clientId = Guid.NewGuid().ToString() + DateTime.Now.ToFileTime();
            client = new MqttClient(serverUrl, serverPort, false, null);

            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            client.Connect(clientId, userId, password);
            //client.Connect(clientId);

            Debug.Log("Connected to : " + SERVER_URL + "  " + SERVER_PORT + "  " + DEFAULT_USER + "   " + DEFAULT_PASSWORD);
            //client.Connect(clientId);

            Debug.Log(" Is sent: " + client.Publish("test", System.Text.Encoding.UTF8.GetBytes("Test 1 sur Test"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true));
        }

        public void Subscribe(string topic) {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        public void Publish(string topic, string message) {
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {
            msgList.Add(e);
        }

        public MqttMsgPublishEventArgs GetNextMessage() {
            if(HasNextMessage()) {
                MqttMsgPublishEventArgs msg = msgList[0];
                msgList.Remove(msg);
                return msg;
            }
            else {
                return null;
            }

        }

        public bool HasNextMessage() {
            if(msgList.Count > 0) return true;
            return false;
        }

        public void Disconnect() {
            client.Disconnect();
        }

        public static List<string> GetTopicsInList() {
            List<string> topicsList = new List<string>
            {
                IMQTTConnector.MAIN_TOPIC,
                IMQTTConnector.MONO_FREE_TOPIC,
                IMQTTConnector.MULTIPLE_FREE_TOPIC,
                IMQTTConnector.POSITION_TOPIC,
                IMQTTConnector.COLOR_TOPIC,
                IMQTTConnector.REPLAY_TOPIC,
                IMQTTConnector.DEFAULT_TOPIC,
                IMQTTConnector.SET_TOPIC,
                IMQTTConnector.GET_TOPIC,
                IMQTTConnector.MOVE_TOPIC,
                IMQTTConnector.PROPERTY_TOPIC,
                IMQTTConnector.NOTIFICATION_TOPIC,
                IMQTTConnector.CREATE_TOPIC,
                IMQTTConnector.DESTROY_TOPIC,
                "listdata"
            };
            return topicsList;
        }


        public void InitTopics() {
            List<string> topicsList = GetTopicsInList();

            foreach(string topic in topicsList) {
                Subscribe(topic);
            }
        }


        public void SendAttributeUpdate(string attributeName, object attributeValue, string MqttTopic) {

        }

    }
}
