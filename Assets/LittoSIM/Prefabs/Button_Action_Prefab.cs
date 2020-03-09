using System;
using ummisco.gama.unity.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim.ActionPrefab
{
    public class Button_Action_Prefab : MonoBehaviour
    {
        public string action_name;
        public int action_code;
        public string button_help_message;
        public string button_icon;
        public string type;
        public Vector3 position;
        public Boolean isOn;
        public bool isButtonActivated;
        public bool OnOffButton;
        public string species = "";

        private void Start()
        {
            isOn = false;
        }

        private void FixedUpdate()
        {
            // Debug.Log("The action code to do is: " + LittosimManager.actionToDo);
            // TODO revoir la manière de gérer cette partie. Ajouter un agent qui gére les boutons;
            if (LittosimManager.actionToDo == action_code)
            {
                ColorBlock cb = gameObject.GetComponent<Button>().colors;
                cb.normalColor = Color.magenta;
                gameObject.GetComponent<Button>().colors = cb;
            }
            else
            {
                ColorBlock cb = gameObject.GetComponent<Button>().colors;
                cb.normalColor = Color.white;
                gameObject.GetComponent<Button>().colors = cb;
            }
        }

        public Button_Action_Prefab(string action_name, int action_code, string msg_help, string icon, string type, Vector3 posi)
        {
            this.action_name = action_name;
            this.action_code = action_code;
            button_help_message = msg_help;
            button_icon = icon;
            this.type = type;
            this.position = posi;
        }

        public void SetUp(string action_name, int action_code, string msg_help, string icon, string type, Vector3 posi)
        {
            this.action_name = action_name;
            this.action_code = action_code;
            button_help_message = msg_help;
            button_icon = icon.Remove(icon.Length - 4);
            button_icon = button_icon.Substring(3);
            this.type = type;
            this.position = posi;
            Sprite ImageIcon = Resources.Load<Sprite>(button_icon);
            if (ImageIcon != null)
                gameObject.GetComponent<Image>().sprite = ImageIcon;

            RectTransform rt = gameObject.GetComponent<RectTransform>();
            rt.localPosition = this.position;
            rt.sizeDelta = new Vector2(Config.BUTTON_SIZE/10, Config.BUTTON_SIZE/10);
            rt.sizeDelta = new Vector2(40, 40);
           // rt.sizeDelta = new Vector2(Config.BUTTON_SIZE / 10, Config.BUTTON_SIZE / 10);
        }
        
        public void OnAddButtonClicked()
        {
            if(LittosimManager.actionToDo == action_code)
            {
                LittosimManager.actionToDo = 0;
            }
            else
            {
                if (OnOffButton)
                    if (isButtonActivated)
                    {
                        isButtonActivated = false;
                        SceneManager.SetSpeciesEnabled(species, isButtonActivated);
                    }
                    else
                    {
                        isButtonActivated = true;
                        SceneManager.SetSpeciesEnabled(species, isButtonActivated);
                    }

                LittosimManager.actionToDo = action_code;
                Debug.Log("--  --  --  --  > The action code LittosimManager.actionToDo " + LittosimManager.actionToDo);// + action.code);

                //GameObject.FindWithTag(ILittoSimConcept.LAND_USE_COMMON_BUTTON_TAG).tag = "Player";  
                //GameObject.FindWithTag(ILittoSimConcept.LAND_USE_COMMON_BUTTON_TAG).SetActive(false);
                gameObject.tag = ILittoSimConcept.LAND_USE_COMMON_BUTTON_TAG;
            }
          

            
        }

        public void ShowTooltip()
        {
            if (!isOn)
            {
                GameObject showTooltip = GameObject.Find("ActionButtonTooltipView");
                if (ILangue.current_langue.TryGetValue(this.button_help_message, out string lng))
                {
                    GameObject.Find("action_help_message").GetComponent<Text>().text = " " + lng;
                }
                else
                {
                    GameObject.Find("action_help_message").GetComponent<Text>().text = " ??";
                }

                Vector3 posi = this.transform.position;
                showTooltip.transform.position = new Vector3(posi.x, posi.y - 50, posi.z + (-20));

                isOn = true;
            }

        }

        public void HideTooltip()
        {
            if (isOn)
            {
                GameObject showTooltip = GameObject.Find("ActionButtonTooltipView");
                showTooltip.transform.localPosition = new Vector3(-1000, -300, 0);
                isOn = false;
            }
        }
    }
}