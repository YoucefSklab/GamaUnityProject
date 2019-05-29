using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public class PlaceUIElementAtWorldPosition : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 uiOffset;
    public Canvas canvas;

    void Start()
    {

        this.rectTransform = GetComponent<RectTransform>();

        // Calculate the screen offset
        this.uiOffset = new Vector2((float)canvas.GetComponent<RectTransform>().rect.width / 2f, (float)canvas.GetComponent<RectTransform>().rect.height / 2f);
        //this.uiOffset = new Vector2((float)canvas.GetComponent<RectTransform>().rect.height / 2f, (float)canvas.GetComponent<RectTransform>().rect.width / 2f);

        MoveToClickPoint(gameObject.transform.position);
    }


    public void MoveToClickPoint(Vector3 objectTransformPosition)
    {
        // Get the position on the canvas
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvas.GetComponent<RectTransform>().rect.height, ViewportPosition.y * canvas.GetComponent<RectTransform>().rect.width);
        //Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvas.GetComponent<RectTransform>().rect.width, ViewportPosition.y * canvas.GetComponent<RectTransform>().rect.height);

        Vector2 po = proportionalPosition - uiOffset;

        // Set the position and remove the screen offset
        //this.rectTransform.localPosition = proportionalPosition - uiOffset;

        this.rectTransform.position = new Vector3(po.x, po.y, this.rectTransform.position.z);
    }

    private void Update()
    {

    }
}