using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class VertScroll : MonoBehaviour {

    // Use this for initialization
    Mesh mesh;

    [Range( 0, 5)]
    public float scrollSpeed;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        /*
        if (mesh.name.Equals("plane.001"))
            scrollSpeed = 1f;
        else
            scrollSpeed = 0.5f;
        */
    }

    // Update is called once per frame
    void Update () {

        //mesh.uv[0] += new Vector2(0,1*Time.deltaTime) ;
        SwapUVs();
        //Debug.Log(mesh.name);
	}

    void SwapUVs()
    {
        Vector2[] uvSwap = mesh.uv;

        for (int b= 0; b < uvSwap.Length; b++)
     {
            uvSwap[b] += new Vector2(0, scrollSpeed * Time.deltaTime);
        }

        mesh.uv = uvSwap;
    }
}
