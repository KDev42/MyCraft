using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemShape : ItemObject
{
    private Material material;


    public void ChangeSkin(ItemTool itemTool)
    {
        if(material == null)
        {
            GetMaterial();
        }
        Vector4 offsets = new Vector4();
        offsets.x = itemTool.baseOffset.x;
        offsets.y = itemTool.baseOffset.y;
        offsets.z = itemTool.offset.x;
        offsets.w = itemTool.offset.y;
        material.SetVector("_Offsets", offsets);

    }

    private void GetMaterial()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }
}
