
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using ummisco.gama.unity.SceneManager;

namespace ummisco.gama.unity.utils
{

    public static class CSVParser
    {
        private static string csvFilePath;
        private static char lineSeperater = '\n'; // It defines line seperate character
        private static char fieldSeperator = ','; // It defines field seperate chracter
        
        public static string readDataIntoString(string csvFilePath)
        {
           StreamReader reader = new StreamReader(csvFilePath);
           return reader.ReadToEnd();
        }
        
        // Get the path in iOS device
        private static string GetiPhoneDocumentsPath()
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return path + "/Documents";
        }

    }
}