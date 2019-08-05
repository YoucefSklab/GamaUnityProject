using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ummisco.gama.unity.files.ShapefileImporter;
using ummisco.gama.unity.geometry;
using ummisco.gama.unity.utils;
using UnityEditor;
using UnityEngine;

public class LoadMapFromSHape: MonoBehaviour
{

    public Material myNewMaterial;
    public Triangulator triangulator;
    public Material material;


    public Vector2[] vertices2D;


    // Use this for initialization
    void Start()
    {
        Debug.Log("Lest's start the import operation");

        // Create Vector2 vertices
        vertices2D = new Vector2[] { new Vector2(0, 0), new Vector2(10, 5), new Vector2(10, 10) };

        triangulator = new Triangulator(vertices2D);

        string fileName = "/Users/sklab/Desktop/TODELETE/zone_etude/zones241115.shp";
        ShapeFile shapeFile = new ShapeFile();

        shapeFile.ReadShapes(fileName, 2000000, 1, 2000000, 1);
        
        

        int i = 0;

        foreach (ShapeFileRecord rec in shapeFile.MyRecords)
        {
            GameObject poly;
            Debug.Log("---> The record number is : " + rec.RecordNumber);
            Debug.Log("---> The record Content length is : " + rec.ContentLength);
            Debug.Log("---> The record Shape type is : " + rec.ShapeType);
            Debug.Log("---> The record number of parts is : " + rec.NumberOfParts);
            Debug.Log("---> The record number of points is : " + rec.NumberOfPoints);
            Debug.Log("---> the record attributes are: " + rec.Attributes);


            Vector2[] listPoint = new Vector2[rec.Points.Count];
            int j = 0;
            foreach(Vector2 v in rec.Points)
            {
                listPoint[j] = v;
                j++;
            }
            triangulator.setPoints(listPoint);

            poly = new GameObject("Poly_" + i);

            poly.AddComponent(typeof(MeshRenderer));
            poly.AddComponent(typeof(MeshFilter));
            CreateMesh(40, poly.GetComponent<MeshFilter>().mesh);
            poly.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
            poly.GetComponent<Renderer>().material = myNewMaterial;
            //poly.transform.localScale = new Vector3(45f, 45f, 45f);

            i++;
        }

        


    }



    // Update is called once per frame
    void Update()
    {

    }
    

    public void CreateMesh(int elevation, Mesh m)
    {
        m.vertices = triangulator.VerticesWithElevation(elevation);
        triangulator.setAllPoints(triangulator.Convert2dTo3dVertices());
        m.triangles = triangulator.Triangulate3dMesh();

        // For Android Build
        //        Unwrapping.GenerateSecondaryUVSet(m);

        m.RecalculateNormals();
        m.RecalculateBounds();
        // For Android Build
        //        MeshUtility.Optimize(m);

    }

}
