using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Button settingButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button newGameBtn;
    [SerializeField] GameObject settingPanel;
    [SerializeField] LoadingWorld loadingWorld;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject lobbyUI;
    //[SerializeField] InputController inputController;
    [SerializeField] GameObject buttonsContainer;
    [SerializeField] GameObject loaderContainer;
    [SerializeField] GameObject warningPanel;

    private SaveLoad saveLoad;

    [Inject]
    private void Construct(SaveLoad saveLoad, GameData gameData)
    {
        this.saveLoad = saveLoad;
    }

    public void Activation()
    {
        settingButton.onClick.AddListener(OpenSetting);
        newGameBtn.onClick.AddListener(CheckNewGame);
        CheckSave();
    }

    public void CloseStartScreen()
    {
        //inputController.StartGame();
        startScreen.SetActive(false);
        lobbyUI.SetActive(false);
    }

    public void CloseWarning()
    {
        warningPanel.SetActive(false);
    }

    public void NewGame()
    {
        AudioSettings.Initialize();
        CloseWarning();
        saveLoad.ClearWorld();
        loadingWorld.StartGeneration();
        Loadin();
    }

    private void OpenSetting()
    {
        AudioSettings.Initialize();
        settingPanel.SetActive(true);
    }

    private void CheckSave()
    {
        if (PlayerPrefs.HasKey("WorldSettings"))
        {
            continueButton.onClick.AddListener(ContinueGame);
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    private void ContinueGame()
    {
        AudioSettings.Initialize();
        loadingWorld.StartGeneration();
        Loadin();
    }

    private void CheckNewGame()
    {
        if (PlayerPrefs.HasKey("WorldSettings"))
        {
            warningPanel.SetActive(true);
        }
        else
        {
            NewGame();
        }
    }

    private void Loadin()
    {
        buttonsContainer.SetActive(false);
        loaderContainer.SetActive(true);
    }
}
