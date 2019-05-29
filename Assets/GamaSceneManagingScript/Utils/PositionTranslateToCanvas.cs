using System;
using UnityEngine;

namespace ummisco.gama.unity.utils

{
    public static class PositionTranslateToCanvas
    {

        public static Vector3 PositionTransalteToCanvas(GameObject gameObject, Canvas canvas)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 uiOffset = new Vector2((float)canvas.GetComponent<RectTransform>().rect.width / 2f, (float)canvas.GetComponent<RectTransform>().rect.height / 2f);
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvas.GetComponent<RectTransform>().rect.height, ViewportPosition.y * canvas.GetComponent<RectTransform>().rect.width);
          
            Vector2 po = proportionalPosition - uiOffset;

            return new Vector3(po.x, po.y, rectTransform.position.z);
        }
    }
}
