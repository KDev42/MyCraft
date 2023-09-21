using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFactory : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Texture2D blocksTexture;
    [SerializeField] int tileSize =16;
    [SerializeField] int particleTextureSize =4;

    private Dictionary<PoolObject, Pool> itemsPool = new Dictionary<PoolObject, Pool>();

    public void SpawnParticles(PoolObject brokeBlockParticle, Vector3 spawnPosition, Quaternion spawnRotation, BlockInfo blockInfo)
    {
        GameObject particle = GetObject(brokeBlockParticle);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        particle.transform.position = spawnPosition;
        particle.transform.rotation = spawnRotation;

        Vector2 textureOffset = blockInfo.GetPixelsOffset(Vector3Int.up)/ tileSize;
        SetTexture((int)textureOffset.x, (int)textureOffset.y, particleSystem.GetComponent<ParticleSystemRenderer>());

        particle.SetActive(true);
        particleSystem.Play();
    }

    private GameObject GetObject(PoolObject objectPrefab)
    {
        if (!itemsPool.ContainsKey(objectPrefab))
        {
            itemsPool.Add(objectPrefab, new Pool(objectPrefab, 4, container, false));
        }

        return itemsPool[objectPrefab].GetFreeElement(false, false).gameObject;
    }

    private void SetTexture(int x, int y, ParticleSystemRenderer particleSystem)
    {
        Texture2D tileTexture = new Texture2D(particleTextureSize, particleTextureSize);
        Color[] colorArray = blocksTexture.GetColorArray(x, y, tileSize, particleTextureSize);

        tileTexture.GetTileTexture(colorArray, particleTextureSize);

        particleSystem.material.mainTexture = tileTexture;
    }
}
