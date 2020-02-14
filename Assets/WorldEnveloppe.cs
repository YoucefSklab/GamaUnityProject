using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class WorldEnveloppe : MonoBehaviour
                         , IPointerClickHandler // 2
                         , IDragHandler
                         , IPointerEnterHandler
                         , IPointerExitHandler
                          // ... And many more available!
{
    [SerializeField] float _minZoom = .1f;
    [SerializeField] float _maxZoom = 10;
    [SerializeField] float _zoomLerpSpeed = 5f;
    float _currentZoom = 1;
    bool _isPinching = false;
    float _startPinchDist;
    float _startPinchZoom;
    Vector2 _startPinchCenterPosition;
    Vector2 _startPinchScreenPosition;
    float _mouseWheelSensitivity = 1;
    bool blockPan = false;



    private Vector3 screenPoint;
    private Vector3 offset;

    public static bool BlockedByUI;
    private EventTrigger eventTrigger;

    float scaleFactor = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
            // Pointer Enter
            enterUIEntry.eventID = EventTriggerType.PointerEnter;
            enterUIEntry.callback.AddListener((eventData) => {EnterUI(); });
            eventTrigger.triggers.Add(enterUIEntry);

            //Pointer Exit
            EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
            exitUIEntry.eventID = EventTriggerType.PointerExit;
            exitUIEntry.callback.AddListener((eventData) => { ExitUI(); });
            eventTrigger.triggers.Add(exitUIEntry);
        }
    }

    public void EnterUI()
    {
        Debug.Log("Old fov is EnterUI ");
        BlockedByUI = true;
    }
    public void ExitUI()
    {
        Debug.Log("Old fov is ExitUI ");
        BlockedByUI = false;
    }


    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;

    void Update()
    {
        /*
        float fov = Camera.main.fieldOfView;
        Debug.Log("Old fov is " + fov);

        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;

        Debug.Log("sensitivity fov is " + fov);

        fov = Mathf.Clamp(fov, minFov, maxFov);

        Debug.Log("Mathf fov is " + fov);

        Camera.main.fieldOfView = fov;

        gameObject.transform.localScale *= scaleFactor;

        Debug.Log(" -----> ");
        */

        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollWheelInput) > float.Epsilon)
        {
            _currentZoom *= 1 + scrollWheelInput * _mouseWheelSensitivity;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
            _startPinchScreenPosition = (Vector2)Input.mousePosition;

            if (Mathf.Abs(gameObject.transform.localScale.x - _currentZoom) > 0.001f)
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.one * _currentZoom, _zoomLerpSpeed * Time.deltaTime);



            RectTransform rectTransform = GameObject.Find("Map_Canvas").GetComponent<RectTransform>();
            // gameObject.transform.localScale *= _currentZoom;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, _startPinchScreenPosition, null, out _startPinchCenterPosition);
            Vector2 pivotPosition = new Vector3(rectTransform.pivot.x * rectTransform.rect.size.x, rectTransform.pivot.y * rectTransform.rect.size.y);
            Vector2 posFromBottomLeft = pivotPosition + _startPinchCenterPosition;
            SetPivot(rectTransform, new Vector2(posFromBottomLeft.x / rectTransform.rect.width, posFromBottomLeft.y / rectTransform.rect.height));
            

        }

        if (Input.GetMouseButton(0))
        {
            //Vector2 point = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2); // convert pixel coords to canvas coords
            if (RectTransformUtility.RectangleContainsScreenPoint(GameObject.Find("Map_Canvas").GetComponent<RectTransform>(), Input.mousePosition))
            {
              transform.position = Input.mousePosition;
            }
        }



        Vector3 MapCanvasPosition = GameObject.Find("Map_Canvas").transform.localPosition;
      //gameObject.transform.localPosition = GameObject.Find("Map_Canvas").transform.localPosition;

    }





    static void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
        rectTransform.pivot = pivot;
        rectTransform.localPosition -= deltaPosition;
    }

    void OnMouseEnter()
    {
        gameObject.transform.localScale *= scaleFactor;

        Debug.Log("----****---->>> 33333");
    }

    void OnMouseExit()
    {
        gameObject.transform.localScale /= scaleFactor;

        Debug.Log("9999****----->>>>>");
    }





    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        Debug.Log("66666666666");
    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);


        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;


        Debug.Log("8888888888888");

    }



    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
        throw new System.NotImplementedException();
    }
}
