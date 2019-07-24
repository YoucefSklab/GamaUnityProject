using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ummisco.gama.unity.littosim.ActionPrefab
{
    public class ActionButtonTooltipView : MonoBehaviour
    {
        public string help_text = " ";
        public Vector3 pos = new Vector3();

        private void FixedUpdate()
        {
           
        }

        public bool IsActive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }
       


        void Awake()
        {
            instance = this;
            gameObject.GetComponent<Text>().text = "help message!";
            SetTooltipInvisible();
        }

        public void SetVisible()
        {
            SetTooltipInvisible();
        }

        public void ShowTooltip()
        {
            gameObject.GetComponent<Text>().text = help_text;
            transform.position = new Vector3(pos.x, pos.y - 80f, 0f);
            SetTooltipVisible();
        }

        public void HideTooltip()
        {
            SetTooltipInvisible();
        }


        public void SetTooltipVisible()
        {
            CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
            canvas.alpha = 1f;
            canvas.blocksRaycasts = true;
        }

        public void SetTooltipVisible(bool value)
        {
            CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
            canvas.alpha = 1f;
            canvas.blocksRaycasts = true;
        }

        public void SetTooltipInvisible()
        {
            CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
            canvas.alpha = 0f;
            canvas.blocksRaycasts = false;
        }



        // Standard Singleton Access 
        private static ActionButtonTooltipView instance;

        public static ActionButtonTooltipView Instance
        {
            get
            {
                if (instance == null)
                    instance = Object.FindObjectOfType<ActionButtonTooltipView>();
                return instance;
            }
        }
    }
}