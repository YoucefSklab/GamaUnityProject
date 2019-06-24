using UnityEngine;

public class TestObject : MonoBehaviour
{


    void Start()
    {

        transform.Rotate(60, 0, 60);

        GameObject UaPrefab = (GameObject)Resources.Load("Prefabs/UA", typeof(GameObject));

        Debug.Log("the name ");
        Debug.Log("the name is " + UaPrefab.name);
        GameObject go = new GameObject();
        go = Instantiate(UaPrefab);
        go.name = "uuuuuuuuuuuuuuuu";


        Vector3 p;
        GameObject g1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g1.name = "Cube4";
        p = GameObject.Find("Cube0").transform.position;
        g1.transform.position = new Vector3(p.x - 50, p.y - 150, p.z);
        g1.transform.localScale = new Vector3(40, 40, 40);
        g1.AddComponent<CheckIfContainedInCanvas>();


        GameObject g2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g2.name = "Cube5";
        p = GameObject.Find("Cube1").transform.position;
        g2.transform.position = new Vector3(p.x - 150, p.y - 50, p.z);
        g2.transform.localScale = new Vector3(40, 40, 40);
        g2.AddComponent<CheckIfContainedInCanvas>();

        GameObject g3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g3.name = "Cube6";
        p = GameObject.Find("Cube2").transform.position;
        g3.transform.position = new Vector3(p.x - 50, p.y - 150, p.z);
        g3.transform.localScale = new Vector3(40, 40, 40);
        g3.AddComponent<CheckIfContainedInCanvas>();

        GameObject g4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g4.name = "Cube7";
        p = GameObject.Find("Cube3").transform.position;
        g4.transform.position = new Vector3(p.x - 150, p.y - 50, p.z);
        g4.transform.localScale = new Vector3(40, 40, 40);
        g4.AddComponent<CheckIfContainedInCanvas>();

    }

    void Update()
    {
        GameObject target = null;
        bool isMouseDrag = false;
        Vector3 screenPosition = new Vector3();
        Vector3 offset = new Vector3();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null)
            {
                isMouseDrag = true;
                Debug.Log("target position :" + target.transform.position);
                //Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
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

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}