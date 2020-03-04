using System;
using System.Collections.Generic;

public static class Config
{
        //[ Network]
        public static string SERVER_ADDRESS = "localhost";
        public static bool ACTIVEMQ_CONNECT = false;

        //[ User]
        public static string LANGUAGE = "en";
        public static List<string> LANGUAGE_LIST = new List<string>();
        public static float BUTTON_SIZE  = 100;
        public static bool LOG_USER_ACTION = true;
        public static bool SAVE_SHP = false;

        // [ Files]
        public static string  SHAPES_FILE = "../includes/oleron/study_area.conf";
        public static string  LANGUAGES_FILE = "../includes/config/langs.conf";
        public static string  LEVERS_FILE = "../includes/config/levers.conf";
        public static string  LISFLOOD_PATH = "C:/Users/Laatabi/Desktop/workspace/LittoDev/includes/lisflood/"; 
}
