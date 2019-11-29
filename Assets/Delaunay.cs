using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ummisco.gama.unity.geometry;
using ummisco.gama.unity.geometry.datastructure;
using ummisco.gama.unity.geometry.triangulation;
using UnityEngine;

public class Delaunay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        TriangulateWithDelaunay();
    }

    public void TriangulateWithDelaunay()
    {
        List<Vector3> sites = new List<Vector3> { new Vector3(1, 2, 0), new Vector3(2, 4, 0), new Vector3(10, 10, 0), new Vector3(6, 1, 0) };

        sites = new List<Vector3> { new Vector3(1.0f, 2.0f, 0.0f), new Vector3(1.0f, 4.0f, 0.0f), new Vector3(7.0f, 4.0f, 0.0f), new Vector3(7.0f, 1.0f, 0.0f), new Vector3(5.0f, 2.0f, 0.0f), new Vector3(6.0f, 3.0f, 0.0f), new Vector3(2.0f, 3.0f, 0.0f), new Vector3(3.0f, 2.0f, 0.0f), new Vector3(2.0f, 1.0f, 0.0f) };

        List<Triangle> TList = new List<Triangle>(); 

        List<Vertex> sitesVertex = new List<Vertex>();

        foreach (Vector3 v in sites)
        {
            Vertex vrt = new Vertex(v);
            sitesVertex.Add(vrt);
        }


        //TList = Triangulation.TriangulateConvexPolygon(sitesVertex);
        //TList = Triangulation.TriangulateConcavePolygon(sites);
        //TList = DelaunayTriangulation.TriangulateByFlippingEdges(sites);

        Debug.Log("Total triangles is: " +TList.Count);


        int [] Tri = new int[TList.Count * 3];

        int cmp = 0;
        int tCmp = 1;
        foreach (Triangle t in TList)
        {
            Vertex v1 = t.v1;
            Vertex v2 = t.v2;
            Vertex v3 = t.v3;
            Tri[cmp] = sites.IndexOf(v1.position);
            cmp ++;
            Tri[cmp] = sites.IndexOf(v2.position);
            cmp ++;
            Tri[cmp] = sites.IndexOf(v3.position);
            cmp++;
            Debug.Log("Triangle Number" +tCmp+ " is:   \n v1 is: " + v1.position + " v2 is: " + v2.position + " v3 is: " + v3.position);
           
            tCmp ++;
        }


        cmp = 0;
        foreach (int nbr in Tri)
        {
            Debug.Log("vertice at position "+ cmp + " is " + nbr);
            cmp++;
        }


       

        Mesh mesh = new Mesh
        {
            vertices = sites.ToArray(),
            triangles = Tri
        };


        MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
        MeshCollider meshCollider = (MeshCollider)gameObject.AddComponent<MeshCollider>();


        meshFilter.mesh = mesh;
        meshFilter.mesh.name = "CustomMesh";

        if (1 == 1)
        {
            Vector2[] tab = new Vector2[sites.Count];
            int r = 0;
            foreach(Vector3 v in sites)
            {
                tab[r] = sites[r];
                r++;
            }

      
            mesh.Clear();

            Triangulator triang = new Triangulator(tab);
            triang.SetAllPoints(triang.Convert2dTo3dVertices());
            mesh.vertices = triang.VerticesWithElevation(10);
            mesh.triangles = triang.Triangulate3dMesh2(mesh.vertices);

            //mesh.RecalculateNormals();


            //mesh = triang.ClockwiseMesh(mesh);

            meshFilter.mesh = mesh;
            meshFilter.mesh.name = "CustomMesh2";

        }

        gameObject.transform.localScale = new Vector3(300, 300, 300);
        Material mat = new Material(Shader.Find("Specular"));
        mat.color = Color.blue;
       
        meshRenderer.material = mat;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
