using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.utils;
using ummisco.gama.unity.Scene;

using System.Reflection;
using System.Linq;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using ummisco.gama.unity.GamaConcepts;

namespace ummisco.gama.unity.topics
{
    public class PositionTopic : Topic
    {
        public PositionTopicMessage topicMessage;

        public PositionTopic(GameObject gameObj) : base(gameObj)
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

            if (targetGameObject != null)
            {
                Vector3 position = topicMessage.position.ToVector3D();

                SendTopic(position);
            }
        }

        // The method to call Game Objects methods
        //----------------------------------------
        public void SendTopic(Vector3 position)
        {
            targetGameObject.transform.position = IGamaManager.TransformGamaLocation(position);
        }

        public override void SetAllProperties(object args)
        {
            object[] obj = (object[])args;
            this.topicMessage = (PositionTopicMessage)obj[0];
            this.targetGameObject = (GameObject)obj[1];
        }
    }
}