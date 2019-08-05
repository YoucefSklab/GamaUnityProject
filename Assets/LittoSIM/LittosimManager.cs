using System;
using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.messages;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt.Messages;
using ummisco.gama.unity.SceneManager;
using ummisco.gama.unity.littosim.ActionPrefab;
using ummisco.gama.unity.utils;

namespace ummisco.gama.unity.littosim
{
    public class LittosimManager : MonoBehaviour
    {
        public static int actionToDo = 0;
        public static int gameNbr = 0;
        public static GameObject UAPrefab;

        public GameObject ActionPanelPrefab;
        public GameObject ActionRecapPanelPrefab;
        public GameObject MessagePanelPrefab;
        public GameObject ButtonActionPrefab;
        public GameObject UA;
        public GameObject Def_Cote;

        public List<GameObject> actionsList = new List<GameObject>();
        public List<GameObject> recapActionsList = new List<GameObject>();
        public List<GameObject> messagesList = new List<GameObject>();

        public Vector2 initialPosition = new Vector2(0f, 0f);
        public Vector2 lastPosition = new Vector2(0f, 0f);

        public Vector2 initialRecapPosition = new Vector2(480f, -40f);
        public Vector2 lastRecapPosition = new Vector2(480f, -40f);

        public Vector2 initialMessagePosition;
        public Vector2 lastMessagePosition;

        public int elementCounter = 0;
        public int actionCounter = 0;
        public int recapActionCounter = 0;
        public int messageCounter = 0;

        public GameObject valider;
        public GameObject valider_text;
        public GameObject valider_montant;

        public float lineHeight = 80f;
        public float zCoordinate = 60;

        public Canvas uiCanvas = null;
        public Canvas mapCanvas = null;

        private GameObject uiManager;
        private GameObject main_canvas;
        

        void Awak()
        {
           
        }

        void Start()
        {

            AddLittosimTags();

            uiManager = GameObject.Find(IUILittoSim.UI_MANAGER);
            main_canvas = GameObject.Find(IUILittoSim.MAIN_CANVAS);


            initialPosition = new Vector2(0f, 0f);
            lastPosition = new Vector2(0f, 0f);

            initialRecapPosition = new Vector2(480f, -40f);
            lastRecapPosition = new Vector2(480f, -40f);

            initialMessagePosition = new Vector2(1032f, -40f);
            lastMessagePosition = new Vector2(1032f, -40f);
                        
            SetUpLittosimUI();
           

        }

        public void SetUpLittosimUI()
        {
            deactivateValider();

            Destroy(GameObject.Find(IUILittoSim.ACTION_PANEL_PREFAB));
            Destroy(GameObject.Find(IUILittoSim.MESSAGE_PANEL_PREFAB));
            Destroy(GameObject.Find(IUILittoSim.ACTION_RECAP_PANEL_PREFAB));

            CanvasGroup cg = GameObject.Find("Canvas_Tips").GetComponent<CanvasGroup>();
            cg.interactable = false;
            cg.alpha = 0;

           
        }


        public void SetMapPanelSize(object args)
        {
            object[] obj = (object[])args;
            string mapName = (string)obj[0];
            float x = float.Parse((string)obj[1]);
            float y = float.Parse((string)obj[2]);
            GameObject.Find(mapName).GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
        }

       

        void FixedUpdate()
        {

            if (Input.GetMouseButtonDown(0))
            {
                GameObject bj;
                bj = (UnityEngine.EventSystems.EventSystem.current != null) ? UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject : null;

                if (bj != null)
                {
                    Vector3 mouse = Input.mousePosition;
                    if (bj is GameObject)
                        if (bj.tag.Equals("DeleteActionButton"))
                        {
                            Debug.Log(" --------->  to delete : " + bj.name);
                            sendDeleteAction(bj.transform.parent.name);
                        }
                }

            }

            if (actionToDo != 0 && Input.GetMouseButtonDown(0))
            {
                Vector3 position = Input.mousePosition;
                CheckIfContainedInCanvas check = new CheckIfContainedInCanvas();
               
                if (check.isPointInCanvas(mapCanvas, position))
                {
                    Debug.Log("yes mouse activated at position -> " + position);
                    createNewElement();
                }
               

            }

        }

