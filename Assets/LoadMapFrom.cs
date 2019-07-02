using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ummisco.gama.unity.files.ShapefileImporter;
using ummisco.gama.unity.utils;
using UnityEditor;
using UnityEngine;

public class LoadMapFrom : MonoBehaviour
{

    public Material myNewMaterial;
    public Triangulator2 triangulator;
    public Material material;


    public Vector2[] vertices2D;


    // Use this for initialization
    void Start()
    {
        Debug.Log("Lest's start the import operation");

        // Create Vector2 vertices
        vertices2D = new Vector2[] { new Vector2(0, 0), new Vector2(10, 5), new Vector2(10, 10) };

        triangulator = new Triangulator2(vertices2D);

        

        string fileName = "/Users/sklab/Desktop/TODELETE/zone_etude/zones241115.shp";
        ShapeFile shapeFile = new ShapeFile();

        shapeFile.ReadShapes(fileName, 2000000, 1, 2000000, 1);
        int i = 0;

        //foreach (ShapeFileRecord rec in shapeFile.MyRecords)
        for(int k=0; k<shapeFile.MyRecords.Count; k++)
        {
            ShapeFileRecord rec = shapeFile.MyRecords[k];
            GameObject poly;
            Debug.Log("---> The record number is : " + rec.RecordNumber);
            Debug.Log("---> The record Content length is : " + rec.ContentLength);
            Debug.Log("---> The record Shape type is : " + rec.ShapeType);
            Debug.Log("---> The record number of parts is : " + rec.NumberOfParts);
            Debug.Log("---> The record number of points is : " + rec.NumberOfPoints);
            Debug.Log("---> the record attributes are: " + rec.Attributes);
            Debug.Log("---> the record points list size is " + rec.Points.Count);


            Vector2[] listPoint = new Vector2[rec.Points.Count-1];
            
            string vert = "";
            //foreach (Vector2 v in rec.Points)
            for(int j=0; j < rec.Points.Count - 1; j++)
            {
               Vector2 v = rec.Points[j];
               Vector2 v2 = new Vector2(v.x - 371000, v.y - 6549000);
               listPoint[j] = v2;
               vert += v2;
            }

           //triangulator.setPoints(listPoint);

            Debug.Log("The record vertices are : " + vert);

            poly = new GameObject("Poly_" + i);

            
            poly.AddComponent(typeof(MeshRenderer));
            poly.AddComponent(typeof(MeshFilter));

           poly.GetComponent<MeshFilter>().mesh = CreateMesh(0, listPoint);
            poly.GetComponent<MeshFilter>().mesh.name = "CustomMesh";
            poly.GetComponent<Renderer>().material = myNewMaterial;
            poly.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
         
            i++;
        }


        // -------------------------


        Vector2[] vertices2D2 = new Vector2[] { new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0), new Vector2(4, 8), new Vector2(2, 6), new Vector2(0, 8) };

        triangulator = new Triangulator2(vertices2D);
        MeshFilter filter;

        GameObject ob;


        ob = new GameObject("AAAAAA");

        ob.AddComponent(typeof(MeshRenderer));
        filter = ob.AddComponent(typeof(MeshFilter)) as MeshFilter;
        ob.GetComponent<MeshFilter>().mesh.Clear();
        ob.GetComponent<MeshFilter>().mesh = CreateMesh(10, vertices2D);
        ob.GetComponent<Renderer>().material = myNewMaterial;

        // ------------------------


       vertices2D2 = new Vector2[] { new Vector2(0, 0), new Vector2(2, 2), new Vector2(4, 0), new Vector2(4, 8), new Vector2(2, 6), new Vector2(0, 8) };

        ob = new GameObject("AAAAAA22");

        ob.AddComponent(typeof(MeshRenderer));
        filter = ob.AddComponent(typeof(MeshFilter)) as MeshFilter;
        ob.GetComponent<MeshFilter>().mesh.Clear();
        ob.GetComponent<MeshFilter>().mesh = CreateMesh(10, vertices2D2);
        ob.GetComponent<Renderer>().material = myNewMaterial;




    }



    // Update is called once per frame
    void Update()
    {

    }


    public Mesh CreateMesh(int elevation, Vector2[] vect)
    {
        Mesh mesh = new Mesh();
        Triangulator2 tri = new Triangulator2(vect);
        tri.setAllPoints(tri.get2dVertices());
        mesh.vertices = tri.get3dVertices(elevation);
        //tri.setAllPoints(tri.get2dVertices());
        mesh.triangles = tri.get3DTriangulesFrom2D();

        // For Android Build
        //        Unwrapping.GenerateSecondaryUVSet(m);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        // For Android Build
        //        MeshUtility.Optimize(m);
        return mesh;
    }

}
