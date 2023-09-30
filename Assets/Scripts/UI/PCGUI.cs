using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGUI : CommonGUI
{
    [SerializeField] GameObject tips;

    protected override void ActivateGui()
    {
        base.ActivateGui();
        tips.SetActive(true);
    }

    protected override void DeactivateGui()
    {
        base.DeactivateGui();
        tips.SetActive(false);
    }
}
