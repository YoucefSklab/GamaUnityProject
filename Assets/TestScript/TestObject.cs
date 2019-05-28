using UnityEngine;

public class TestObject : MonoBehaviour
{
    float m_Speed;
    public bool m_WorldSpace;

    void Start()
    {
        //Set the speed of the rotation
        m_Speed = 20.0f;
        //Rotate the GameObject a little at the start to show the difference between Space and Local
        transform.Rotate(60, 0, 60);
<<<<<<< HEAD


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

=======
>>>>>>> parent of c9a8ab8... Desapear GameObjects when outside the map canvas
    }

    void Update()
    {
        
        //Rotate the GameObject in World Space if in the m_WorldSpace state
        if (m_WorldSpace)
            transform.Rotate(Vector3.up * m_Speed * Time.deltaTime, Space.World);
        //Otherwise, rotate the GameObject in local space
        else
            transform.Rotate(Vector3.up * m_Speed * Time.deltaTime, Space.Self);

        //Press the Space button to switch between world and local space states
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Make the current state switch to the other state
            m_WorldSpace = !m_WorldSpace;
            //Output the Current state to the console
            Debug.Log("World Space : " + m_WorldSpace.ToString());
        }
    }
}