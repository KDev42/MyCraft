using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Texturing 
{
    public static void GetTileTexture(this Texture2D tileTexture, Color[] colorArray,int textureSize)
    {
        tileTexture.SetPixels(0, 0, textureSize, textureSize, colorArray);

        tileTexture.filterMode = FilterMode.Point;
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.Apply();
    }

    public static Color[] GetColorArray(this Texture2D texture , int x, int y, int tileSize, int textureSize)
    {
        return texture.GetPixels(tileSize * x, tileSize * y, textureSize, textureSize);
    }
}
