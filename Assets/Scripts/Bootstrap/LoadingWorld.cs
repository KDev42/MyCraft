using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWorld : MonoBehaviour
{
    [SerializeField] WorldGenerator worldGenerator;
    [SerializeField] StartScreen startScreen;
    [SerializeField] GameScreen gameScreen;

    public void StartGeneration()
    {
        PlayerData.Initialize();
        worldGenerator.GenerateMap(CompledeGeneration);
    }

    private void CompledeGeneration()
    {
        startScreen.CloseStartScreen();
        gameScreen.OpenGameScreen();
    }
}
