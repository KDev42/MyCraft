using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider sensSlider;
    [SerializeField] Button closeBtn;
    [SerializeField] Button applyBtn;

    private GameData gameData;
    private SaveLoad saveLoad;

    [Inject]
    private void Construct (GameData gameData, SaveLoad saveLoad)
    {
        this.gameData = gameData;
        this.saveLoad = saveLoad;
    }

    private void Awake()
    {
        closeBtn.onClick.AddListener(DropSettings);
        applyBtn.onClick.AddListener(ApplySettings);
    }

    private void OnEnable()
    {
        soundSlider.value = gameData.gameSettings.soundValue;
        sensSlider.value = gameData.gameSettings.sens;

        soundSlider.onValueChanged.AddListener(ChangeSoundValue);
    }

    private void OnDisable()
    {
        soundSlider.onValueChanged.RemoveAllListeners();
    }

    private void ApplySettings()
    {
        gameData.gameSettings.soundValue = soundSlider.value;
        gameData.gameSettings.sens = sensSlider.value;
        saveLoad.SavePlayerSettings();
        Close();
    }

    private void DropSettings()
    {
        ChangeSoundValue(gameData.gameSettings.soundValue);
        Close();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void ChangeSoundValue(float soundValue)
    {
        AudioSettings.SFXVolumeLevel(soundValue);
    }
}
