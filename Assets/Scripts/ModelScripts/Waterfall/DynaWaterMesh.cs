using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynaWaterMesh : MonoBehaviour {

    // Use this for initialization
    Vector3[] vertPositions;
    Mesh mesh;
    float[] modifierX;
    float[] modifierY;
    bool[] up;
    bool[] uptwo;


    void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        vertPositions = mesh.vertices;
       
    }


    void Update()
    {

        Vector3[] vertices = mesh.vertices;

        for (int i=0; i<mesh.vertices.Length; i++)
        {
            float rand = Random.Range(9900f,11000f);
            //float randY = Random.Range(4500f, 5500f);

            vertices[i].z = vertPositions[i].z - ((Mathf.Sin(Time.time * 10f + vertPositions[i].z))/rand);
            vertices[i].z += (i/ 20000f);

            vertices[i].y = vertPositions[i].y - ((Mathf.Sin(Time.time * 20f + (vertPositions[i].y * 10000f)))/rand);
            //vertices[i].y += (i / 20000f);
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

}
