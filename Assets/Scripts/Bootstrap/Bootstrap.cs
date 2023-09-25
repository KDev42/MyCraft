using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] StartScreen startScreen;

    private SaveLoad saveLoad;
    private GameData gameData;

    [Inject]
    private void Construct(SaveLoad saveLoad, GameData gameData)
    {
        this.saveLoad = saveLoad;
        this.gameData = gameData;
    }

    private void Awake()
    {
        SetSettings();
    }

    private void SetSettings()
    {
        saveLoad.LoadWorldSettings();
        GameSettings gameSettings = saveLoad.LoadPlayerSettings();
        gameData.gameSettings = gameSettings;
        AudioSettings.Initialize();
        AudioSettings.SFXVolumeLevel(gameSettings.soundValue);

        startScreen.Activation();
    }
}
