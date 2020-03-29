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

        switch (player.typeOfPlayer)
        {
            case TypesOfPlayer.knight:
                stateMachine.SetState(new PlayerKnight(stateMachine));
                break;
            case TypesOfPlayer.wizard:
                stateMachine.SetState(new PlayerWizard(stateMachine));
                break;
            default:
                Debug.LogError("There is another Type of Player (enum) - add state machine");
                break;
        }

        yield break;
    }
}
