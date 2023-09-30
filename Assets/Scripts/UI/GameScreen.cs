using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour
{
    [SerializeField] GameObject Settings;

    private void Awake()
    {
        EventsHolder.openSettings += OpenSettings;
    }

    private void OpenSettings()
    {
        Settings.SetActive(true);
    }
}
