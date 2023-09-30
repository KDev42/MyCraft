using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;
using DG.Tweening;
using Zenject;

public class TimerAds : MonoBehaviour
{
    [Range(0,180)]
    [SerializeField] float timer;
    [SerializeField] GameObject adsWarningPanel;
    [SerializeField] TMP_Text adsWarning;
    [SerializeField] string warning = "Показ рекламы через: ";

    [DllImport("__Internal")]
    private static extern void ShowAdsExtern();

    private Coroutine callerAds;
    [Inject] PauseManager pauseManager;

    private void Awake()
    {
        ShowAds();
    }

    private void OnEnable()
    {
        callerAds = StartCoroutine(CallAds());
    }

    private void OnDisable()
    {
        if (callerAds != null)
            StopCoroutine(callerAds);
    }

    public void ShowAds()
    {
        try
        {
            ShowAdsExtern();
            pauseManager.PauseGame();
        }
        catch (System.Exception) { Debug.Log("Error ---------- Ads"); }
    }

    IEnumerator CallAds()
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);
            ShowWarning();
        }
    }

    private void ShowWarning()
    {
        adsWarningPanel.SetActive(true);

        Tween tween = DOTween.To(setter: value =>
        {
            adsWarning.text = warning +"\n" + (int)value;
        }, 
        startValue: 4, endValue: 0, duration: 3)
            .SetEase(Ease.Linear);

        tween.OnComplete(()=> {
            adsWarningPanel.SetActive(false);
            ShowAds();
        });
    }
}
