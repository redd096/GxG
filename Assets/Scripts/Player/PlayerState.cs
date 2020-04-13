using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected GameManager gm;
    protected Player player;
    protected Transform transform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool isLookingRight;

    public PlayerState(StateMachine _stateMachine) : base(_stateMachine)
    {
        GetReferences();
    }

    protected virtual void GetReferences()
    {
        gm = GameManager.instance;

        player = stateMachine as Player;
        transform = player.transform;
        rb = player.rb;
        anim = player.anim;
        sprite = player.sprite;
    }
}
