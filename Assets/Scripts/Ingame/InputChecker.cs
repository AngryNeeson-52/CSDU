using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTN_FLOAT
{
    Horizontal,
    Vertical,
    MouseHori,
    MouseVert,
}

public enum BTN_BOOL
{
    ESC,
    Jump,
    Fire,
    Aiming,
    ReLoad,
}

public class InputChecker
{
    public float this[BTN_FLOAT btn]
    {
        get
        {
            switch (btn)
            {
                case BTN_FLOAT.Horizontal:
                    return Input.GetAxisRaw("Horizontal");
                case BTN_FLOAT.Vertical:
                    return Input.GetAxisRaw("Vertical");
                case BTN_FLOAT.MouseHori:
                    return Input.GetAxisRaw("Mouse X");
                case BTN_FLOAT.MouseVert:
                    return Input.GetAxisRaw("Mouse Y");
                default:
                    Debug.Log("잘못된 플레이어 입력");
                    return 0;
            }
        }
    }

    public bool this[BTN_BOOL btn]
    {
        get
        {
            switch (btn)
            {
                case BTN_BOOL.ESC:
                    return Input.GetKeyDown(KeyCode.Escape);
                case BTN_BOOL.Jump:
                    return Input.GetKey(KeyCode.Space);
                case BTN_BOOL.Fire:
                    return Input.GetMouseButton(0);
                case BTN_BOOL.Aiming:
                    return Input.GetMouseButton(1);
                case BTN_BOOL.ReLoad:
                    return Input.GetKey(KeyCode.R);
                default:
                    Debug.Log("잘못된 플레이어 입력");
                    return false;
            }
        }
    }
}
