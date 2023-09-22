using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWorld : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;

    public void StartGeneration()
    {
        PlayerData.Initialize();
        worldGenerator.GenerateMap();
    }
}
