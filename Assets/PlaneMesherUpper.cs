using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PlaneMesherUpper : MonoBehaviour {
    public float m_Width = 1;
    public float m_Length = 1;


	void Start () {
        MeshBuilder meshBuilder = new MeshBuilder();
        
        //Set up the vertices and triangles:
        meshBuilder.Vertices.Add(new Vector3(-m_Width / 2f, 0.0f, -m_Length / 2f));
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);
        
        meshBuilder.Vertices.Add(new Vector3(-m_Width / 2f, 0.0f, m_Length / 2f));
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);
        
        meshBuilder.Vertices.Add(new Vector3(m_Width / 2f, 0.0f, m_Length / 2f));
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Vector3.up);
        
        meshBuilder.Vertices.Add(new Vector3(m_Width / 2f, 0.0f, -m_Length / 2f));
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(Vector3.up);
        
        meshBuilder.AddTriangle(0, 1, 2);
        meshBuilder.AddTriangle(0, 2, 3);

        //Create the mesh:
        Mesh myMesh = meshBuilder.CreateMesh();
        myMesh.name = "JoshMesh";

        this.GetComponent<MeshFilter>().sharedMesh = myMesh;
      this.GetComponent<MeshCollider>().sharedMesh = myMesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public class MeshBuilder
{
 private List<Vector3> m_Vertices = new List<Vector3>();
 public List<Vector3> Vertices { get { return m_Vertices; } }

 private List<Vector3> m_Normals = new List<Vector3>();
 public List<Vector3> Normals { get { return m_Normals; } }

 private List<Vector2> m_UVs = new List<Vector2>();
 public List<Vector2> UVs { get { return m_UVs; } }

 private List<int> m_Indices = new List<int>();

 public void AddTriangle(int index0, int index1, int index2)
 {
     m_Indices.Add(index0);
     m_Indices.Add(index1);
     m_Indices.Add(index2);
 }

 public Mesh CreateMesh()
 {
     Mesh mesh = new Mesh();

     mesh.vertices = m_Vertices.ToArray();
     mesh.triangles = m_Indices.ToArray();

     //Normals are optional. Only use them if we have the correct amount:
     if (m_Normals.Count == m_Vertices.Count)
         mesh.normals = m_Normals.ToArray();

     //UVs are optional. Only use them if we have the correct amount:
     if (m_UVs.Count == m_Vertices.Count)
         mesh.uv = m_UVs.ToArray();

     mesh.RecalculateBounds();

     return mesh;
 }
}
