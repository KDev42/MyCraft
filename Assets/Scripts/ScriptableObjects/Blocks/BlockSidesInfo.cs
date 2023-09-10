using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Block Sides Info")]
public class BlockSidesInfo : BlockInfo
{
    public Vector2 textureIndexUp;
    public Vector2 textureIndexDown;

    public override Vector2 GetPixelsOffset(Vector3Int normal)
    {
        if(normal == Vector3Int.up)
        {
            return textureIndexUp*textureSize;
        }    
        else if(normal == Vector3Int.down)
        {
            return textureIndexDown * textureSize;
        }
        else
        {
            return base.GetPixelsOffset(normal) ;
        }
    }
}
