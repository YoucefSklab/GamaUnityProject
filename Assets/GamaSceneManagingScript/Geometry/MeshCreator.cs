using System;
using ummisco.gama.unity.utils;
using UnityEngine;

namespace ummisco.gama.unity.geometry
{
    public class MeshCreator
    {
        public MeshCreator()
        {

        }


        public Mesh CreateMesh(float elevation, Vector2[] vect, Vector3 shifMesh)
        {
            Mesh mesh = new Mesh();
            Triangulator tri = new Triangulator(vect);
            tri.setAllPoints(tri.Convert2dTo3dVertices());
            mesh.vertices = tri.VerticesWithElevation(elevation, shifMesh);
            mesh.triangles = tri.Triangulate3dMesh();
    

            // For Android Build
            // Unwrapping.GenerateSecondaryUVSet(m);

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            // For Android Build
            // MeshUtility.Optimize(m);
            return mesh;
        }

        public Mesh CreateMesh(float elevation, Vector2[] vect)
        {
             return CreateMesh(elevation, vect, new Vector3(0,0,0));
        }


        public Mesh CreateMesh2(float elevation, Vector2[] vect, Vector3 shifMesh)
        {
            Mesh mesh = new Mesh();
            Triangulator tri = new Triangulator(vect);
            tri.setAllPoints(tri.Convert2dTo3dVertices());
            mesh.vertices = tri.VerticesWithElevation(elevation, shifMesh);
            mesh.triangles = tri.Triangulate3dMesh2();

            // For Android Build
            // Unwrapping.GenerateSecondaryUVSet(m);

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            // For Android Build
            // MeshUtility.Optimize(m);
            return mesh;
        }

        public Mesh CreateMesh2(float elevation, Vector2[] vect)
        {
          return CreateMesh2(elevation, vect, new Vector3(0,0,0));
        }
    }
}
