/*
    CSVReader by Dock. (24/8/11)
    http://starfruitgames.com
 
    usage: 
    CSVReader.SplitCsvGrid(textString)
 
    returns a 2D string array. 
 
    Drag onto a gameobject for a demo of CSV parsing.
*/

using UnityEngine;
using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using ummisco.gama.unity.littosim;
using System;
using ummisco.gama.unity.Scene;
using ummisco.gama.unity.utils;
using UnityEngine.UI;

namespace ummisco.gama.unity.files
{
    //TODO  Use the class CSVParsing to read the data from csv file
    public class CSVReader : MonoBehaviour
    {
        //public string path = IGamaManager.RESOURCES_PATH + "langs_def.csv";
        public string path = IGamaManager.RESOURCES_PATH + "config/langs.conf";
        public string lng = Config.LANGUAGE;
        public Dictionary<string, Langue> langueDic = new Dictionary<string, Langue>();

        public void Start()
        {
            StreamReader reader = new StreamReader(path);
            string content = reader.ReadToEnd();
           langueDic = GetInDictionnary(content, lng);
           //langueDic = GetInDictionnary(new CSVParser().ReadDataIntoString(path), lng);
        }

        
        public void LoadCSVFile()
        {
           langueDic = GetInDictionnary(new StreamReader(path).ReadToEnd(), lng);
        }
        

        // get csv langue in dictionnary
        static public Dictionary<string, Langue> GetInDictionnary(string csvText, string lng)
        {
            string[] lines = csvText.Split("\n"[0]);
            Dictionary<string, Langue> langue = new Dictionary<string, Langue>();

            string allFile = "";

            for (int i = 0; i < lines.Length; i++)
            {
                Langue langueElement = GetLangueElements(lines[i], lng);
                langue.Add(langueElement.element, langueElement);
                allFile += "public static string " + langueElement.element + " = \"" + langueElement.value + "\"; \n";
            }

            return langue;
        }



        public static Langue GetLangueElements(string line, string lng)
        {
            string langueElement = "";
            string fr = "";
            string en = "";

            string[] splitString = line.Split(new string[] { ";" }, StringSplitOptions.None);

            langueElement = splitString[0];
            fr = splitString[1];
            en = splitString[2];

            string value = "";


            if (lng.Equals("fr"))
            {
                value = splitString[1];
                Text txt = GameObject.Find("Te").GetComponent<Text>();
                var textFile = Resources.Load<TextAsset>("config/Test");
                txt.text = textFile.ToString();

            }
            else if (lng.Equals("en"))
            {
                value = splitString[2];
            }



            return new Langue(langueElement, value);
        }

        public void SetLangue(string langueElement, string langue)
        {
            Langue tempElement = null;
            if (langueDic.TryGetValue(langueElement, out tempElement))
            {
                if (langue.Equals("fr"))
                {
                    //                Debug.Log("The element is : " + tempElement.Element + " and value is " + tempElement.Element_fr);
                }
                else if (langue.Equals("en"))
                {
                    //              Debug.Log("The element is : " + tempElement.Element + " and value is " + tempElement.Element_en);
                }

            }
        }

    }
}