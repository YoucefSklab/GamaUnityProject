using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim
{
    public class Protected_Area : MonoBehaviour
    {

        public string name;
        public int meshElevation = 3;

        public Vector3 localScale = new Vector3(0.2f, 0.2f, 0.2f);
               
        // Start is called before the first frame update
        void Start()
        {
            
        }

        void Update()
        {

        }

        public Vector3 GetNewPosition()
        {
            return new Vector3(-2218, -963f, 100f);
        }

    }
}
