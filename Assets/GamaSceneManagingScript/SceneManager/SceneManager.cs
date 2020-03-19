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
        public static bool isOpenGL2;
        public static RectTransform worldEnveloppeRT;
        public static Canvas worldEnveloppeCanvas;
        
        public static Vector2 AnchorMin   // property
        {
            get { return isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1); }
            set => AnchorMin = isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1);
        }

        public static Vector2 AnchorMax   // property
        {
            get { return isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1); }
            set => AnchorMax = isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1);
        }

        public static Vector2 Pivot   // property
        {
            get { return isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1); }
            set => Pivot = isOpenGL2 ? new Vector2(0, 0) : new Vector2(0, 1);
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


        public void Start()
        {
            if (GameObject.Find(IGamaManager.WORLD_ENVELOPPE))
            {
                worldEnveloppeRT = GameObject.Find(IGamaManager.WORLD_ENVELOPPE).GetComponent<RectTransform>();
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

                worldEnveloppeCanvas = canvasEnveloppeObject.GetComponent<Canvas>();

                canvasRT.sizeDelta = new Vector2(855f, 984f);
                canvasEnveloppeObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

                canvasRT.anchorMin = SceneManager.AnchorMin;
                canvasRT.anchorMax = SceneManager.AnchorMax;
                canvasRT.pivot = SceneManager.Pivot;

                worldEnveloppeRT.SetParent(canvasRT);

                canvasRT.anchorMin = SceneManager.AnchorMin;
                canvasRT.anchorMax = SceneManager.AnchorMax;
                canvasRT.pivot = SceneManager.Pivot;

                worldEnveloppeRT.anchorMin = SceneManager.AnchorMin;
                worldEnveloppeRT.anchorMax = SceneManager.AnchorMax;
                worldEnveloppeRT.pivot = SceneManager.Pivot;

               

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

            if (!GameObject.Find(IGamaManager.WORLD_ENVELOPPE))
            {
                if (isGenericScene)
                {
                    CreateEnveloppe();
                }
            }

            if (isGenericScene)
            {
                GameObject.Find(IGamaManager.WORLD_ENVELOPPE).GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
                SetMainCameraInitialPosition(x, y);
            }
            else
            {
                Vector3 p = GameObject.Find("Map_Canvas").transform.position;
                //GameObject.Find("MapCanvas").GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);

                GameObject.Find(mapName).GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
                GameObject.Find(mapName).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                GameObject.Find(mapName).transform.position = p;
                GameObject.Find(mapName).GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);
            }
            
        }

        public void SetMainCameraInitialPosition(float x, float y, float z)
        {
            GameObject mainCamera = GameObject.Find(IGamaManager.GAMA_MAIN_CAMERA);
            mainCamera.transform.parent = GameObject.Find(IGamaManager.WORLD_ENVELOPPE).transform;
            mainCamera.transform.position = new Vector3((x / 2), -(y / 2), IGamaManager.z_axis_main_camera);
        }

        public void SetMainCameraInitialPosition(float x, float y)
        {
            SetMainCameraInitialPosition(x, y, 0);
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

        public Vector2 GetAnchorMin()
        {
            if(isOpenGL2) return new Vector2(0, 0);
            return new Vector2(0, 1);
        }

        public Vector2 GetAnchorMax()
        {
            if (isOpenGL2) return new Vector2(0, 0);
            return new Vector2(0, 1);
        }

        public Vector2 GetPivot()
        {
            if (isOpenGL2) return new Vector2(0, 0);
            return new Vector2(0, 1);
        }

      
        public static System.Type LoadSpeciesScript(string species)
        {
            return System.Type.GetType(species + ",Assembly-CSharp");
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
