using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AxisPad
{
    RightStickX,
    RightStickY,
    LeftStickX,
    LeftStickY,
    DPadX,
    DPadY,
    LT,
    RT,
    A,
    B,
    X,
    Y,
    LB,
    RB,
    RightStick,
    LeftStick,
    Start,
    Back,
}

[System.Serializable]
public enum ButtonsPad
{
    A,
    B,
    X,
    Y,
    LB,
    RB,
    RightStick,
    LeftStick,
    Start,
    Back,
}

[System.Serializable]
public enum AxisKeyboard
{
    MouseX,
    MouseY,
    LeftRight,
    UpDown,
    AD,
    WS,
    Space,
    LeftClick,
    RightClick,
    Shift,
    Ctrl,
    Esc,
    Enter,
    LeftArrow,
    RightArrow,
    UpArrow,
    DownArrow,
    R,
}

[System.Serializable]
public enum ButtonsKeyboard
{
    Space,
    LeftClick,
    RightClick,
    Shift,
    Ctrl,
    Esc,
    Enter,
    LeftArrow,
    RightArrow,
    UpArrow,
    DownArrow,
    R,
}

public static class Inputs
{
    public static float GetAxis(AxisPad inputPad, AxisKeyboard inputKeyboard)
    {
        float pad = Input.GetAxis(inputPad.ToString());
        if (pad != 0)
            return pad;
        else
            return Input.GetAxis(inputKeyboard.ToString());
    }

    public static bool GetButtonDown(ButtonsPad buttonPad, ButtonsKeyboard buttonKeyboard)
    {
        bool pad = Input.GetButtonDown(buttonPad.ToString());
        if (pad)
            return pad;
        else
            return Input.GetButtonDown(buttonKeyboard.ToString());
    }

    public static bool GetButton(ButtonsPad buttonPad, ButtonsKeyboard buttonKeyboard)
    {
        bool pad = Input.GetButton(buttonPad.ToString());
        if (pad)
            return pad;
        else
            return Input.GetButton(buttonKeyboard.ToString());
    }
}
