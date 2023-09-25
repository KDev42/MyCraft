using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject tips;
    [SerializeField] GameObject Settings;

    private void Awake()
    {
        EventsHolder.openSettings += OpenSettings;
    }

    public void OpenGameScreen()
    {
        gameScreen.SetActive(true);
        tips.SetActive(true);
    }

    private void OpenSettings()
    {
        Settings.SetActive(true);
    }
}
