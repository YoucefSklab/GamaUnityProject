using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using ummisco.gama.unity.Scene;

namespace ummisco.gama.unity.littosim
{
    public class UIManager : MonoBehaviour
    {
        public static string activePanel = IUILittoSim.UA_PANEL;

        private Sprite selectedOnglet;
        private Sprite notSelectedOnglet;

        GameObject Land_Use_ACTION_INSPECT = null;
        GameObject Land_Use_ACTION_DISPLAY_FLOODING = null;
        GameObject Land_Use_ACTION_DISPLAY_FLOODED_AREA = null;
        GameObject Land_Use_ACTION_DISPLAY_PROTECTED_AREA = null;

        GameObject Coastal_Defense_ACTION_INSPECT = null;
        GameObject Coastal_Defense_ACTION_DISPLAY_FLOODING = null;
        GameObject Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA = null;
        GameObject Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA = null;

        GameObject Ua_Panel;
        GameObject Def_Cote_Panel;

        private List<string> UIElementUPLayerList;

        public UIManager()
        {

        }
        void Awake()
        {
            activePanel = IUILittoSim.UA_PANEL;
            selectedOnglet = Resources.Load<Sprite>("images/ihm/onglet_selectionne");
            notSelectedOnglet = Resources.Load<Sprite>("images/ihm/onglet_non_selectionne");
        }

        void Start()
        {

            UIElementUPLayerList = new List<string> { "Button_Action_Prefab", "Action_Recap_Main_Panel", "Messages_Main_Panel", "TooltipView",
            //"Onglets_Area", "Canvas_Tips", "ActionButtonTooltipView",
            "Buttons_Area",
            "Actions_Main_Panel",
            "MiniMapCameraContainer", "Actions_Area", "LittoSIM_Logo_Panel" };

            UIElementUPLayerList = new List<string>
            {
                //"Onglets_Area", "Canvas_Tips", "ActionButtonTooltipView",

            };

            Ua_Panel = GameObject.Find("Ua_Panel");
            Def_Cote_Panel = GameObject.Find("Def_Cote_Panel");
            setActivePanel(IUILittoSim.ONGLET_AMENAGEMENT);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetUp()
        {

        }

        public void setActivePanel(string panelName)
        {
            GetButtonsInstances();
            if (panelName.Equals(IUILittoSim.ONGLET_AMENAGEMENT))
            {
                SetLandUseTab();
            }
            else if (panelName.Equals(IUILittoSim.ONGLET_DEFENSE))
            {
                SetCoastalDefenseTab();
            }
        }



        public void GetButtonsInstances()
        {
            if (Land_Use_ACTION_INSPECT == null)
            {
                Land_Use_ACTION_INSPECT = GameObject.Find("Land_Use_ACTION_INSPECT");
            }
            else
            {
                return;
            }

            if (Land_Use_ACTION_DISPLAY_FLOODING == null)
                Land_Use_ACTION_DISPLAY_FLOODING = GameObject.Find("Land_Use_ACTION_DISPLAY_FLOODING");
            if (Land_Use_ACTION_DISPLAY_FLOODED_AREA == null)
                Land_Use_ACTION_DISPLAY_FLOODED_AREA = GameObject.Find("Land_Use_ACTION_DISPLAY_FLOODED_AREA");
            if (Land_Use_ACTION_DISPLAY_PROTECTED_AREA == null)
                Land_Use_ACTION_DISPLAY_PROTECTED_AREA = GameObject.Find("Land_Use_ACTION_DISPLAY_PROTECTED_AREA");

            if (Coastal_Defense_ACTION_INSPECT == null)
                Coastal_Defense_ACTION_INSPECT = GameObject.Find("Coastal_Defense_ACTION_INSPECT");
            if (Coastal_Defense_ACTION_DISPLAY_FLOODING == null)
                Coastal_Defense_ACTION_DISPLAY_FLOODING = GameObject.Find("Coastal_Defense_ACTION_DISPLAY_FLOODING");
            if (Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA == null)
                Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA = GameObject.Find("Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA");
            if (Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA == null)
                Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA = GameObject.Find("Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA");
        }

        public void SetLandUseCommonButton()
        {
            /*
            Land_Use_ACTION_INSPECT.SetActive(true);
            Land_Use_ACTION_DISPLAY_FLOODING.SetActive(true);
            Land_Use_ACTION_DISPLAY_FLOODED_AREA.SetActive(true);
            Land_Use_ACTION_DISPLAY_PROTECTED_AREA.SetActive(true);

            Coastal_Defense_ACTION_INSPECT.SetActive(false);
            Coastal_Defense_ACTION_DISPLAY_FLOODING.SetActive(false);
            Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA.SetActive(false);
            Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA.SetActive(false);
            */
            SetCanvasGroupInvisible(Ua_Panel);
            SetCanvasGroupVisible(Def_Cote_Panel);
        }

        public void SetCoastalDefenseCommonButton()
        {
            /*
            Land_Use_ACTION_INSPECT.SetActive(false); 
            Land_Use_ACTION_DISPLAY_FLOODING.SetActive(false);
            Land_Use_ACTION_DISPLAY_FLOODED_AREA.SetActive(false);
            Land_Use_ACTION_DISPLAY_PROTECTED_AREA.SetActive(false);

            Coastal_Defense_ACTION_INSPECT.SetActive(true);
            Coastal_Defense_ACTION_DISPLAY_FLOODING.SetActive(true);
            Coastal_Defense_ACTION_DISPLAY_FLOODED_AREA.SetActive(true);
            Coastal_Defense_ACTION_DISPLAY_PROTECTED_AREA.SetActive(true);
            */

            SetCanvasGroupInvisible(Def_Cote_Panel);
            SetCanvasGroupVisible(Ua_Panel);
        }


        public void SetSpriteSelected(string ongletName)
        {
            GameObject.Find(ongletName).GetComponent<Image>().sprite = selectedOnglet;
        }

        public void SetSpriteDeselected(string ongletName)
        {
            GameObject.Find(ongletName).GetComponent<Image>().sprite = notSelectedOnglet;
        }

        void SetCanvasGroupInvisible(GameObject Target)
        {
            CanvasGroup canvasGroup = Target.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f; 
            canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        }

        void SetCanvasGroupVisible(GameObject Target)
        {
            CanvasGroup canvasGroup = Target.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true; 
        }


        void SetTargetInvisible(GameObject Target)
        {
            foreach (Renderer r in Target.GetComponentsInChildren(typeof(Renderer)))
            {
                r.enabled = false;
            }
        }

        void SetTargetVisible(GameObject Target)
        {
            foreach (Renderer r in Target.GetComponentsInChildren(typeof(Renderer)))
            {
                r.enabled = true;
            }
        }

        public static Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
        {
            Vector3 screenPos = worldPos;
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);

          
            return parentCanvas.transform.TransformPoint(movePos);
        }

