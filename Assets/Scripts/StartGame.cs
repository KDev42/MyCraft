using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;

    private void Awake()
    {
        PlayerData.Initialize();
        worldGenerator.GenerateMap();
    }
}
