using System;
using System.Collections.Generic;
using ummisco.gama.unity.messages;
using UnityEngine;
using UnityEngine.UI;
using ummisco.gama.unity.Scene;

using ummisco.gama.unity.utils;

namespace ummisco.gama.unity.littosim
{
    public class LittosimManager : MonoBehaviour
    {
        public static int actionToDo;
        public static int gameNbr;
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

        public int elementCounter;
        public int actionCounter;
        public int recapActionCounter;
        public int messageCounter;

        public GameObject valider;
        public GameObject valider_text;
        public GameObject valider_montant;

        public float lineHeight = 80f;
        public float zCoordinate = 60;

        public Canvas uiCanvas;
        public Canvas mapCanvas;

        private GameObject uiManager;
       
        

        void Awak()
        {
           
        }

        void Start()
        {

            AddLittosimTags();
            uiManager = GameObject.Find(IUILittoSim.UI_MANAGER);
           
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
            DeactivateValider();

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
                bj = UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject;

                if (bj != null)
                {
                   if (bj is GameObject)
                        if (bj.tag.Equals("DeleteActionButton"))
                        {
                            SendDeleteAction(bj.transform.parent.name);
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
                    CreateNewElement();
                }
               

            }

        }

        public void CreateNewElement()
        {
            Vector3 position = Input.mousePosition;
            Debug.Log("Mouse position is : " + position);
            //position = uiManager.GetComponent<UIManager>().worldToUISpace(uiCanvas, position);
            position = uiManager.GetComponent<UIManager>().worldToUISpace(mapCanvas, position);
            position.z = -80;
            SendGamaMessage(position);

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



        public void SendGamaMessage(Vector3 position)
        {
            switch (actionToDo)
            {
                case ILittoSimConcept.ACTION_URBANISE:
                    PublishMessage(GetSerializedMessage(ILittoSimConcept.ACTION_URBANISE, position));
                    break;
                case ILittoSimConcept.ACTION_DENSIFIE:
                    PublishMessage(GetSerializedMessage(ILittoSimConcept.ACTION_DENSIFIE, position));
                    break;
                case ILittoSimConcept.ACTION_DIGUE:
                    PublishMessage(GetSerializedMessage(ILittoSimConcept.ACTION_DIGUE, position));
                    break;
                case ILittoSimConcept.ACTION_AGRICULTURAL:
                    PublishMessage(GetSerializedMessage(ILittoSimConcept.ACTION_AGRICULTURAL, position));
                    break;
                case ILittoSimConcept.ACTION_NATURAL:
                    PublishMessage(GetSerializedMessage(ILittoSimConcept.ACTION_NATURAL, position));
                    break;
            }
            actionToDo = 0;
            gameNbr++;
        }

        public string GetSerializedMessage(int idAction, Vector3 position)
        {
            return MsgSerialization.ToXML(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, ILittoSimConcept.GAMA_AGENT, idAction, position.x, position.y, DateTime.Now.ToString()));
        }

        public void AddCube(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            AddObjectOnPanel(type, name, texte, delay, montant);
        }

        public void AddSphere(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            AddObjectOnPanel(type, name, texte, delay, montant);
        }

        public void AddCapsule(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            AddObjectOnPanel(type, name, texte, delay, montant);
        }

        public void AddCylinder(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            AddObjectOnPanel(type, name, texte, delay, montant);

        }

        public void AddCube2(Vector3 position, Color color, int type, string name, string texte, int delay, int montant, GameObject parentObject)
        {
            GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            createdObject.transform.position = position;
            createdObject.transform.localScale = new Vector3(40, 40, 40);
            Renderer rend = createdObject.GetComponent<Renderer>();
            rend.material.color = color;
            createdObject.transform.SetParent(parentObject.transform);
            AddObjectOnPanel(type, name, texte, delay, montant);
        }

        public void GamaAddElement(object args)
        {
            object[] obj = (object[])args;
            int type = Int32.Parse((string)obj[0]);
            string eltName = (string)obj[1];
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
                    AddCube(position, Color.red, type, eltName, texte, delay, montant, parentObject);
                    break;
                case 2:
                    AddCube(position, Color.blue, type, eltName, texte, delay, montant, parentObject);
                    break;
                case 3:
                    AddCube(position, Color.green, type, eltName, texte, delay, montant, parentObject);
                    break;
                case 4:
                    AddCube(position, Color.yellow, type, eltName, texte, delay, montant, parentObject);
                    break;
                case 5:
                    AddCube(position, Color.white, type, eltName, texte, delay, montant, parentObject);
                    break;
            }

            elementCounter++;
        }

