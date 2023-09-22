using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Button settingButton;
    [SerializeField] Button newGameBtn;
    [SerializeField] GameObject settingPanel;
    [SerializeField] LoadingWorld loadingWorld;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject lobbyUI;
    [SerializeField] InputController inputController;

    private void Awake()
    {
        settingButton.onClick.AddListener(OpenSetting);
        newGameBtn.onClick.AddListener(NewGame);
    }

    private void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    private void NewGame()
    {
        loadingWorld.StartGeneration();
        CloseStartScreen();
    }

    private void CloseStartScreen()
    {
        inputController.StartGame();
        startScreen.SetActive(false);
        lobbyUI.SetActive(false);
    }

    private void OpenGameScreen()
    {

    }
}
