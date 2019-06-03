using ummisco.gama.unity.littosim;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchZoom : MonoBehaviour
{
    public Canvas canvas; // The canvas
    public float zoomSpeed = 0.5f;        // The rate of change of the canvas scale factor
    public float minFov = 15f;
    public float maxFov = 900f;
    public float sensitivity = 10f;

    GameObject uiMananger;

    private void Start()
    {
        uiMananger = GameObject.Find(IUILittoSim.UI_MANAGER);
    }

    void Update()
    {
       
    }


 
 
 private void FixedUpdate()
    {
        //ZoomInOut();
        //ZoomInOutScene();
        ZoomInOutPanel();
        DragDropPanel();
    }

    private void ZoomInOut()
    {
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    private void ZoomInOutScene()
    {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");           //This little peece of code is written by JelleWho https://github.com/jellewie
        if (ScrollWheelChange != 0)
        {                                            //If the scrollwheel has changed
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
            float CamY = Camera.main.transform.position.y;                      //^
            float CamZ = Camera.main.transform.position.z;                      //^
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
        }
    }

    private void ZoomInOutPanel()
    {
        float ZoomAmount  = 0; //With Positive and negative values
        float MaxToClamp  = 10;
        float ROTSpeed  = 10;

        //GameObject gameObj = GameObject.Find("Def_Cote_Panel");
        GameObject gameObj = GameObject.Find(uiMananger.GetComponent<UIManager>().activePanel);
        ZoomAmount += Input.GetAxis("Mouse ScrollWheel");
        ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
        var translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp - Mathf.Abs(ZoomAmount));
        gameObj.transform.Translate(0, 0, translate * ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")));
      
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = GameObject.Find("Def_Cote_Panel").transform.position;
        pos.y = Input.mousePosition.y;
        GameObject.Find("Def_Cote_Panel").transform.position = pos;
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        Debug.Log(" Catch Clicked Object ! ..... ");
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }


    void DragDropPanel()
        {

        bool isMouseDrag = false;
        GameObject target = null;
        Vector3 screenPosition = new Vector3(0f,0f,0f);// = 0;
        Vector3 offset = new Vector3(0f, 0f, 0f);// = 0;

        if (Input.GetMouseButtonDown(0))
        {
           
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null)
            {
                isMouseDrag = true;
                Debug.Log("target position :" + target.transform.position);
                //Convert world position to screen position.
                screenPosition =  Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDrag = false;
        }

        if (isMouseDrag)
        {
            //track mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

            //convert screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

            //It will update target gameobject's current postion.
            target.transform.position = currentPosition;
        }

    }

}