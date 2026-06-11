using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class KillLog : Logs
{
    [SerializeField]
    private TextMeshProUGUI killer;
    [SerializeField]
    private TextMeshProUGUI deadPool;

    public override void SetLog(string _killer, string _deadPool)
    {
        killer.text = _killer;
        deadPool.text = _deadPool;

        base.SetLog(_killer, _deadPool);
    }
}
