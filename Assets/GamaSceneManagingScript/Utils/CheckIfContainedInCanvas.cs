using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.littosim;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ummisco.gama.unity.utils
{

    public class CheckIfContainedInCanvas
    {


        public CheckIfContainedInCanvas()
        {

        }


        Vector3[] getCanvasWorldCorners(Canvas canvas)
        {
            Vector3[] v = new Vector3[4];
            canvas.GetComponent<RectTransform>().GetWorldCorners(v);
            return v;
        }

        bool isInCanvas(Vector3[] corners, Vector3 p)
        {
            Vector3 p1 = corners[0];
            Vector3 p2 = corners[1];
            Vector3 p3 = corners[2];
            Vector3 p4 = corners[3];

            Debug.Log(" Corners are " + p1 + " and " + p2 + " and " + p3 + " and " + p4 +  " and point is " + p);

            if (isInCanvasArea(p1, p3, p))
            {
                Debug.Log(" true. it is in ");
                return true;
            }

            Debug.Log(" sorry, it is false. it is not in ");
            return false;
        }


        bool isInCanvasArea(Vector3 p1, Vector3 p2, Vector3 p)
        {
            //if (p.x > p1.x && p.x < p2.x && p.y > p1.y && p.y < p2.y)
            if (p.x > p1.x && p.x < p2.x && p.y > p1.y && p.y < p2.y)
                    return true;
            return false;
        }

        public bool isPointInCanvas(Canvas canvas, Vector3 p)
        {
            Debug.Log(" fist level");
            Vector3[] canvasCorners = getCanvasWorldCorners(canvas);
            return isInCanvas(canvasCorners, p);
        }
    }
}
