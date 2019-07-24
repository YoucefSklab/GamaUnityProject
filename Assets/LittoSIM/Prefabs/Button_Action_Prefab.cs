using System;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim.ActionPrefab
{
    public class Button_Action_Prefab : MonoBehaviour
    {
        public string action_name;
        public int code;
        public string button_help_message;
        public string button_icon;
        public string type;
        public Vector3 position;
        public Boolean isOn = false;

        private void FixedUpdate()
        {
            //            Debug.Log("The action code to do is: " + LittosimManager.actionToDo);
            if (LittosimManager.actionToDo == code)
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
            code = action_code;
            button_help_message = msg_help;
            button_icon = icon;
            this.type = type;
            this.position = posi;
      
        }

        public void SetUp(string action_name, int action_code, string msg_help, string icon, string type, Vector3 posi)
        {
            this.action_name = action_name;
            code = action_code;
            button_help_message = msg_help;
            button_icon = icon;
            this.type = type;
            this.position = posi;

            Sprite ImageIcon = Resources.Load<Sprite>(icon);
            if (ImageIcon != null)
                gameObject.GetComponent<Image>().sprite = ImageIcon;
            gameObject.GetComponent<RectTransform>().localPosition = this.position;

        }


        public void onAddButtonClicked()
        {
            Debug.Log("--  --  --  --  > The action code is " + code);// + action.code);
            LittosimManager.actionToDo = code;
        }

        public void ShowTooltip()
        {
            if (!isOn)
            {
                GameObject showTooltip = GameObject.Find("ActionButtonTooltipView");
                string lng = "";

                if (ILangue.current_langue.TryGetValue(this.button_help_message, out lng))
                {
                    GameObject.Find("action_help_message").GetComponent<Text>().text = "  "+lng;
                }
                else
                {
                    GameObject.Find("action_help_message").GetComponent<Text>().text = "  ??";
                }

                Vector3 posi = this.transform.position;
                showTooltip.transform.position = new Vector3(posi.x, posi.y - 130, posi.z + (-20));

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