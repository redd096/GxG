using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : PlayerState
{
    public PlayerDead(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        //wait while dead
        anim.SetBool("Dead", true);

        while (player.health <= 0)
        {
            //TODO TEMPORANEO
            if (Inputs.GetButtonDown(player.inputs.ressTEMP, player.inputs.ressTEMPKeyboard))
                player.Ress(player.maxHealth);

            yield return null;
        }

        //ress
        anim.SetBool("Dead", false);

        stateMachine.SetState(new PlayerBegin(stateMachine));
    }
}
