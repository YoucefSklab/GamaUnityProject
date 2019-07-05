using System;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.littosim.ActionPrefab;
using UnityEngine;


namespace ummisco.gama.unity.littosim.Agents
{
    public class UA_OLD : Agent
    {

        string ua_name = "";
        int id;
        int ua_code = 0;
        //Color my_color = cell_color() update: cell_color();
        int population;
        string classe_densite = " ";
        int cout_expro = 0;
        bool isUrbanType; 
        bool isAdapte; 
        bool isEnDensification;

        public Boolean isOn = false;

        public UA_OLD(string ua_name, int id, int ua_code, int population, int cout_expro, bool isUrbanType, bool isAdapte, bool isEnDensification)
        {
            this.ua_name = ua_name;
            this.id = id;
            this.ua_code = ua_code;
            this.population = population;
            this.cout_expro = cout_expro;
            this.isUrbanType = isUrbanType;
            this.isAdapte = isAdapte;
            this.isEnDensification = isEnDensification;

        }

        private void FixedUpdate()
        {
           
        }

        public void Start()
        {
            //       gameObject.GetComponent<Renderer>().material.color = Color.red;

          
        }



        public void ShowTooltip()
        {
            Debug.Log("UA Tooltip showed");

            if (!isOn)
            {
                GameObject showTooltip = GameObject.Find("MAPTooltipView");
                showTooltip.SetActive(true);
                showTooltip.GetComponent<MAPTooltipView>().SetVisible();//.SendMessage("SetVisible");
                string lng = "";

                /*
                if (ILangue.current_langue.TryGetValue(this.button_help_message, out lng))
                {
                    showTooltip.GetComponent<MAPTooltipView>().help_text = lng;
                }
                else
                {
                    showTooltip.GetComponent<MAPTooltipView>().help_text = "??";
                }
                */

                showTooltip.GetComponent<MAPTooltipView>().help_text = "This should contain the UA descriptive data";


                showTooltip.GetComponent<MAPTooltipView>().pos = this.transform.position;
                showTooltip.SendMessage("ShowTooltip");
                isOn = true;
                Debug.Log("Tooltip showed");
            }
        }

        public void HideTooltip()
        {
            if (isOn)
            {
                GameObject showTooltip = GameObject.Find("MAPTooltipView");
                Debug.Log(" THE GAMEOBJECT NAME IS "+showTooltip.name);
                showTooltip.GetComponent<MAPTooltipView>().HideTooltip();//.SendMessage("HideTooltip");
                isOn = false;
            }
        }

        public void printMessage()
        {
            Debug.Log(" THIS THE MESSAGE ... ");
        }

        public void Update()
        {

        }
    }
}
