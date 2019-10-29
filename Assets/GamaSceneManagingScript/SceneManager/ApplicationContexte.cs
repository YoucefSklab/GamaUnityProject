using System;
using System.Collections.Generic;
using UnityEngine;

namespace ummisco.gama.unity.Scene
{
    public static class ApplicationContexte
    {
        public static Dictionary<string, List<GameObject>> gamaAgentList = new Dictionary<string, List<GameObject>>();

        public static void AddObjectToList(string species, GameObject obj)
        {
            if (gamaAgentList.ContainsKey(species) == true)
            {
                gamaAgentList[species].Add(obj);
            }
            else
            {
                List<GameObject> list = new List<GameObject>();
                list.Add(obj);
                gamaAgentList.Add(species, list);
            }
        }

        public static void RemoveObjectFromList(string species, GameObject obj)
        {
            if (gamaAgentList.ContainsKey(species))
            {
                if (gamaAgentList[species].Contains(obj))
                {
                    gamaAgentList[species].Remove(obj);
                }
            }
        }

    }








}
