using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.GamaAgent;
using ummisco.gama.unity.littosim;
using ummisco.gama.unity.utils;
using UnityEngine;

public class AgentCreator : MonoBehaviour
{
    private MeshCreator meshCreator = new MeshCreator();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAgent(Agent agent, Transform parentTransform, Material mat, int speciesId)
    {
        GameObject newObject = new GameObject(agent.agentName);
        newObject.AddComponent(typeof(MeshRenderer));
        newObject.AddComponent(typeof(MeshFilter));
        newObject.AddComponent<MeshCollider>();

        newObject.GetComponent<Transform>().SetParent(parentTransform);
        agent.height = 30;

        newObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(agent.height, agent.agentCoordinate.getVector2Coordinates());
        //newObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(agent.height, agent.ConvertVertices());

        newObject.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
        //newGameObject.GetComponent<MeshFilter>().mesh = meshCreator.CreateMesh(30, agent.ConvertVertices());
        //mat.color = agent.color.getColorFromGamaColor();
        newObject.GetComponent<Renderer>().material = mat;

        Vector3 posi = agent.location;
        posi.y = -posi.y;

        //posi = uiManager.GetComponent<UIManager>().worldToUISpace(canvas, posi);
        

        RectTransform rt = (newObject.AddComponent<RectTransform>()).GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);

        //newObject.GetComponent<Transform>().localPosition = posi;
        newObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);//posi;

        AttacheCode(newObject, speciesId, agent);
    }

    public void AttacheCode(GameObject obj, int speciesId, Agent agent)
    {
        switch (speciesId)
        {
            case IUILittoSim.LAND_USE_ID: // Land_Use
                obj.AddComponent<Land_Use>();
                obj.GetComponent<Land_Use>().id = 1;
                obj.GetComponent<Land_Use>().lu_name = agent.agentName+"_"+1;
                obj.GetComponent<Land_Use>().lu_code = 1;
                obj.GetComponent<Land_Use>().dist_code = "dist_code_"+1;
                obj.GetComponent<Land_Use>().population = 1;
                obj.GetComponent<Land_Use>().mean_alt = 1;
                break;
            case IUILittoSim.COASTAL_DEFENSE_ID: // Coastal_Defense
                obj.AddComponent<Coastal_Defense>();
                obj.GetComponent<Coastal_Defense>().type = "Type";
                obj.GetComponent<Coastal_Defense>().district_code = agent.agentName+"_code_"+2;
                obj.GetComponent<Coastal_Defense>().status = "status";
                break;
            case IUILittoSim.DISTRICT_ID: // District
                obj.AddComponent<District>();
                obj.GetComponent<District>().district_name = agent.agentName + "_name";
                obj.GetComponent<District>().district_code = agent.agentName +"_code";               
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