        public void DeleteActionFromList(object args)
        {
            object[] obj = (object[])args;
            string eltName = (string)obj[0];

            if (actionsList.Count > 0)
            {
                GameObject gameObj = GameObject.Find(eltName);

                actionsList.Remove(gameObj);

                foreach (var gameOb in actionsList)
                {
                    if (gameOb.name.Equals(eltName))
                    {
                        actionsList.Remove(gameOb);
                        break;
                    }
                }

                Destroy(gameObj);
                lastPosition = initialPosition;
                foreach (var objGame in actionsList)
                {
                    objGame.GetComponent<RectTransform>().anchoredPosition = lastPosition;
                    lastPosition.y -= lineHeight;
                }
            }
            //Debug.Log("Here to delete the action from the list the action " + name);
            UpdateValiderPosition();
        }

        public void GamaAddValidElement(object args)
        {
            object[] obj = (object[])args;
            //int type = Int32.Parse((string)obj[0]);
            string objName = (string)obj[1];
            string texte = (string)obj[2];
            int delay = Int32.Parse((string)obj[3]);

            GameObject panelParent = GameObject.Find(IUILittoSim.ACTION_LIST_RECAP_PANEL);
            CreateRecapActionPaneChild(objName, panelParent, texte, delay);
        }

        public void PublishMessage(string message)
        {
            Debug.Log("Prepare to send message");
            //int msgId = GamaManager.client.Publish(LITTOSSIM_TOPIC, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            GamaManager.connector.Publish(ILittoSimConcept.LITTOSIM_TOPIC, message);
        }

