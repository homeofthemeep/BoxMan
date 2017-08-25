using UnityEngine;
using System.Collections;

public class DynamicWater : MonoBehaviour
{
    Vector3[] baseHeight;
    Vector3[] waterVertices;// = new Vector3[121];
    [Range (0,10)]
    public float waveSpeed = 0.5f;
    [Range(0, 10)]
    public float waveScale = 0.25f;

    private float waveFreq = 6.9115f * 2f;
    float random;
    private Mesh mesh;
    private Mesh baseMesh;
    private float time;

    void Start()
    {
        random = Random.Range(0f, 1f);
        mesh = GetComponent<MeshFilter>().mesh;
        waterVertices= new Vector3[mesh.vertices.Length];
    }

    void Update()
    {
        if (baseHeight == null)
            baseHeight = mesh.vertices;

        for (int i = 0; i < waterVertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            float perlinRandom;
            vertex.y += Mathf.Sin(Time.time * waveSpeed
                + ((baseHeight[i].x) * waveFreq)) * waveScale
                + Mathf.Sin(Time.time * waveSpeed
                + (baseHeight[i].z * waveFreq)) / 2 * waveScale;
            if (vertex.x != -5f && vertex.x != 5f && vertex.z != 5f && vertex.z != -5f)
                perlinRandom = random;
            else
                perlinRandom = 0;
            vertex.y += Mathf.PerlinNoise(baseHeight[i].z, baseHeight[i].y
            + perlinRandom + Mathf.Sin(Time.time));
            waterVertices[i] = vertex;
        }
        mesh.vertices = waterVertices;
        mesh.RecalculateNormals();
    }
}