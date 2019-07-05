using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ummisco.gama.unity.files.ShapefileImporter;
using ummisco.gama.unity.utils;
using UnityEditor;
using UnityEngine;

public class LoadMapFrom : MonoBehaviour
{

    public Material uaMaterial;
    public Material commeunesMaterial;
    public Material defCoteMaterial;
    public Triangulator triangulator;
    public Material material;

    public string parentName = "Ground";
    public Vector2[] vertices2D;


    // Use this for initialization
    void Start()
    {
        Debug.Log("Lest's start the import operation");

        // Create Vector2 vertices
        vertices2D = new Vector2[] { new Vector2(0, 0), new Vector2(10, 5), new Vector2(10, 10) };

        triangulator = new Triangulator(vertices2D);
        
        string uaFileName = "/Users/sklab/Desktop/TODELETE/zone_etude/zones241115.shp";
        string communesFileName = "/Users/sklab/Desktop/TODELETE/zone_etude/communes.shp";
        string defCoteFileName = "/Users/sklab/Desktop/TODELETE/zone_etude/defense_cote_littoSIM-05122015.shp";

        //loadShape(communesFileName, "Communes", "Commune",commeunesMaterial, 10);
        loadShape(uaFileName, "UA", "UA", uaMaterial, 30);
        //loadShape(defCoteFileName, "DefCote", "DefCote", defCoteMaterial, 50);

    }



    // Update is called once per frame
    void Update()
    {

    }


    public void loadShape(string fileName, string parentName, string prefix, Material mat, int elevation)
    {
        GameObject parent = GameObject.Find(parentName);
        ShapeFile shapeFile = new ShapeFile();

        shapeFile.ReadShapes(fileName, 2000000, 1, 2000000, 1);
        int i = 0;

        //foreach (ShapeFileRecord rec in shapeFile.MyRecords)
        for (int k = 0; k < shapeFile.MyRecords.Count; k++)
        {
            ShapeFileRecord rec = shapeFile.MyRecords[k];
            GameObject newGameObject;
           
            Vector2[] listPoint = new Vector2[rec.Points.Count - 1];

            string vert = "";

            for (int j = 0; j < rec.Points.Count - 1; j++)
            {
                Vector2 v = rec.Points[j];
                Vector2 v2 = new Vector2(v.x - 371000, v.y - 6549000);
                listPoint[j] = v2;
                vert += v2;
            }

            //triangulator.setPoints(listPoint);

            Debug.Log("The record vertices are : " + vert);

            newGameObject = new GameObject(prefix+"_" + i);
            
            newGameObject.AddComponent(typeof(MeshRenderer));
            newGameObject.AddComponent(typeof(MeshFilter));

            newGameObject.GetComponent<MeshFilter>().mesh = CreateMesh(elevation, listPoint);
            newGameObject.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
            newGameObject.GetComponent<Renderer>().material = mat;
            newGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            newGameObject.GetComponent<Transform>().SetParent(parent.GetComponent<Transform>());
            newGameObject.AddComponent<UA>();
            newGameObject.AddComponent<MeshCollider>();

            newGameObject.GetComponent<UA>().ua_name = prefix + "_" + i;
            newGameObject.GetComponent<UA>().ua_code = i;
            newGameObject.GetComponent<UA>().population = i;
            newGameObject.GetComponent<UA>().cout_expro = i;
            newGameObject.GetComponent<UA>().fullNameOfUAname = prefix + "_FULLNAME_" + i;
            newGameObject.GetComponent<UA>().classe_densite = prefix + "_CLASSE_DENSITE_" + i;

            

            i++;
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
