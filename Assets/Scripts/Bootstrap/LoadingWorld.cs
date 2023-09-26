using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoadingWorld : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] StartScreen startScreen;
    [SerializeField] GameScreen gameScreen;
    [SerializeField] GameObject player;

    private SaveLoad saveLoad;
    private GameData gameData;

    [Inject]
    private void Construct(SaveLoad saveLoad, GameData gameData)
    {
        this.saveLoad = saveLoad;
        this.gameData = gameData;
    }

    public void StartGeneration()
    {
        WorldSettings worldSettings = saveLoad.LoadWorldSettings();

        Debug.Log(JsonUtility.ToJson(worldSettings));

        PlayerData.Initialize(worldSettings.handInventory, worldSettings.mainInventory);
        worldGenerator.GenerateMap(CompledeGeneration, StartCoordinate(worldSettings));
    }

    private void CompledeGeneration()
    {
        startScreen.CloseStartScreen();
        gameScreen.OpenGameScreen();
    }

    private Vector3 StartCoordinate(WorldSettings worldSettings)
    {
        if (worldSettings.playerPosition == new Vector3(0,0,0))
        {
            float coord = (WorldConstants.maxSizeWorld* WorldConstants.chunkWidth)/2 + 0.5f ;

            return new Vector3(coord, 50, coord);
        }
        else
        {
            return worldSettings.playerPosition;
        }
    }
}
