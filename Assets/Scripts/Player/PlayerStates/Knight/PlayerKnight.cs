using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnight : PlayerMoving
{
    public PlayerKnight(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    protected override void DoAttack1()
    {
        stateMachine.SetState(new KnightAttack1(stateMachine));
    }
}
