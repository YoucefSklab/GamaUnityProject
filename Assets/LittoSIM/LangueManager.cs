using System;
using System.Collections.Generic;
using ummisco.gama.unity.files;
using ummisco.gama.unity.Scene;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class LangueManager : MonoBehaviour
    {
        public static string langue = "fr";

        public Dictionary<string, Langue> langueDic = new Dictionary<string, Langue>();

        void Start()
        {
            ReadConfigFile();
            string lng = Config.LANGUAGE;

            Text text = GameObject.Find("Te").GetComponent<Text>();
            text.text = lng;

            GameObject obj;
            obj = GameObject.Find(IGamaManager.CSV_READER);
            obj.GetComponent<CSVReader>().lng = lng;
            obj.GetComponent<CSVReader>().LoadCSVFile(); 
            langueDic = obj.GetComponent<CSVReader>().langueDic;
                       
            SetUpLangueDictionnary();

            
            //Debug.Log(" -------------------------------> " + GetLangueElementValue(langueDic, ILangue.MSG_INITIAL_BUDGET, "en", ILangue.MSG_INITIAL_BUDGET));

            GameObject.Find(ILittoSimConcept.MSG_INITIAL_BUDGET).GetComponent<Text>().text = ILangue.GetLangueElement(ILangue.MSG_INITIAL_BUDGET);
            GameObject.Find(ILittoSimConcept.MSG_REMAINING_BUDGET).GetComponent<Text>().text = ILangue.GetLangueElement(ILangue.MSG_REMAINING_BUDGET); 
            GameObject.Find(ILittoSimConcept.LEGEND_PLU).GetComponentInChildren<Text>().text = "  " + ILangue.GetLangueElement(ILangue.LEGEND_PLU);
            GameObject.Find(ILittoSimConcept.LEGEND_DYKE).GetComponentInChildren<Text>().text = "  " + ILangue.GetLangueElement(ILangue.LEGEND_DYKE);
            GameObject.Find(ILittoSimConcept.LEGEND_NAME_ACTIONS).GetComponent<Text>().text = ILangue.GetLangueElement(ILangue.LEGEND_NAME_ACTIONS);
                      

        }

        public string GetLangueElementValue(Dictionary<string, Langue> dic, string elementName, string langue, string defaultName)
        {
            if (dic.TryGetValue(elementName, out Langue tempElement))
            {
                if (langue.Equals("fr"))
                {
                    return tempElement.value;
                }
                else if (langue.Equals("en"))
                {
                    return tempElement.value;
                }
            }
            return defaultName;
        }

        public void SetUpLangueDictionnary()
        {
            ILangue.current_langue.Clear();
            foreach (KeyValuePair<string, Langue> lng in langueDic)
            {
                ILangue.current_langue.Add(lng.Key, lng.Value.value);
//                Debug.Log("Langue element added is : " + lng.Key + " it's value is "+ lng.Value.value);
            }

 //           Debug.Log("The dic size is " + ILangue.current_langue.Count);

        }


        public void ReadConfigFile()
        {
            CSVParser csvParser = new CSVParser();
            string fileContent = csvParser.ReadDataIntoString("littosim");

            string[] lines = fileContent.Split("\n"[0]);
           
            foreach (string line in lines)
            {
                string[] splitString = line.Split(new string[] { ";" }, StringSplitOptions.None);

                if (splitString.Length > 1)
                {
                    string elt = splitString[0];
                    switch (elt){
                        case "SERVER_ADDRESS":
                            Config.SERVER_ADDRESS = splitString[1];
                            break;
                        case "ACTIVEMQ_CONNECT":
                            Config.ACTIVEMQ_CONNECT = bool.Parse(splitString[1]);
                            break;
                        case "LANGUAGE":
                            Config.LANGUAGE = splitString[1];
                            break;
                        case "BUTTON_SIZE":
                            Config.BUTTON_SIZE = Int32.Parse(splitString[1]);
                            break;
                        case "LOG_USER_ACTION":
                            Config.LOG_USER_ACTION = bool.Parse(splitString[1]);
                            break;
                        case "SAVE_SHP":
                            Config.SAVE_SHP = bool.Parse(splitString[1]);
                            break;
                        case "SHAPES_FILE":
                            Config.SHAPES_FILE = splitString[1];
                            break;
                        case "LANGUAGES_FILE":
                            Config.LANGUAGES_FILE = splitString[1];
                            break;
                        case "LEVERS_FILE":
                            Config.LEVERS_FILE = splitString[1];
                            break;
                        case "LISFLOOD_PATH":
                            Config.LISFLOOD_PATH = splitString[1];
                            break;
                        default :
                            break;
                    }
                }
            }

        }


    }
}
