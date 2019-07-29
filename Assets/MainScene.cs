using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.littosim;
using ummisco.gama.unity.messages;
using ummisco.gama.unity.SceneManager;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public static Agent[] gamaAgentList = new Agent[5000];
    private static MainScene m_Instance = null;
    public Material mat;
    void Awake()
    {
        /*
        m_Instance = this;
        if (m_Instance == null)
            m_Instance = this;
        else if (m_Instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(transform.gameObject);
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var agent in gamaAgentList)
        {
            GameObject newGameObject;
            newGameObject = new GameObject(agent.agentName);

            newGameObject.AddComponent(typeof(MeshRenderer));
            newGameObject.AddComponent(typeof(MeshFilter));

            newGameObject.GetComponent<MeshFilter>().mesh = CreateMesh(30, agent.agentCoordinate.getVector2Coordinates());
            newGameObject.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
            newGameObject.GetComponent<Renderer>().material = mat;
            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            newGameObject.GetComponent<Transform>().SetParent(GameObject.Find("Test").GetComponent<Transform>());
            newGameObject.AddComponent<Land_Use>();
            newGameObject.AddComponent<MeshCollider>();

            newGameObject.GetComponent<Land_Use>().lu_name = agent.agentName + "_";
            newGameObject.GetComponent<Land_Use>().lu_code = 12;
            newGameObject.GetComponent<Land_Use>().population = 12;
            newGameObject.GetComponent<Land_Use>().expro_cost = 12;
            newGameObject.GetComponent<Land_Use>().density_class = agent.agentName + "_CLASSE_DENSITE_" + 12;
        }
    }

    // Update is called once per frame
    void Update()
    {





    }


    public void SceneLoad()
    {
        /*
        MainScene.gamaAgentList = GamaManager.gamaAgentList;
        StartCoroutine(LoadYourAsyncScene("MainScene"));
        //SceneManager.LoadScene("LittoSIMInterface");
        */
    }

    public void CreateMesh()
    {
        /*
        MainScene.gamaAgentList = GamaManager.gamaAgentList;
        foreach (var agent in gamaAgentList)
        {
            GameObject newGameObject;
            newGameObject = new GameObject(agent.agentName);

            newGameObject.AddComponent(typeof(MeshRenderer));
            newGameObject.AddComponent(typeof(MeshFilter));

            newGameObject.GetComponent<MeshFilter>().mesh = CreateMesh(30, agent.agentCoordinate.getVector2Coordinates());
            newGameObject.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
            newGameObject.GetComponent<Renderer>().material = mat;
            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            newGameObject.GetComponent<Transform>().SetParent(GameObject.Find("MapElements").GetComponent<Transform>());
            newGameObject.AddComponent<Land_Use>();
            newGameObject.AddComponent<MeshCollider>();

            newGameObject.GetComponent<Land_Use>().lu_name = agent.agentName + "_";
            newGameObject.GetComponent<Land_Use>().lu_code = 12;
            newGameObject.GetComponent<Land_Use>().population = 12;
            newGameObject.GetComponent<Land_Use>().expro_cost= 12;
            newGameObject.GetComponent<Land_Use>().density_class = agent.agentName + "_CLASSE_DENSITE_" + 12;
        }
        */
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        /*
        Scene littosimScene = SceneManager.GetSceneByName("LittoSIMInterface");
        GameObject[] list = littosimScene.GetRootGameObjects();
        foreach (GameObject go in list)
        {
            if (go.name.Equals("GamaManager"))
            {
                Debug.Log("Good, all gameObjects are: " + go.name + " -> ");
            }
        }
        */

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public Mesh CreateMesh(int elevation, Vector2[] vect)
    {
        Mesh mesh = new Mesh();
        Triangulator tri = new Triangulator(vect);
        tri.setAllPoints(tri.Convert2dTo3dVertices());
        mesh.vertices = tri.VerticesWithElevation(elevation);
        mesh.triangles = tri.Triangulate3dMesh();

        // For Android Build
        //        Unwrapping.GenerateSecondaryUVSet(m);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        // For Android Build
        //        MeshUtility.Optimize(m);
        return mesh;
    }



    public void AgentsGenerator()
    {
        Vector2[] vertices2D = new Vector2[] { new Vector2(340.4437f, 739.1506f), new Vector2(350.0437f, 688.6506f), new Vector2(369.9437f, 695.1506f), new Vector2(366.7437f, 712.6506f), new Vector2(374.9437f, 713.9506f), new Vector2(383.5437f, 714.5506f), new Vector2(387.2437f, 714.7506f), new Vector2(387.7437f, 712.6506f), new Vector2(383.7437f, 712.3506f), new Vector2(371.8438f, 708.3506f), new Vector2(373.8438f, 695.3506f), new Vector2(398.5437f, 702.7506f), new Vector2(398.4437f, 705.2506f), new Vector2(406.8438f, 707.4506f), new Vector2(407.3438f, 715.0506f), new Vector2(403.0437f, 754.9506f), new Vector2(390.0437f, 754.8506f), new Vector2(389.8438f, 756.5506f), new Vector2(365.3438f, 752.5506f), new Vector2(363.7437f, 743.0506f), new Vector2(361.1437f, 742.7506f), new Vector2(361.8438f, 745.0506f), new Vector2(339.1437f, 747.0506f) };

        List<GamaPoint> list = new List<GamaPoint>();
        foreach (Vector2 v in vertices2D)
        {
            GamaPoint p = new GamaPoint();
            p.x = v.x;
            p.y = v.y;
            p.z = 0;
            list.Add(p);
        }

        for (int i=0; i<3000; i++)
        {
            UnityAgent unityAgent = new UnityAgent();
            unityAgent.unread = "unread";
            unityAgent.sender = "sender";
            unityAgent.receivers = "receivers";
            unityAgent.emissionTimeStamp = 12;

            Content contents = new Content();

                
            contents.agentName = "agent_"+i;
            contents.species = "UA";
            contents.geometryType = "POLYGON";
            contents.vertices = list;
            contents.color = new GamaColor();
            contents.height = 0;
            contents.location = new GamaPoint(0, 0, 0);

            unityAgent.contents = contents;

            string message = MsgSerialization.serialization(unityAgent);

            Debug.Log("Message is " + message);

            UnityAgent newAgent = (UnityAgent)MsgSerialization.deserialization(message, new UnityAgent());

            Agent gamaAgent = newAgent.GetAgent();

            Debug.Log("Agent created : agent_" + gamaAgent.name);
        }

       




      //
    }


}
