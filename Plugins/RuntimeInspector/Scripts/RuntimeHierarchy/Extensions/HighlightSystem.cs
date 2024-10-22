using HighlightPlus;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSystem : MonoBehaviour
{
    public Material highLightMaterial;
    Bounds targetBounds;
    bool isHighlighting = false;

    // HighLightPlus asset
    public HighlightManager hm;

    //private void Awake()
    //{
    //    if (!highLightMaterial)
    //    {
    //        // Unity has a built-in shader that is useful for drawing
    //        // simple colored things.
    //        var shader = Shader.Find("Hidden/Internal-Colored");
    //        highLightMaterial = new Material(shader);
    //        highLightMaterial.hideFlags = HideFlags.HideAndDontSave;
    //        // Turn on alpha blending
    //        highLightMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //        highLightMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //        // Turn backface culling off
    //        highLightMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
    //        // Turn off depth writes
    //        highLightMaterial.SetInt("_ZWrite", 0);
    //        // z test off
    //        highLightMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    //    }
    //}

    public void Highlight(List<Transform> targets)
    {
        if (targets == null) return;

        hm.SelectObject(targets[0]);

        /*
        // get all bounds
        Bounds bounds = new Bounds();
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null) continue;

            Renderer[] renderers = targets[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < renderers.Length; j++)
            {
                if (renderers[j] == null) continue;

                if (bounds.size == Vector3.zero)
                {
                    bounds = renderers[j].bounds;

                }
                else
                {
                    bounds.Encapsulate(renderers[j].bounds);
                }
            }
        }

        //Debug.Log("bounds: "+bounds);

        if (bounds.size == Vector3.zero) bounds = new Bounds(Vector3.zero, Vector3.one*2);

        targetBounds = bounds;
        */
        isHighlighting = true;
    } // Highlight

    //private void OnRenderObject()
    //{
    //    if (isHighlighting)
    //    {
    //        DrawBoundsGL(targetBounds);
    //    }
    //}

    void DrawBoundsGL(Bounds bounds)
    {
        highLightMaterial.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(Color.red);

        Vector3 p1 = bounds.min;
        Vector3 p2 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        Vector3 p3 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        Vector3 p4 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        Vector3 p5 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        Vector3 p6 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        Vector3 p7 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        Vector3 p8 = bounds.max;

        // gl lines
        GL.Vertex(p1); GL.Vertex(p2);
        GL.Vertex(p1); GL.Vertex(p3);
        GL.Vertex(p1); GL.Vertex(p5);
        GL.Vertex(p2); GL.Vertex(p4);
        GL.Vertex(p2); GL.Vertex(p6);
        GL.Vertex(p3); GL.Vertex(p4);
        GL.Vertex(p3); GL.Vertex(p7);
        GL.Vertex(p4); GL.Vertex(p8);
        GL.Vertex(p5); GL.Vertex(p6);
        GL.Vertex(p5); GL.Vertex(p7);
        GL.Vertex(p6); GL.Vertex(p8);
        GL.Vertex(p7); GL.Vertex(p8);

        GL.End();
        GL.PopMatrix();
    }

}