        public void AddObjectOnPanel(int type, string name, string texte, int delay, int montant)
        {
            GameObject ActionsPanelParent = GameObject.Find(IUILittoSim.ACTION_LIST_PANEL);
            ActivateValider();

            switch (type)
            {
                case 1:
                    CreateActionPaneChild( name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 2:
                    CreateActionPaneChild( name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 3:
                    CreateActionPaneChild( name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 4:
                    CreateActionPaneChild( name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
                case 5:
                    CreateActionPaneChild( name, ActionsPanelParent, texte, delay.ToString(), montant.ToString());
                    break;
            }
        }

        public Vector2 GetAtActionPanelPosition()
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

        public Vector2 GetAtRecapActionPanelPosition()
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

        public void UpdateRecapActionPosition()
        {
            if (recapActionsList.Count > 0)
            {
                lastRecapPosition = initialRecapPosition;
                foreach (var objGame in recapActionsList)
                {
                    objGame.GetComponent<RectTransform>().anchoredPosition = lastRecapPosition;
                    lastRecapPosition.y -= lineHeight;
                }
            }
        }

        public Vector2 GetAtMessagePanelPosition()
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

        public void CreateActionPaneChild(string name, GameObject panelParent, string texte, string delay, string montant)
        {

            GameObject panelChild = Instantiate(ActionPanelPrefab);
            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = name;

            childRectTrans.parent = parentRectTran;
            childRectTrans.anchoredPosition = GetAtActionPanelPosition();

            actionsList.Add(panelChild);
            panelChild.transform.Find(IUILittoSim.ACTION_TITLE).GetComponent<Text>().text = texte;
            panelChild.transform.Find(IUILittoSim.ACTION_CYCLE).transform.Find(IUILittoSim.ACTION_CYCLE_VALUE).GetComponent<Text>().text = (delay);
            panelChild.transform.Find(IUILittoSim.ACTION_BUDGET).GetComponent<Text>().text = (montant);


            UpdateValiderPosition();

            if (actionsList.Count >= 10)
            {
                parentRectTran.sizeDelta = new Vector2(parentRectTran.sizeDelta.x, (parentRectTran.sizeDelta.y + (actionsList.Count - 10) * lineHeight));
            }
            actionCounter++;
        }

        public void UpdateValiderPosition()
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
                DeactivateValider();
            }
        }

        public void DeactivateValider()
        {
            valider.SetActive(false);//.active = false;
            valider_text.SetActive(false);//.active = false;
            valider_montant.SetActive(false);//.active = false;
        }

        public void ActivateValider()
        {
            valider.SetActive(true);//.active = false;
            valider_text.SetActive(true);//.active = false;
            valider_montant.SetActive(true);//.active = false;
        }

        public void ValidateActionList()
        {
            string message = MsgSerialization.ToXML(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, "GamaMainAgent", 100, 0, 0, DateTime.Now.ToString()));
            PublishMessage(message);
        }

        public void SendDeleteAction(string name)
        {
            string message = MsgSerialization.ToXML(new LittosimMessage(ILittoSimConcept.GAMA_TOPIC, "GamaMainAgent", 101, name, 0, 0, DateTime.Now.ToString()));
            PublishMessage(message);
        }

        public void DestroyElement(string name)
        {
            if (GameObject.Find(name))
            {
                GameObject obj = GameObject.Find(name);
                actionsList.Remove(obj);
                Destroy(obj);
                if (actionsList.Count == 0)
                {
                    DeactivateValider();
                }
            }
        }

        public void CreateRecapActionPaneChild( string name, GameObject panelParent, string texte, int delay)
        {
            GameObject panelChild = Instantiate(ActionRecapPanelPrefab);
            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = name;
            childRectTrans.SetParent(parentRectTran);

            childRectTrans.anchoredPosition = GetAtRecapActionPanelPosition();
            recapActionsList.Add(panelChild);

            string CycleIcon = GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, name);
            string ValideIcon = GeObjectComposedName(IUILittoSim.ACTION_RECAP_VALIDE_ICON, name);
            string CyclePlus = GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, name);
            string ActionTitre = GeObjectComposedName(IUILittoSim.ACTION_RECAP_TITRE, name);

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
            UpdateRecapActionPosition();
        }

        public void NewInfoMessage(object args)
        {
            object[] obj = (object[])args;
            //int type = Int32.Parse((string)obj[0]);
            string nameVal = (string)obj[1];
            string texte = (string)obj[2];


            GameObject panelChild = Instantiate(MessagePanelPrefab);
            GameObject panelParent = GameObject.Find(IUILittoSim.MESSAGES_PANEL);

            RectTransform childRectTrans = panelChild.GetComponent<RectTransform>();
            RectTransform parentRectTran = panelParent.GetComponent<RectTransform>();
            panelChild.name = nameVal;
            childRectTrans.SetParent(parentRectTran);

            childRectTrans.anchoredPosition = GetAtMessagePanelPosition();

            messagesList.Add(panelChild);

            panelChild.transform.Find(IUILittoSim.MESSAGE_TITRE).GetComponent<Text>().text = (texte);

            if (messagesList.Count > 5)
            {
                GameObject gameObj = messagesList[0];
                messagesList.RemoveAt(0);
                Destroy(gameObj);
                lastMessagePosition = initialMessagePosition;
                foreach (var objGame in messagesList)
                {
                    objGame.GetComponent<RectTransform>().anchoredPosition = lastMessagePosition;
                    lastMessagePosition.y -= lineHeight;
                }
            }

            messageCounter++;
        }

        public void SetInitialBudget(object args)
        {
            object[] obj = (object[])args;
            string value = (string)obj[0];
            GameObject.Find(IUILittoSim.BUDGET_INITIAL).GetComponent<Text>().text = value;
        }

        public void SetRemainingBudget(object args)
        {
            object[] obj = (object[])args;
            string value = (string)obj[0];
            GameObject.Find(IUILittoSim.BUDGET_RESTANT).GetComponent<Text>().text = value;
        }

        public void SetValiderMontant(string montant)
        {
            if (GameObject.Find(IUILittoSim.BUDGET_ACTION_LIST))
                GameObject.Find(IUILittoSim.BUDGET_ACTION_LIST).GetComponent<Text>().text = montant;
        }

        public void SetValidActionActiveIcon(object args)
        {
            object[] obj = (object[])args;
            string actionName = (string)obj[0];
            bool icon1 = Boolean.Parse((string)obj[1]);
            bool icon2 = Boolean.Parse((string)obj[2]);
            bool icon3 = Boolean.Parse((string)obj[3]);

            GameObject Parent = GameObject.Find(actionName);

            Parent.transform.Find(GeObjectComposedName(IUILittoSim.ACTION_RECAP_VALIDE_ICON, actionName)).gameObject.SetActive(icon1);
            Parent.transform.Find(GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, actionName)).gameObject.SetActive(icon2);
            Parent.transform.Find(GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, actionName)).gameObject.SetActive(icon3);
        }

        public void SetValidActionText(object args)
        {
            object[] obj = (object[])args;
            string actionName = (string)obj[0];
            string value1 = (string)obj[1];
            string value2 = (string)obj[2];

            GameObject Parent = GameObject.Find(actionName);

            GameObject OB = Parent.transform.Find(GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_ICON, actionName)).gameObject;
            OB.transform.Find(IUILittoSim.ACTION_RECAP_ROUND).GetComponent<Text>().text = value1;
            OB = Parent.transform.Find(GeObjectComposedName(IUILittoSim.ACTION_RECAP_CYCLE_PLUS, actionName)).gameObject;
            OB.transform.Find(IUILittoSim.ACTION_RECAP_CYCLE_PLUS_NBR).GetComponent<Text>().text = value2;
        }

        public string GeObjectComposedName(string constVar, string name)
        {
            return constVar + "_" + name;
        }

        public void AddLittosimTags()
        {
            SceneManager.AddTag(ILittoSimConcept.LAND_USE_TAG);
            SceneManager.AddTag(ILittoSimConcept.COASTAL_DEFENSE_TAG);
            SceneManager.AddTag(ILittoSimConcept.DISTRICT_TAG);
            SceneManager.AddTag(ILittoSimConcept.FLOOD_RISK_AREA_TAG);
            SceneManager.AddTag(ILittoSimConcept.PROTECTED_AREA_TAG);
            SceneManager.AddTag(ILittoSimConcept.ROAD_TAG);
            SceneManager.AddTag(ILittoSimConcept.LAND_USE_COMMON_BUTTON_TAG);
            SceneManager.AddTag(ILittoSimConcept.COASTAL_DEFENSE_COMMON_BUTTON_TAG);
        }

    }

}
