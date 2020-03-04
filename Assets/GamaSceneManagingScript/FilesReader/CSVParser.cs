
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using ummisco.gama.unity.Scene;

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

        public string ReadDataIntoString(string csvFileName)
        {
            var file = Resources.Load<TextAsset>(IGamaManager.CONFIG_PATH+ csvFileName);

            if (file != null)
            {
                StreamReader reader = new StreamReader(new MemoryStream(file.bytes));
                return reader.ReadToEnd();
            }
            else
            {
                return null;
            }
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