        public void createNewElement()
        {
            Vector3 position = Input.mousePosition;
            Debug.Log("Mouse position is : " + position);
            //position = uiManager.GetComponent<UIManager>().worldToUISpace(uiCanvas, position);
            position = uiManager.GetComponent<UIManager>().worldToUISpace(mapCanvas, position);
            position.z = -80;
            sendGamaMessage(position);

            // To delete
            // TODO to detete 
            //  if (1 == 2) 
            {
                GameObject panelChild = GameObject.CreatePrimitive(PrimitiveType.Cube); //Instantiate(UA);
                GameObject panelParent = GameObject.Find(IUILittoSim.UA_MAP_PANEL);
                panelChild.AddComponent<RectTransform>();
                panelChild.transform.SetParent(panelParent.transform);
                panelChild.GetComponent<RectTransform>().localPosition = new Vector3(0,0,0);
               

                panelChild.name = "UA" + position.x + "_" + position.y;
                panelChild.transform.position = position;
                panelChild.transform.localScale = new Vector3(50f, 50f, 50f);
               
               
                
            }
           Debug.Log("Final created position is :" + position);
        }
               
        void OnGUI()
        {
            Rect bounds = new Rect(58, -843, 1320, 200);
            GUI.Label(bounds, "test");
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.red);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(bounds, GUIContent.none);
        }



        public void sendGamaMessage(Vector3 position)
        {
            switch (actionToDo)
            {
                case ILittoSimConcept.ACTION_URBANISE:
                    publishMessage(getSerializedMessage(ILittoSimConcept.ACTION_URBANISE, position));
                    break;
                case ILittoSimConcept.ACTION_DENSIFIE:
                    publishMessage(getSerializedMessage(ILittoSimConcept.ACTION_DENSIFIE, position));
                    break;
                case ILittoSimConcept.ACTION_DIGUE:
                    publishMessage(getSerializedMessage(ILittoSimConcept.ACTION_DIGUE, position));
                    break;
                case ILittoSimConcept.ACTION_AGRICULTURAL:
                    publishMessage(getSerializedMessage(ILittoSimConcept.ACTION_AGRICULTURAL, position));
                    break;
                case ILittoSimConcept.ACTION_NATURAL:
                    publishMessage(getSerializedMessage(ILittoSimConcept.ACTION_NATURAL, position));
                    break;
            }
            actionToDo = 0;
            gameNbr++;
        }

