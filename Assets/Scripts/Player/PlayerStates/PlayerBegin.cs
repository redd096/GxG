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

        if (player.typeOfPlayer == TypesOfPlayer.knight)
        {
            stateMachine.SetState(new PlayerKnight(stateMachine));
        }
        else if (player.typeOfPlayer == TypesOfPlayer.wizard)
        {
            stateMachine.SetState(new PlayerWizard(stateMachine));
        }

        yield break;
    }
}
