using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cracks : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    [SerializeField] int tileSize;

    private int maxDamage = 8;

    public void ChangeTexture(float miningTime, float maxTime)
    {
        int tileIndex = (int)(miningTime/(maxTime / maxDamage));
        tileIndex = tileIndex < maxDamage ? tileIndex : maxDamage-1;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        Texture2D tileTexture = new Texture2D(tileSize, tileSize);
        Color[] colorArray = texture.GetColorArray(tileIndex, 0, tileSize, tileSize);

        tileTexture.GetTileTexture( colorArray, tileSize);

        meshRenderer.material.mainTexture = tileTexture;
    }
}
