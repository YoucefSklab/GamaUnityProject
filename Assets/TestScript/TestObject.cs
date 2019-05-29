using UnityEngine;

public class TestObject : MonoBehaviour
{
  

    void Start()
    {
       
       transform.Rotate(60, 0, 60);

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

    }
}