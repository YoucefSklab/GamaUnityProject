using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ummisco.gama.unity.datastructure;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.Scene
{
    public class SceneManager : MonoBehaviour
    {
        public SceneManager()
        {

        }


        public static void SetSpeciesEnabled(string species, bool enabled)
        {
            if (ApplicationContexte.gamaAgentList.ContainsKey(species))
            {
                List<GameObject> list = ApplicationContexte.gamaAgentList[species];

                foreach (GameObject obj in list)
                {
                    obj.SetActive(enabled);
                }
            }
        }

        public static void AddTag(string tag)
        {
            // causes errors with build
            /*
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
          //  UnityEngine.Object[] asset =  Resources.LoadAll("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                    {
                        return;     // Tag already present, nothing to do.
                    }
                }
                tags.InsertArrayElementAtIndex(0);
                tags.GetArrayElementAtIndex(0).stringValue = tag;
                so.ApplyModifiedProperties();
                so.Update();
            }
            */
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



        // The method to call Game Objects methods
        //----------------------------------------
        public void SetAttribute(string agentName, Dictionary<string, string> data)
        {
            Debug.Log("Set the attributes ---- ");
            GameObject targetGameObject = GameObject.Find(agentName);
            MonoBehaviour[] scripts = targetGameObject.GetComponents<MonoBehaviour>();
            int size = data.Count;

            List<string> keyList = new List<string>(data.Keys);
            string obj = data[keyList.ElementAt(0)];

            FieldInfo[] fieldInfoSet = targetGameObject.GetComponent(scripts[0].GetType()).GetType().GetFields();

            foreach (KeyValuePair<string, string> pair in data)
            {
                foreach (FieldInfo fi in fieldInfoSet)
                {
                    if (fi.Name.Equals(pair.Key.ToString()))
                    {
                        UnityEngine.Component ob = (UnityEngine.Component)targetGameObject.GetComponent(scripts[0].GetType());

                        if (fi.FieldType.Equals(typeof(UnityEngine.UI.Text)))
                        {

                            Component[] cs = (Component[])targetGameObject.GetComponents(typeof(Component));
                            foreach (Component c in cs)
                            {
                                if (c.name.Equals(pair.Key.ToString()))
                                {
                                    Text txt = targetGameObject.GetComponent<Text>();

                                    txt.text = "Score : ";
                                }
                            }

                            //	Debug.Log ("try to get the fields lis  " + fi.GetType ().GetField ("text").ToString ());
                            FieldInfo[] fieldInfoSet2 = fi.FieldType.GetFields();
                            Debug.Log("Its fieldInfoSet2 is 1 ----> : " + fieldInfoSet2.ToList().ToString());
                            foreach (FieldInfo fi2 in fieldInfoSet2)
                            {
                                Debug.Log("Its Name is 1 ----> : " + fi2.Name);
                                //Debug.Log ("Its Value is 1 ----> : " + fi2.GetValue ());
                            }

                            //fi.FieldType.GetFields ();

                            //fi.SetValue (ob, setObject);

                        }
                        else
                        {
                            //TODO: need to complete this list
                            Debug.Log("Its Name is ----> : " + fi.Name + " and type is :" + fi.FieldType.ToString());
                            switch (fi.FieldType.ToString())
                            {

                                case IDataType.UNITY_INT:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    int valInt = Convert.ToInt32(pair.Value);
                                    fi.SetValue(ob, valInt);
                                    break;
                                case IDataType.UNITY_DOUBLE:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    double valDouble = Convert.ToDouble(pair.Value);
                                    fi.SetValue(ob, valDouble);
                                    break;
                                case IDataType.UNITY_SINGLE:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    fi.SetValue(ob, Convert.ToSingle(pair.Value));
                                    break;
                                case IDataType.UNITY_BOOLEAN:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    fi.SetValue(ob, Convert.ChangeType(pair.Value, fi.FieldType));
                                    break;
                                case IDataType.UNITY_STRING:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    fi.SetValue(ob, (System.String)pair.Value);
                                    break;
                                case IDataType.UNITY_CHAR:
                                    Debug.Log("Its type is ----> :" + fi.FieldType);
                                    char valChar = Convert.ToChar(pair.Value);
                                    fi.SetValue(ob, valChar);
                                    break;

                                default:

                                    break;

                            }
                         }
                    }
                }
            }
        }
    }
}