        public void SetCoastalDefenseTab()
        {
            SetSpeciesActive(IUILittoSim.COASTAL_DEFENSE, true);
            SetSpriteSelected(IUILittoSim.ONGLET_DEFENSE);
            SetSpriteDeselected(IUILittoSim.ONGLET_AMENAGEMENT);
            activePanel = IUILittoSim.DEF_COTE_PANEL;
            SetCoastalDefenseCommonButton();
        }

        public void SetLandUseTab()
        {
            SetSpeciesActive(IUILittoSim.COASTAL_DEFENSE, false);
            SetSpriteSelected(IUILittoSim.ONGLET_AMENAGEMENT);
            SetSpriteDeselected(IUILittoSim.ONGLET_DEFENSE);
            activePanel = IUILittoSim.UA_PANEL;
            SetLandUseCommonButton();

            
        }


        public void SetSpeciesActive(string speciesName, bool value)
        {
            List<GameObject> coastalDefenseElement = null;
            ApplicationContexte.gamaAgentList.TryGetValue(speciesName, out coastalDefenseElement);

            if(coastalDefenseElement != null)
            foreach (GameObject obj in coastalDefenseElement)
            {
                obj.SetActive(value);
            }
        }

        public string getActivePanel()
        {
            return activePanel;
        }

        public static string getActiveMapPanel()
        {
            if (activePanel.Equals(IUILittoSim.DEF_COTE_PANEL))
            {
                return IUILittoSim.DEF_COTE_MAP_PANEL;
            }
            else
            {
                return IUILittoSim.UA_MAP_PANEL;
            }
        }
    }
}