        public string getSerializedMessage(int idAction, Vector3 position)
        {
            return MsgSerialization.serialization(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, ILittoSimConcept.GAMA_AGENT, idAction, position.x, position.y, DateTime.Now.ToString()));
        }

        public void addCube(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            addObjectOnPanel(type, name, texte, delay, montant);
        }

        public void addSphere(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            addObjectOnPanel(type, name, texte, delay, montant);
        }

        public void addCapsule(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            addObjectOnPanel(type, name, texte, delay, montant);
        }

        public void addCylinder(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            addObjectOnPanel(type, name, texte, delay, montant);

        }

        public void addCube2(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            addObjectOnPanel(type, name, texte, delay, montant);
        }

        public void gamaAddElement(object args)
        {
            object[] obj = (object[])args;
            int type = Int32.Parse((string)obj[0]);
            string name = (string)obj[1];
            string texte = (string)obj[2];
            int delay = Int32.Parse((string)obj[3]);
            int montant = Int32.Parse((string)obj[4]);
            float x = float.Parse((string)obj[5]);
            float y = float.Parse((string)obj[6]);
            float z = float.Parse((string)obj[7]);

            Vector3 position = new Vector3(x, y, z);
            GameObject parentObject = GameObject.Find(UIManager.activePanel);
            switch (type)
            {
                case 1:
                    addCube(position, Color.red, type, name, texte, delay, montant, parentObject);
                    break;
                case 2:
                    addCube(position, Color.blue, type, name, texte, delay, montant, parentObject);
                    break;
                case 3:
                    addCube(position, Color.green, type, name, texte, delay, montant, parentObject);
                    break;
                case 4:
                    addCube(position, Color.yellow, type, name, texte, delay, montant, parentObject);
                    break;
                case 5:
                    addCube(position, Color.white, type, name, texte, delay, montant, parentObject);
                    break;
            }

            elementCounter++;
        }

        public void deleteActionFromList(object args)
        {
            object[] obj = (object[])args;
            string name = (string)obj[0];

            if (actionsList.Count > 0)
            {
                GameObject gameObj = GameObject.Find(name);

                actionsList.Remove(gameObj);

                foreach (var gameOb in actionsList)
                {
                    if (gameOb.name.Equals(name))
                    {
                        actionsList.Remove(gameOb);
                        break;
                    }
                }

                Destroy(gameObj);
                lastPosition = initialPosition;
                foreach (var gameObject in actionsList)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition = lastPosition;
                    lastPosition.y = lastPosition.y - lineHeight;
                }
            }
            //Debug.Log("Here to delete the action from the list the action " + name);
            updateValiderPosition();
        }

        public void gamaAddValidElement(object args)
        {
            object[] obj = (object[])args;
            int type = Int32.Parse((string)obj[0]);
            string name = (string)obj[1];
            string texte = (string)obj[2];
            int delay = Int32.Parse((string)obj[3]);

            GameObject panelParent = GameObject.Find(IUILittoSim.ACTION_LIST_RECAP_PANEL);
            createRecapActionPaneChild(type, name, panelParent, texte, delay);
        }

        public void publishMessage(string message)
        {
            Debug.Log("Prepare to send message");
            //int msgId = GamaManager.client.Publish(LITTOSSIM_TOPIC, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            int msgId = GamaManager.client.Publish(ILittoSimConcept.LITTOSIM_TOPIC, System.Text.Encoding.UTF8.GetBytes(message));
            Debug.Log("-- > msgId is: " + msgId + " -> " + message);
        }

        public void addObjectOnPanel(int type, string name, string texte, int delay, int montant)
        {
            GameObject ActionsPanelParent = GameObject.Find(IUILittoSim.ACTION_LIST_PANEL);
            activateValider();

            switch (type)
            {
                case 1:
                    createActionPaneChild(type, name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 2:
                    createActionPaneChild(type, name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 3:
                    createActionPaneChild(type, name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 4:
                    createActionPaneChild(type, name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 5:
                    createActionPaneChild(type, name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
            }
        }

        public Vector2 getAtActionPanelPosition()
        {
            if (actionsList.Count > 0)
            {

                lastPosition = new Vector2(0, actionsList.Count * (-lineHeight));

                return lastPosition;
            }
            else
            {
                return new Vector2(0f, 0);
                //return initialPosition;
            }
        }

        public Vector2 getAtRecapActionPanelPosition()
        {
            if (recapActionsList.Count > 0)
            {
                return new Vector2(initialRecapPosition.x, -((recapActionsList.Count * lineHeight) + initialRecapPosition.y));
            }
            else
            {
                return initialRecapPosition;
            }
        }

        public void updateRecapActionPosition()
        {
            if (recapActionsList.Count > 0)
            {
                lastRecapPosition = initialRecapPosition;
                foreach (var gameObject in recapActionsList)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition = lastRecapPosition;
                    lastRecapPosition.y = lastRecapPosition.y - lineHeight;
                }
            }
        }

        public Vector2 getAtMessagePanelPosition()
        {
            if (messagesList.Count > 0)
            {
                return new Vector2(initialMessagePosition.x, -((messagesList.Count * lineHeight) + initialMessagePosition.y));
            }
            else
            {
                return initialMessagePosition;
            }
        }

        public void createActionPaneChild(int type, string name, GameObject panelParent, string texte, string delay, string montant)
        {

            GameObject panelChild = Instantiate(ActionPanelPrefab);
            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = name;

            childRectTrans.parent = parentRectTran;
            childRectTrans.anchoredPosition = getAtActionPanelPosition();

            actionsList.Add(panelChild);
            panelChild.transform.Find(IUILittoSim.ACTION_TITLE).GetComponent<Text>().text = texte;
            panelChild.transform.Find(IUILittoSim.ACTION_CYCLE).transform.Find(IUILittoSim.ACTION_CYCLE_VALUE).GetComponent<Text>().text = (delay);
            panelChild.transform.Find(IUILittoSim.ACTION_BUDGET).GetComponent<Text>().text = (montant);


            updateValiderPosition();

            if (actionsList.Count >= 10)
            {
                parentRectTran.sizeDelta = new Vector2(parentRectTran.sizeDelta.x, (parentRectTran.sizeDelta.y + (actionsList.Count - 10) * lineHeight));
            }
            actionCounter++;
        }

        public void updateValiderPosition()
        {
            if (actionsList.Count > 0)
            {
                Vector3 lastActionPosition = actionsList[actionsList.Count - 1].GetComponent<RectTransform>().anchoredPosition;
                Vector3 validerosition = valider.GetComponent<RectTransform>().anchoredPosition;

                validerosition.y = lastActionPosition.y - (lineHeight * 2);
                valider.GetComponent<RectTransform>().anchoredPosition = validerosition;

                validerosition = valider_text.GetComponent<RectTransform>().anchoredPosition;
                validerosition.y = lastActionPosition.y - (lineHeight * 2);
                valider_text.GetComponent<RectTransform>().anchoredPosition = validerosition;

                validerosition = valider_montant.GetComponent<RectTransform>().anchoredPosition;
                validerosition.y = lastActionPosition.y - (lineHeight * 2);
                valider_montant.GetComponent<RectTransform>().anchoredPosition = validerosition;
            }
            else
            {
                deactivateValider();
            }
        }

        public void deactivateValider()
        {
            valider.SetActive(false);//.active = false;
            valider_text.SetActive(false);//.active = false;
            valider_montant.SetActive(false);//.active = false;
        }

        public void activateValider()
        {
            valider.SetActive(true);//.active = false;
            valider_text.SetActive(true);//.active = false;
            valider_montant.SetActive(true);//.active = false;
        }

        public void validateActionList()
        {
            string message = MsgSerialization.serialization(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, "GamaMainAgent", 100, 0, 0, DateTime.Now.ToString()));
            publishMessage(message);
        }

        public void sendDeleteAction(string name)
        {
            string message = MsgSerialization.serialization(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, "GamaMainAgent", 101, name, 0, 0, DateTime.Now.ToString()));
            publishMessage(message);
        }

        public void destroyElement(string name)
        {
            if (GameObject.Find(name))
            {
                GameObject obj = GameObject.Find(name);
                actionsList.Remove(obj);
                Destroy(obj);
                if (actionsList.Count == 0)
                {
                    deactivateValider();
                }
            }
        }

        public void createRecapActionPaneChild(int type, string name, GameObject panelParent, string texte, int delay)
        {
            GameObject panelChild = Instantiate(ActionRecapPanelPrefab);
            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = name;
            childRectTrans.SetParent(parentRectTran);

            childRectTrans.anchoredPosition = getAtRecapActionPanelPosition();
            recapActionsList.Add(panelChild);

            string CycleIcon = geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, name);
            string ValideIcon = geObjectComposedName(IUILittoSim.ACTION_RECAP_VALIDE_ICON, name);
            string CyclePlus = geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, name);
            string ActionTitre = geObjectComposedName(IUILittoSim.ACTION_RECAP_TITRE, name);

            panelChild.transform.Find(IUILittoSim.ACTION_RECAP_TITRE).transform.name = ActionTitre;
            panelChild.transform.Find(IUILittoSim.ACTION_RECAP_CYCLE_ICON).transform.name = CycleIcon;
            panelChild.transform.Find(IUILittoSim.ACTION_RECAP_VALIDE_ICON).transform.name = ValideIcon;
            panelChild.transform.Find(IUILittoSim.ACTION_RECAP_CYCLE_PLUS).transform.name = CyclePlus;

            panelChild.transform.Find(ActionTitre).GetComponent<Text>().text = (texte);
            panelChild.transform.Find(CycleIcon).transform.Find(IUILittoSim.ACTION_RECAP_ROUND).GetComponent<Text>().text = (delay.ToString());

            GameObject.Find(CycleIcon).SetActive(false);
            GameObject.Find(ValideIcon).SetActive(false);
            GameObject.Find(CyclePlus).SetActive(false);



            if (recapActionsList.Count >= 5)
            {
                RectTransform rt = panelParent.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, (rt.sizeDelta.y + ((recapActionsList.Count - 5) * lineHeight)));
            }
            recapActionCounter++;
            updateRecapActionPosition();
        }

        public void newInfoMessage(object args)
        {
            object[] obj = (object[])args;
            int type = Int32.Parse((string)obj[0]);
            string name = (string)obj[1];
            string texte = (string)obj[2];


            GameObject panelChild = Instantiate(MessagePanelPrefab);
            GameObject panelParent = GameObject.Find(IUILittoSim.MESSAGES_PANEL);

            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = name;
            childRectTrans.SetParent(parentRectTran);

            childRectTrans.anchoredPosition = getAtMessagePanelPosition();

            messagesList.Add(panelChild);

            panelChild.transform.Find(IUILittoSim.MESSAGE_TITRE).GetComponent<Text>().text = (texte);

            if (messagesList.Count > 5)
            {
                GameObject gameObj = messagesList[0];
                messagesList.RemoveAt(0);
                Destroy(gameObj);
                lastMessagePosition = initialMessagePosition;
                foreach (var gameObject in messagesList)
                {
                    gameObject.GetComponent<RectTransform>().anchoredPosition = lastMessagePosition;
                    lastMessagePosition.y = lastMessagePosition.y - lineHeight;
                }
            }

            messageCounter++;
        }

        public void setInitialBudget(object args)
        {
            object[] obj = (object[])args;
            string value = (string)obj[0];
            GameObject.Find(IUILittoSim.BUDGET_INITIAL).GetComponent<Text>().text = value;
        }

        public void setRemainingBudget(object args)
        {
            object[] obj = (object[])args;
            string value = (string)obj[0];
            GameObject.Find(IUILittoSim.BUDGET_RESTANT).GetComponent<Text>().text = value;
        }

        public void setValiderMontant(string montant)
        {
            if (GameObject.Find(IUILittoSim.BUDGET_ACTION_LIST))
                GameObject.Find(IUILittoSim.BUDGET_ACTION_LIST).GetComponent<Text>().text = montant;
        }

        public void setValidActionActiveIcon(object args)
        {
            object[] obj = (object[])args;
            string actionName = (string)obj[0];
            bool icon1 = Boolean.Parse((string)obj[1]);
            bool icon2 = Boolean.Parse((string)obj[2]);
            bool icon3 = Boolean.Parse((string)obj[3]);

            GameObject Parent = GameObject.Find(actionName);

            Parent.transform.Find(geObjectComposedName(IUILittoSim.ACTION_RECAP_VALIDE_ICON, actionName)).gameObject.SetActive(icon1);
            Parent.transform.Find(geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, actionName)).gameObject.SetActive(icon2);
            Parent.transform.Find(geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, actionName)).gameObject.SetActive(icon3);
        }

        public void setValidActionText(object args)
        {
            object[] obj = (object[])args;
            string actionName = (string)obj[0];
            string value1 = (string)obj[1];
            string value2 = (string)obj[2];

            GameObject Parent = GameObject.Find(actionName);

            GameObject OB = Parent.transform.Find(geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, actionName)).gameObject;
            OB.transform.Find(IUILittoSim.ACTION_RECAP_ROUND).GetComponent<Text>().text = value1;
            OB = Parent.transform.Find(geObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, actionName)).gameObject;
            OB.transform.Find(IUILittoSim.ACTION_RECAP_CYCLE_PLUS_NBR).GetComponent<Text>().text = value2;
        }

        public string geObjectComposedName(string constVar, string name)
        {
            return constVar + "_" + name;
        }

        public void AddLittosimTags()
        {
            GamaManager.AddTag(ILittoSimConcept.LAND_USE_TAG);
            GamaManager.AddTag(ILittoSimConcept.COASTAL_DEFENSE_TAG);
            GamaManager.AddTag(ILittoSimConcept.DISTRICT_TAG);
            GamaManager.AddTag(ILittoSimConcept.FLOOD_RISK_AREA_TAG);
            GamaManager.AddTag(ILittoSimConcept.PROTECTED_AREA_TAG);
            GamaManager.AddTag(ILittoSimConcept.ROAD_TAG);

            GamaManager.AddTag(ILittoSimConcept.LAND_USE_COMMON_BUTTON_TAG);
            GamaManager.AddTag(ILittoSimConcept.COASTAL_DEFENSE_COMMON_BUTTON_TAG);
        }

    }

}
