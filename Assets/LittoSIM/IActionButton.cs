using System;
using UnityEngine;

namespace ummisco.gama.unity.littosim
{

    public class IActionButton
    {
        public static float max_x = 450; //1400;
        public static float min_x = -450; //-1400;
        public static int action_nbr = 10;
        
        public static float y = 0f;
        public static float z = 0f;

        public static Vector3 GetPosition(int pos_nbr)
        {
            float x = 0f;
            if (pos_nbr >= 2)
            {
                x = (((max_x + (-1 * min_x)) / (action_nbr + 1)) * (pos_nbr - 1 ) ) + min_x;
                return new Vector3(x, y, z);
            }
            else
            {
                return new Vector3(min_x, y, z);
            }
           
        }

    }
}
