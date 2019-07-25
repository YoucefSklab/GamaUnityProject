
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using ummisco.gama.unity.SceneManager;

namespace ummisco.gama.unity.files
{

    public class CSVParser
    {
        private string csvFilePath;
        private char lineSeperater = '\n'; // It defines line seperate character
        private char fieldSeperator = ','; // It defines field seperate chracter

        public CSVParser(string csvFilePath)
        {
            this.csvFilePath = csvFilePath;
        }

        public CSVParser()
        {

        }

        public string readDataIntoString(string csvFilePath)
        {
            StreamReader reader = new StreamReader(csvFilePath);
            return reader.ReadToEnd();
        }

        // Get the path in iOS device
        private string GetiPhoneDocumentsPath()
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }

    }
}