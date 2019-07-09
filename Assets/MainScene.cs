using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.SceneManager;
using ummisco.gama.unity.utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public static List<Agent> gamaAgentList = new List<Agent>();
    private static MainScene m_Instance = null;
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
            //newGameObject.GetComponent<Renderer>().material = mat;
            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            newGameObject.GetComponent<Transform>().SetParent(GameObject.Find("Test").GetComponent<Transform>());
            newGameObject.AddComponent<UA>();
            newGameObject.AddComponent<MeshCollider>();

            newGameObject.GetComponent<UA>().ua_name = agent.agentName + "_";
            newGameObject.GetComponent<UA>().ua_code = 12;
            newGameObject.GetComponent<UA>().population = 12;
            newGameObject.GetComponent<UA>().cout_expro = 12;
            newGameObject.GetComponent<UA>().fullNameOfUAname = agent.agentName + "_FULLNAME_" + 12;
            newGameObject.GetComponent<UA>().classe_densite = agent.agentName + "_CLASSE_DENSITE_" + 12;
        }
    }

    // Update is called once per frame
    void Update()
    {





    }


    public void SceneLoad()
    {
        //StartCoroutine(LoadYourAsyncScene("MainScene"));
        SceneManager.LoadScene("LittoSIMInterface");
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        Scene littosimScene = SceneManager.GetSceneByName("LittoSIMInterface");
        GameObject[] list = littosimScene.GetRootGameObjects();
        foreach (GameObject go in list)
        {
            if (go.name.Equals("GamaManager"))
            {
                Debug.Log("Good, all gameObjects are: " + go.name + " -> ");
            }
        }

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


}
