using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : EventTrigger
{

    private bool dragging = false;
    public Vector3 screenSpace;
    public Vector3 panelPosition;
    public Vector3 offset;



    public void Update()
    {
        if (dragging)
        {
            //transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log("The transform position is " + transform.position);
            Debug.Log("The transform local position is " + transform.localPosition);
            Debug.Log("The transform Anchored position is " + gameObject.GetComponent<RectTransform>().anchoredPosition);

           // panelPosition = transform.position;
           //panelPosition = transform.localPosition;
           panelPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;

            screenSpace = Camera.main.WorldToScreenPoint(panelPosition);


            offset = panelPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

            Debug.Log("The offset is " + offset);


            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            Debug.Log("The curPosition is " + curPosition);

            //transform.position = curPosition;
            //transform.localPosition = curPosition;
            gameObject.GetComponent<RectTransform>().anchoredPosition = curPosition;

            Debug.Log("The position is " + screenSpace);

            

        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        Debug.Log("Object moved! ");
    }
}
