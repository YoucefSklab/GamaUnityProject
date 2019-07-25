using System;
using System.Collections.Generic;
using System.IO;
using ummisco.gama.unity.littosim.ActionPrefab;
using ummisco.gama.unity.SceneManager;
using UnityEngine;
using UnityEngine.UI;

//TODO  Use the class CSVParsing to read the data from csv file
namespace ummisco.gama.unity.littosim
{
    public class ActionManager : MonoBehaviour
    {

        public Dictionary<string, Action> actions_dic = new Dictionary<string, Action>();
        public string path;
        public List<string> communActions; 
        public string contentArea; 

        public char lineSeperater = '\n';
        public char fieldSeperator = ';';

        public ActionManager()
        {
            // Debug.Log("--  --  --  --  > The action code is " );
        }


        public void Start()
        {
            communActions = new List<string>() { "ACTION_INSPECT", "ACTION_DISPLAY_FLOODING", "ACTION_DISPLAY_FLOODED_AREA", "ACTION_DISPLAY_PROTECTED_AREA" };
            path = IGamaManager.RESOURCES_PATH + "config/actions.conf";
            StreamReader reader = new StreamReader(path);
            string fileContent = reader.ReadToEnd();
            actions_dic = GetActionsList(fileContent);
            Dictionary<string, Action> us_actions_dic = GetUAActionsList();
            Dictionary<string, Action> def_cote_actions_dic = GetDefCotActionsList();

            SetUpUAActions(us_actions_dic);
            SetUpDefCoteActions(def_cote_actions_dic);
            AddCommunButtons();

        }

        // Read data from CSV file
        private void readData(string fileContent)
        {
            string[] records = fileContent.Split(lineSeperater);

            foreach (string record in records)
            {
                string[] fields = record.Split(fieldSeperator);

                foreach (string field in fields)
                {
                    contentArea += field + "\t";
                }
                contentArea += '\n';
            }
        }

        public Dictionary<string, Action> GetActionsList(string fileContent)
        {
            Dictionary<string, Action> actions_list = new Dictionary<string, Action>();
            string[] lines = fileContent.Split(lineSeperater);
            for(int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Length > 2)
                {
                    Action act = GetActionElement(lines[i]);
                    if (((act.coast_def_index >= 1) || (act.lu_index >= 1)) ||  (communActions.Contains(act.action_name)))
                    {
                        actions_list.Add(act.action_name, act);
                    }
                }                
            }
            return actions_list;
        }


        public Action GetActionElement(string line)
        {
            string[] fields = line.Split(fieldSeperator);
                        
            Action act = new Action();

            act.action_name = fields[0];
            if (!Int32.TryParse(fields[1], out act.action_code)) { act.action_code = -1; }
            if (!Int32.TryParse(fields[2], out act.delay)) { act.delay = -1; }
            if (!float.TryParse(fields[3], out act.cost)) { act.cost = -1; }
            act.entity = fields[4];
            act.button_help_message = fields[5];
            act.button_icon_file = fields[6];
            if (!Int32.TryParse(fields[7], out act.coast_def_index)) { act.coast_def_index = -1; }
            if (!Int32.TryParse(fields[8], out act.lu_index)) { act.lu_index = -1; }

            return act;
        }


        public Dictionary<string, Action> GetUAActionsList()
        {
            Dictionary<string, Action> ua_actions_list = new Dictionary<string, Action>();

            foreach (KeyValuePair<string, Action> act in actions_dic)
            {
                if (act.Value.lu_index >= 1)
                {
                    ua_actions_list.Add(act.Key, act.Value);
                }
            }
            return ua_actions_list;
        }

        public Dictionary<string, Action> GetDefCotActionsList()
        {
            Dictionary<string, Action> def_cote_actions_list = new Dictionary<string, Action>();

            foreach (KeyValuePair<string, Action> act in actions_dic)
            {
                if (act.Value.coast_def_index >= 1)
                {
                    def_cote_actions_list.Add(act.Key, act.Value);
                }
            }
            return def_cote_actions_list;
        }

        public void SetUpUAActions(Dictionary<string, Action> actions)
        {
            GameObject Ua_Panel = GameObject.Find(IUILittoSim.UA_PANEL);

            foreach (KeyValuePair<string, Action> act in actions)
            {
                
                GameObject action_button = Instantiate(GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().ButtonActionPrefab);
                action_button.name = "Land_Use_" + act.Key;
                action_button.GetComponent<RectTransform>().SetParent(Ua_Panel.GetComponent<RectTransform>());
                action_button.GetComponent<Button_Action_Prefab>().SetUp("Land_Use_" + act.Key, act.Value.action_code, act.Value.button_help_message, act.Value.button_icon_file, "Land_Use", IActionButton.GetPosition(act.Value.lu_index));

            }
        }

        public void SetUpDefCoteActions(Dictionary<string, Action> actions)
        {

            GameObject Def_Cote_Panel = GameObject.Find(IUILittoSim.DEF_COTE_PANEL);

            foreach (KeyValuePair<string, Action> act in actions)
            {
                GameObject action_button = Instantiate(GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().ButtonActionPrefab);
                action_button.name = "Coastal_Defense_" + act.Key;
                action_button.GetComponent<RectTransform>().SetParent(Def_Cote_Panel.GetComponent<RectTransform>());
                action_button.GetComponent<Button_Action_Prefab>().SetUp("Coastal_Defense_" + act.Key, act.Value.action_code, act.Value.button_help_message, act.Value.button_icon_file, "Coastal_Defense", IActionButton.GetPosition(act.Value.coast_def_index));
            }
        }

        public void AddCommunButtons()
        {
            GameObject Ua_Panel = GameObject.Find(IUILittoSim.UA_PANEL);
            GameObject Def_Cote_Panel = GameObject.Find(IUILittoSim.DEF_COTE_PANEL);
            Action act = null;

            int nbrAct = communActions.Count;

            foreach(string action in communActions)
            {
                actions_dic.TryGetValue(action, out act);

                if (act != null)
                {
                    //Coastal Defense 
                    GameObject action_button = Instantiate(GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().ButtonActionPrefab);
                    action_button.name = "Land_Use_" + action;
                    action_button.GetComponent<RectTransform>().SetParent(Ua_Panel.GetComponent<RectTransform>());
                    action_button.GetComponent<Button_Action_Prefab>().SetUp("Land_Use_" + action, act.action_code, act.button_help_message, act.button_icon_file, "Land_Use", IActionButton.GetPosition(13 - nbrAct));

                    action_button = Instantiate(GameObject.Find(ILittoSimConcept.LITTOSIM_MANANGER).GetComponent<LittosimManager>().ButtonActionPrefab);
                    action_button.name = "Coastal_Defense_" + action;
                    action_button.GetComponent<RectTransform>().SetParent(Def_Cote_Panel.GetComponent<RectTransform>());
                    action_button.GetComponent<Button_Action_Prefab>().SetUp("Coastal_Defense_" + action, act.action_code, act.button_help_message, act.button_icon_file, "Coastal_Defense", IActionButton.GetPosition(13 - nbrAct));
                    nbrAct--;
                }
                

            }


           

            





        }
    }
}
