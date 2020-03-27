using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBegin : State
{
    public PlayerBegin(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        Player player = stateMachine as Player;

        if (player.isKnight)
        {
            stateMachine.SetState(new PlayerKnight(stateMachine));
        }
        else
        {
            stateMachine.SetState(new PlayerWizard(stateMachine));
        }

        yield break;
    }
}
