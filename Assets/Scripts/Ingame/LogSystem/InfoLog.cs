using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoLog : Logs
{
    [SerializeField]
    private TextMeshProUGUI infoText;

    public override void SetLog(string _info)
    {
        infoText.text = _info;

        base.SetLog(_info);
    }
}
