using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Zenject;

public class Reward : MonoBehaviour
{
    public bool CanOpenAds { get; set; } = true;

    [Inject] GameData gameData;
    [Inject] PauseManager pauseManager;

    [DllImport("__Internal")]
    private static extern void RunAdsExtern();

    private void Awake()
    {
        EventsHolder.getReward += RunAds;
    }

    public void RunAds()
    {
        if (CanOpenAds)
        {
            try
            {
                RunAdsExtern();
            }
            catch (System.Exception)
            {
                Debug.Log("Error ---------- Ads");
#if UNITY_EDITOR
                EventsHolder.StartDoubleDrop();
#endif
            }
        }
    }

    public void SetReward()
    {
        ResumeGame();
        EventsHolder.StartDoubleDrop();
    }

    public void OpenAds()
    {
        pauseManager.PauseGame();
    }

    public void ResumeGame()
    {
        pauseManager.ResumeGame();
    }
}
