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
    Back
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
    Back
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
    DownArrow
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
    DownArrow
}

[System.Serializable]
public struct Input1
{
    [Header("Pad")]
    public AxisPad movementX;
    public AxisPad movementY;
    public ButtonsPad switchPlayer;
    public ButtonsPad attack1;
    [Header("Keyboard")]
    public AxisKeyboard movementXKeyboard;
    public AxisKeyboard movementYKeyboard;
    public ButtonsKeyboard switchPlayerKeyboard;
    public ButtonsKeyboard attack1Keyboard;
}

public class Player : StateMachine
{
    [Header("Important")]
    public TypesOfPlayer typeOfPlayer;

    [Header("GroundChecker - Blue Sphere")]
    public Vector2 position_groundChecker = new Vector2(0, 0.05f);
    public float width_groundChecker = 0.75f;
    public float height_groundChecker = 0.05f;

    [Header("Movement")]
    public float speed = 100;
    public float jump = 8;
    public float fallSpeed = 10;
    public float maxFallSpeed = 10;

    [Header("Health")]
    public float maxHealth = 3;

    [Header("Attack")]
    public float damage = 1;
    public float delayAttack = 0.5f;
    [Tooltip("Red Line")]
    public Vector2 spawnAttack = new Vector2(0, 0.55f);
    [Tooltip("Red Line")]
    public float rangeAttack = 1;

    [Header("Debug")]
    public float health;

    [Header("Inputs")]
    public Input1 inputs;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public SpriteRenderer sprite { get; private set; }

    void Start()
    {
        SetReferences();
        SetDefaults();

        SetState(new PlayerBegin(this));
    }

    void Update()
    {
        state.Execution();
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        //draw groundChecker
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(position + position_groundChecker, new Vector2(width_groundChecker, height_groundChecker));

        //range attack
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position + spawnAttack, position + spawnAttack + Vector2.left * rangeAttack);
    }

    #region private API

    void SetReferences()
    {
        rb = GetAddComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();

        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void SetDefaults()
    {
        health = maxHealth;
    }

    T GetAddComponent<T>() where T : Component
    {
        //get component
        T component = GetComponent<T>();

        //if there isn't, add component
        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }

    #endregion

    #region public API

    public void GetDamage(float damage)
    {
        //do something only if not already dead
        if (health <= 0) return;

        //get damage and check if dead
        health -= damage;

        if (health <= 0)
        {
            SetState(new PlayerDead(this));
            return;
        }

        anim.SetTrigger("Hurt");
    }

    public void Ress(float life)
    {
        //do something only if dead
        if (health > 0) return;

        //get life - limit max health
        health = life < maxHealth ? life : maxHealth;
    }

    #endregion

    #region static API

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

    #endregion
}
