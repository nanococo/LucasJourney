using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGL : MonoBehaviour
{
    // Draws a red triangle using pixels as coordinates to paint on.
    [SerializeField] Material mat;

    void Start()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadPixelMatrix();
        GL.Color(Color.red);

        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 10 / 2, 0);
        GL.Vertex3(20 / 2, 20 / 2, 0);
        GL.End();

        GL.PopMatrix();
        Debug.Log("Here");
    }
}
