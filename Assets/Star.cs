using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour {
	public Vector3 point = Vector3.up;
	public int numberOfPoints = 10;
	
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	private int currentNumberOfPoints = 0;
	private Vector3 currentPoint = Vector3.up;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Star Mesh";
		
	}
	
	// Update is called once per frame
	void Update () {
		if(currentNumberOfPoints == numberOfPoints && point == currentPoint) {
			return;
		}
	
		if(numberOfPoints < 3) {
			numberOfPoints = 3;
		}
		
		Mesh newMesh = new Mesh();
		vertices = new Vector3[numberOfPoints + 1];
		triangles = new int[numberOfPoints * 3];		
		float angle = -360f / numberOfPoints;
		for(int v = 1, t = 1; v < vertices.Length; v++, t += 3){
			vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * point;
			triangles[t] = v;
			triangles[t + 1] = v + 1;
		}
		triangles[triangles.Length - 1] = 1;

		newMesh.vertices = vertices;
		newMesh.triangles = triangles;
		
		currentNumberOfPoints = numberOfPoints;
		currentPoint = point;
		
		GetComponent<MeshFilter>().mesh = mesh = newMesh;			
		mesh.name = "Star Mesh - " + point.ToString() + numberOfPoints.ToString();
	}
}