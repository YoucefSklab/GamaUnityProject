using System;
using ummisco.gama.unity.datastructure;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.geometry;
using ummisco.gama.unity.littosim;
using ummisco.gama.unity.Scene;
using UnityEngine;

public class AgentCreator : MonoBehaviour
{
    private MeshCreator meshCreator = new MeshCreator();

    public Material lineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void CreateAgent(Agent agent, Transform parentTransform, Material mat, int speciesId, bool elevate, string tagName, float zAxis)
    {
        GameObject newObject = new GameObject(agent.AgentName);
        MeshRenderer meshRenderer = (MeshRenderer) newObject.AddComponent(typeof(MeshRenderer));
        MeshFilter meshFilter = (MeshFilter) newObject.AddComponent(typeof(MeshFilter));
        MeshCollider meshCollider = (MeshCollider) newObject.AddComponent<MeshCollider>();


        Debug.Log("----> Agent's attributes are : "+agent.Attributes.Count);

        foreach(AgentAttribute a in agent.Attributes)
        {
            Debug.Log("Attribute name is: " + a.name);
        }

        newObject.GetComponent<Transform>().SetParent(parentTransform);
        float elvation = elevate ? agent.Height : 0;

        meshFilter.mesh = meshCreator.CreateMesh(elvation, agent.AgentCoordinate.GetVector2Coordinates());
        //newObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(agent.height, agent.ConvertVertices());

        meshFilter.mesh.name = "CustomMesh";
        //newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(30, agent.ConvertVertices());
        //mat.color = agent.color.getColorFromGamaColor();
        meshRenderer.material = mat;
        meshCollider.sharedMesh = meshFilter.mesh;

        Vector3 posi = agent.Location;
        posi.y = -posi.y;

        //posi = uiManager.GetComponent<UIManager>().worldToUISpace(canvas, posi);
        
        RectTransform rt = (newObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();

        rt.anchorMin = SceneManager.AnchorMin;
        rt.anchorMax = SceneManager.AnchorMax;
        rt.pivot = SceneManager.Pivot;

        newObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        posi = newObject.GetComponent<RectTransform>().localPosition;
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(posi.x, posi.y, zAxis);

        SetAgentTag(newObject, tagName);
        AttacheCode(newObject, speciesId, agent);
        AddAgentToContexte(agent.Species, newObject);
    }

    public void SetAgentTag(GameObject newObject, string tagName)
    {
        if (tagName != null)
        {
            newObject.tag = tagName;
        }
        else
        {
            newObject.tag = "unknown";
        }
    }

    public void CreateGenericPolygonAgent(Agent agent, bool elevate, string tagName, float zAxis)
    {
        CreateGenericPolygonAgent(agent, elevate, tagName, zAxis, true);
    }

    public void CreateGenericPolygonAgent(Agent agent, bool elevate, string tagName, float zAxis, bool groupBySpecies)
    {
        GameObject newObject = new GameObject()
        {
            name = agent.AgentName
        };
        
        SetAgentTag(newObject, tagName);
        newObject.AddComponent<Agent>();
        newObject.GetComponent<Agent>().SetAttributes(agent);
        newObject.GetComponent<Agent>().InitAgent(SceneManager.worldEnveloppeRT, elevate, zAxis);
        if (groupBySpecies)
        {
            SetObjectSpecies(newObject, agent.Species);
        }
      
        AddAgentToContexte(agent.Species, newObject);
    }


    public void SetObjectSpecies(GameObject newObject, string species)
    {
        if(GameObject.Find(species))
        {
            newObject.transform.SetParent(GameObject.Find(species).transform);
        }
        else
        {
            GameObject obj = new GameObject(species);
            newObject.transform.SetParent(obj.transform);
            obj.transform.SetParent(SceneManager.worldEnveloppeRT);
        }
    }

    public void CreateGenericPointAgent(Agent agent, float height, string tagName, float zAxis)
    {
        CreateGenericPointAgent(agent, height, tagName, zAxis, true);
    }

    public void CreateGenericPointAgent(Agent agent, float height, string tagName, float zAxis, bool groupBySpecies)
    {
        
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newObject.name = agent.AgentName;
        newObject.transform.localScale = new Vector3(height, height, height);

        //MeshRenderer meshRenderer = (MeshRenderer)newObject.AddComponent(typeof(MeshRenderer));
        //MeshFilter meshFilter = (MeshFilter)newObject.AddComponent(typeof(MeshFilter));
        //MeshCollider meshCollider = (MeshCollider)newObject.AddComponent<MeshCollider>();

        MeshFilter meshFilter = (MeshFilter)newObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = (MeshRenderer)newObject.GetComponent<MeshRenderer>();
        MeshCollider meshCollider = (MeshCollider)newObject.GetComponent<MeshCollider>();

        newObject.GetComponent<Transform>().SetParent(SceneManager.worldEnveloppeRT);
        float elvation = height;

        //meshFilter.mesh = meshCreator.CreateMesh2(elvation, agent.agentCoordinate.getVector2Coordinates());
        //newObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(agent.height, agent.ConvertVertices());

        meshFilter.mesh.name = "CustomMesh";
        //newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(30, agent.ConvertVertices());

        Material mat = new Material(Shader.Find("Specular"));
        //mat.color = agent.color.getColorFromGamaColor();
        mat.color = Color.yellow;
        meshRenderer.material = mat;
        //meshCollider.sharedMesh = meshFilter.mesh;

        Vector3 posi = agent.Location;
        posi.y = -posi.y;

        /*
        //posi = uiManager.GetComponent<UIManager>().worldToUISpace(canvas, posi);

        RectTransform rt = (newObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();

        rt.anchorMin = SceneManager.AnchorMin; 
        rt.anchorMax = SceneManager.AnchorMax;
        rt.pivot = SceneManager.Pivot;

        newObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
       // posi = newObject.GetComponent<RectTransform>().localPosition;
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(posi.x, posi.y, zAxis);
        */

        newObject.transform.localPosition = new Vector3(posi.x, posi.y, zAxis);

        SetAgentTag(newObject, tagName);

        if (groupBySpecies)
        {
            SetObjectSpecies(newObject, agent.Species);
        }
        //AttacheCode(newObject, speciesId, agent);
        AddAgentToContexte(agent.Species, newObject);
    }


    public void CreateLineAgent(Agent agent, Transform parentTransform, Material mat, int speciesId, bool elevate, float lineWidth, string tagName, float zPosition)
    {
        GameObject newObject = new GameObject(agent.AgentName);
        newObject.GetComponent<Transform>().SetParent(parentTransform);
        var meshFilter = newObject.AddComponent<MeshFilter>();
        LineRenderer line = newObject.AddComponent<LineRenderer>();
        Mesh mesh = new Mesh();
        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = lineMaterial;
        meshRenderer.sharedMaterial = mat;
        //LineRenderer line = (LineRenderer)newObject.GetComponent(typeof(LineRenderer));

        line.useWorldSpace = true;

        line.positionCount = agent.AgentCoordinate.GetVector3Coordinates().Length;
        line.SetPositions(agent.AgentCoordinate.GetVector3Coordinates());
        //line.positionCount = agent.agentCoordinate.getVector2Coordinates().Length / 2;
        line.material = lineMaterial;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;
       
        RectTransform rt = newObject.AddComponent<RectTransform>(); //(newObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();

        rt.anchorMin = SceneManager.AnchorMin;
        rt.anchorMax = SceneManager.AnchorMax;
        rt.pivot = SceneManager.Pivot;

        rt.anchoredPosition = new Vector3(0, 0, 0);
        Vector3 p = rt.localPosition;
        rt.localPosition = new Vector3(p.x, p.y, zPosition);

        SetAgentTag(newObject, tagName);

        SetObjectSpecies(newObject, agent.Species);
      

        Destroy(line);

    }

    public void CreateGenericLineAgent(Agent agent, float lineWidth, string tagName, float zPosition)
    {
        CreateGenericLineAgent(agent, lineWidth, tagName, zPosition, true);
    }
        public void CreateGenericLineAgent(Agent agent, float lineWidth, string tagName, float zPosition, bool groupBySpecies)
    {
        GameObject newObject = new GameObject(agent.AgentName);
        var meshFilter = newObject.AddComponent<MeshFilter>();
        LineRenderer line = newObject.AddComponent<LineRenderer>();
        Mesh mesh = new Mesh();
        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = lineMaterial;
        Material mat = new Material(Shader.Find("Specular"))
        {
            color = Color.red
        };
        meshRenderer.sharedMaterial = mat;
        //LineRenderer line = (LineRenderer)newObject.GetComponent(typeof(LineRenderer));

        line.useWorldSpace = true;

        line.positionCount = agent.AgentCoordinate.GetVector3Coordinates().Length;
        line.SetPositions(agent.AgentCoordinate.GetVector3Coordinates());
        //line.positionCount = agent.agentCoordinate.getVector2Coordinates().Length / 2;
        line.material = lineMaterial;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        RectTransform rt = newObject.AddComponent<RectTransform>(); //(newObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();
        rt.SetParent(SceneManager.worldEnveloppeRT);
        rt.anchorMin = SceneManager.AnchorMin; 
        rt.anchorMax = SceneManager.AnchorMax;
        rt.pivot = SceneManager.Pivot;

        rt.anchoredPosition = new Vector3(0, 0, 0);
        Vector3 p = rt.localPosition;
        rt.localPosition = new Vector3(p.x, p.y, zPosition);

        SetAgentTag(newObject, tagName);
        if (groupBySpecies)
        {
            SetObjectSpecies(newObject, agent.Species);
        }
        

        AddAgentToContexte(agent.Species, newObject);

        Destroy(line);

    }

    public void AddAgentToContexte(string species, GameObject newObject)
    {
        ApplicationContexte.AddObjectToList(species, newObject);
    }

    public void CreateLine()
    {
  
        Vector3[] v = { new Vector3(2397, 901, 0), new Vector3(2388, 909, 0), new Vector3(2376, 917, 0), new Vector3(2352, 933, 0), new Vector3(2327, 949, 0), new Vector3(2296, 970, 0), new Vector3(2267, 989, 0), new Vector3(2237, 1009, 0), new Vector3(2207, 1029, 0), new Vector3(2197, 1036, 0), new Vector3(2189, 1043, 0), new Vector3(2182, 1050, 0), new Vector3(2175, 1059, 0), new Vector3(2171, 1069, 0), new Vector3(2168, 1079, 0) };
        GameObject newObject = new GameObject("TEST_LINE_AGENT_1", typeof(LineRenderer));
        newObject.GetComponent<Transform>().SetParent(GameObject.Find("WorldEnveloppe").GetComponent<RectTransform>());
       // newObject.AddComponent<LineRenderer>();
        //LineRenderer line = (LineRenderer)newObject.GetComponent(typeof(LineRenderer));
        LineRenderer line = newObject.GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = v.Length;
        line.SetPositions(v);
        //line.positionCount = agent.agentCoordinate.getVector2Coordinates().Length / 2;

        //line.material = new Material(Shader.Find("Particles/Additive"));

        line.material = lineMaterial; // new Material(Shader.Find("Standard"));
        //line.material = new Material(Shader.Find("Particles/Standard Surface"));
        //line.material.color = Color.red;
        //Color c1 = Color.red;
        //line.startColor = c1;
        //line.endColor = c1;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        /*
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        line.colorGradient = gradient;
        */

        line.startWidth = 20.0f;
        line.endWidth = 20.0f;
        line.Simplify(10);
        
        // line.        
        
        RectTransform rt = newObject.AddComponent<RectTransform>().GetComponent<RectTransform>();

        /*

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        //newObject.GetComponent<Transform>().localPosition = posi;
        //newObject.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);//posi;
        */
        Vector3 p = new Vector3(2262, 996, 0);
      
        newObject.GetComponent<RectTransform>().anchoredPosition = p;//new Vector3(0, 0, 0);//posi


        //var lineRenderer = lineObj.GetComponent<LineRenderer>();
        var meshFilter = newObject.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        line.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        var meshRenderer = newObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = lineMaterial;

        Destroy(line);

    }

    public void AttacheCode(GameObject obj, int speciesId, Agent agent)
    {
        
        switch (speciesId)
        {
            case IUILittoSim.LAND_USE_ID: // Land_Use
                obj.AddComponent<Land_Use>();

                Int32.TryParse(agent.GetAttributeValue("ID"), out obj.GetComponent<Land_Use>().id);
                Int32.TryParse(agent.GetAttributeValue("lu_code"), out obj.GetComponent<Land_Use>().lu_code);
                obj.GetComponent<Land_Use>().lu_name = agent.GetAttributeValue("lu_name");
                Int32.TryParse(agent.GetAttributeValue("population"), out obj.GetComponent<Land_Use>().population);

                obj.GetComponent<Land_Use>().dist_code = agent.GetAttributeValue("dist_code");
                float.TryParse(agent.GetAttributeValue("mean_alt"), out obj.GetComponent<Land_Use>().mean_alt);
                bool.TryParse(agent.GetAttributeValue("is_in_densification"), out obj.GetComponent<Land_Use>().is_in_densification);
                bool.TryParse(agent.GetAttributeValue("focus_on_me"), out obj.GetComponent<Land_Use>().focus_on_me);
                bool.TryParse(agent.GetAttributeValue("is_adapted_type"), out obj.GetComponent<Land_Use>().is_adapted_type);
                bool.TryParse(agent.GetAttributeValue("is_urban_type"), out obj.GetComponent<Land_Use>().is_urban_type);
                Int32.TryParse(agent.GetAttributeValue("expro_cost"), out obj.GetComponent<Land_Use>().expro_cost);


                break;
            case IUILittoSim.COASTAL_DEFENSE_ID: // Coastal_Defense

                obj.AddComponent<Coastal_Defense>();
                Int32.TryParse(agent.GetAttributeValue("coast_def_id"), out obj.GetComponent<Coastal_Defense>().coast_def_id);
                obj.GetComponent<Coastal_Defense>().type = agent.GetAttributeValue("type");
                obj.GetComponent<Coastal_Defense>().district_code = agent.GetAttributeValue("district_code");
                //obj.GetComponent<Coastal_Defense>().color = agent.getAttributeValue("color");
                float.TryParse(agent.GetAttributeValue("height"), out obj.GetComponent<Coastal_Defense>().height);
                bool.TryParse(agent.GetAttributeValue("ganivelle"), out obj.GetComponent<Coastal_Defense>().ganivelle);
                float.TryParse(agent.GetAttributeValue("alt"), out obj.GetComponent<Coastal_Defense>().alt);
                obj.GetComponent<Coastal_Defense>().status = agent.GetAttributeValue("status");
                Int32.TryParse(agent.GetAttributeValue("length_coast_def"), out obj.GetComponent<Coastal_Defense>().length_coast_def);
                break;
            case IUILittoSim.DISTRICT_ID: // District
                obj.AddComponent<District>();
                obj.GetComponent<District>().district_name = agent.GetAttributeValue("district_name");
                obj.GetComponent<District>().district_code = agent.GetAttributeValue("district_code");
                break;
            case IUILittoSim.PROTECTED_AREA_ID: // Protected_Area
                obj.AddComponent<Protected_Area>();
                break;
            case IUILittoSim.ROAD_ID: // Road
                obj.AddComponent<Road>();
                break;
            case IUILittoSim.FLOOD_RISK_AREA_ID: // Flood_Risk_Area
                obj.AddComponent<Flood_Risk_Area>();
                break;
            default:
                break;

        }
    }
}
