using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardButton : MonoBehaviour
{
    [SerializeField] GameObject waitStateObject;
    [SerializeField] GameObject activeStateObject;
    [SerializeField] Image cooldownImage;
    [Range(10,240)]
    [SerializeField] int timerDoubleDrop;
    [SerializeField] Reward reward;

    private void Awake()
    {
        EventsHolder.startDoubleDrop += ActiveState;
    }

    private void ActiveState()
    {
        reward.CanOpenAds = false;
        waitStateObject.SetActive(false);
        activeStateObject.SetActive(true);

        Tween tweenCooldown = DOTween.To(setter: value =>
        {
            cooldownImage.fillAmount = value;
        }, startValue: 1, endValue: 0, duration: timerDoubleDrop)
            .SetEase(Ease.Linear);

        tweenCooldown.OnComplete(() => {
            EventsHolder.StopDoubleDrop();
            WaitState();
        });
    }

    private void WaitState()
    {
        reward.CanOpenAds = true;
        waitStateObject.SetActive(true);
        activeStateObject.SetActive(false);
    }
}
