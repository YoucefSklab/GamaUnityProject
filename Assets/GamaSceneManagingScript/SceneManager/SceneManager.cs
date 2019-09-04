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
        public static bool isGenericScene;
        public static RectTransform worldEnveloppeRT;
        public static Canvas worldEnveloppeCanvas;

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
            // Debug.Log(tag);
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

        public void CreateEnveloppe()
        {
            if (isGenericScene)
            {
                GameObject enveloppeObject = new GameObject(IGamaManager.WORLD_ENVELOPPE);
                GameObject canvasEnveloppeObject = new GameObject(IGamaManager.CANVAS_ENVELOPPE);
                canvasEnveloppeObject.AddComponent<Canvas>();
                canvasEnveloppeObject.AddComponent<RectTransform>();
                canvasEnveloppeObject.AddComponent<CanvasScaler>();
                canvasEnveloppeObject.AddComponent<GraphicRaycaster>();

                enveloppeObject.AddComponent<RectTransform>();

                RectTransform canvasRT = canvasEnveloppeObject.GetComponent<RectTransform>();
                worldEnveloppeRT = enveloppeObject.GetComponent<RectTransform>();


                canvasRT.sizeDelta = new Vector2(855f, 984f);
                canvasEnveloppeObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

                canvasRT.anchorMin = new Vector2(0, 1);
                canvasRT.anchorMax = new Vector2(0, 1);
                canvasRT.pivot = new Vector2(0, 1);
                               
                worldEnveloppeRT.SetParent(canvasRT);

                worldEnveloppeRT.anchorMin = new Vector2(0, 1);
                worldEnveloppeRT.anchorMax = new Vector2(0, 1);
                worldEnveloppeRT.pivot = new Vector2(0, 1);

                enveloppeObject.transform.position = new Vector3(0, 0, 0);
            }            
        }

        public void SetGenericScene(bool value)
        {
            isGenericScene = value;
        }

        public void SetWorldEnvelope(object args)
        {
            object[] obj = (object[])args;
            string mapName = (string)obj[0];
            float x = float.Parse((string)obj[1]);
            float y = float.Parse((string)obj[2]);
            if (isGenericScene)
            {
                GameObject.Find(IGamaManager.WORLD_ENVELOPPE).GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
            }
            else
            {
                GameObject.Find(mapName).GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
            }
            
        }


        // The method to call Game Objects methods
        //----------------------------------------
        public void SendMessageToGameObject(GameObject gameObject, string methodName, Dictionary<object, object> data)
        {

            int size = data.Count;
            List<object> keyList = new List<object>(data.Keys);

            System.Reflection.MethodInfo info = gameObject.GetComponent("PlayerController").GetType().GetMethod(methodName);
            ParameterInfo[] par = info.GetParameters();
            /*
            for (int j = 0; j < par.Length; j++)
            {
                System.Reflection.ParameterInfo par1 = par[j];
            }
            */

            switch (size)
            {
                case 0:
                    gameObject.SendMessage(methodName);
                    break;
                case 1:
                    gameObject.SendMessage(methodName, ConvertParameter(data[keyList.ElementAt(0)], par[0]));
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

        public object ConvertParameter(object val, ParameterInfo par)
        {
            object propValue = Convert.ChangeType(val, par.ParameterType);
            return propValue;
        }



        // The method to call Game Objects methods
        //----------------------------------------
        public void SetAttribute(string agentName, Dictionary<string, string> data)
        {
            GameObject targetGameObject = GameObject.Find(agentName);
            MonoBehaviour[] scripts = targetGameObject.GetComponents<MonoBehaviour>();
          
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
                          FieldInfo[] fieldInfoSet2 = fi.FieldType.GetFields();
                        }
                        else
                        {
                            //TODO: need to complete this list
                            switch (fi.FieldType.ToString())
                            {

                                case IDataType.UNITY_INT:
                                    int valInt = Convert.ToInt32(pair.Value);
                                    fi.SetValue(ob, valInt);
                                    break;
                                case IDataType.UNITY_DOUBLE:
                                    double valDouble = Convert.ToDouble(pair.Value);
                                    fi.SetValue(ob, valDouble);
                                    break;
                                case IDataType.UNITY_SINGLE:
                                    fi.SetValue(ob, Convert.ToSingle(pair.Value));
                                    break;
                                case IDataType.UNITY_BOOLEAN:
                                    fi.SetValue(ob, Convert.ChangeType(pair.Value, fi.FieldType));
                                    break;
                                case IDataType.UNITY_STRING:
                                    fi.SetValue(ob, (System.String)pair.Value);
                                    break;
                                case IDataType.UNITY_CHAR:
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
