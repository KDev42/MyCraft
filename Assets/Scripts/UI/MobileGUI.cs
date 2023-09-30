using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileGUI : CommonGUI
{
    [SerializeField] GameObject leftButtons;
    [SerializeField] GameObject rightButtons;

    protected override void ActivateGui()
    {
        base.ActivateGui();
        leftButtons.SetActive(true);
        rightButtons.SetActive(true);
    }

    protected override void DeactivateGui()
    {
        base.DeactivateGui();
        leftButtons.SetActive(false);
        rightButtons.SetActive(false);
    }
}
