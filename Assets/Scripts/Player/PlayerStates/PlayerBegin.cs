using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBegin : PlayerState
{
    public PlayerBegin(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        switch (player.typeOfPlayer)
        {
            case TypesOfPlayer.knight:
                stateMachine.SetState(new PlayerKnight(stateMachine));
                yield break;
            case TypesOfPlayer.wizard:
                stateMachine.SetState(new PlayerWizard(stateMachine));
                yield break;
            default:
                Debug.LogError("There is another Type of Player (enum) - add state machine (" + player.typeOfPlayer + ")");
                yield break;
        }
    }
}
