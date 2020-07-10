using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Input1
{
    [Header("Pad")]
    public AxisPad movementX;
    public AxisPad movementY;
    public ButtonsPad switchPlayer;
    public ButtonsPad attack1;
    public ButtonsPad ressTEMP;
    [Header("Keyboard")]
    public AxisKeyboard movementXKeyboard;
    public AxisKeyboard movementYKeyboard;
    public ButtonsKeyboard switchPlayerKeyboard;
    public ButtonsKeyboard attack1Keyboard;
    public ButtonsKeyboard ressTEMPKeyboard;
}

public class Player : StateMachine
{
    [Header("Important")]
    public TypesOfPlayer typeOfPlayer;

    [Header("GroundChecker - Blue Cube")]
    [DraggableLocalPoint] public Vector3 testDrag;
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

    [Header("Attack1")]
    public float attack1Damage = 1;
    public float attack1Duration = 0.5f;
    public float attack1Delay = 0.5f;
    [HideInInspector] public float timeLastAttack;
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

    
    public Vector3 test;

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
}
