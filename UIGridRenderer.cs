using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[ExecuteInEditMode]
public class UIGridRenderer : Graphic
{

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        UIVertex vertext = UIVertex.simpleVert;
        vertext.color = color;
        vh.AddVert(vertext);

        vertext.position = new Vector3(0, height);
        vh.AddVert(vertext);

        vertext.position = new Vector3(width, height);
        vh.AddVert(vertext);

        vertext.position = new Vector3(width, 0);
        vh.AddVert(vertext);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);


    }

}
