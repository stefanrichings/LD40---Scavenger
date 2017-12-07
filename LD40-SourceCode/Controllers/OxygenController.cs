using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenController : UnitySingleton<OxygenController>
{
    public float oxygenLevels = 100f;

    float depletionAmount;

    protected override void Awake ()
    {
        base.Awake();

        GameController.OnCoreChange += (bool val) =>
        {
            if (val)
            {
                ReduceEfficiency();
            }
        };
    }

    void Start()
    {
        depletionAmount = 100f / 12f;
    }

    public void ReduceEfficiency()
    {
        oxygenLevels -= depletionAmount;
        oxygenLevels = Mathf.Clamp(oxygenLevels, 5f, 100f);
        UIController.Instance.UpdateOxygenLabel(oxygenLevels);
        Blackout.Instance.Blink();
    }
}
