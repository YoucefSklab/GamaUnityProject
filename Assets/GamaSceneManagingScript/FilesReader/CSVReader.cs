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
        public string path = IGamaManager.CONFIG_PATH + "langs.conf";
        public string lng = Config.LANGUAGE;
        public Dictionary<string, Langue> langueDic = new Dictionary<string, Langue>();

        public void Start()
        {
            LoadCSVFile();
        }

        
        public void LoadCSVFile()
        {
            var langsFile = Resources.Load<TextAsset>(IGamaManager.CONFIG_PATH + "langs");

            if (langsFile != null)
            {
                StreamReader reader = new StreamReader(new MemoryStream(langsFile.bytes));
                string fileContent = reader.ReadToEnd();
                langueDic = GetInDictionnary(fileContent, lng);
            }
        }
        

        // get csv langue in dictionnary
        static public Dictionary<string, Langue> GetInDictionnary(string csvText, string lng)
        {
            string[] lines = csvText.Split("\n"[0]);
            Dictionary<string, Langue> langue = new Dictionary<string, Langue>();

            for (int i = 0; i < lines.Length; i++)
            {
                Langue langueElement = GetLangueElements(lines[i], lng);
                langue.Add(langueElement.element, langueElement);
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