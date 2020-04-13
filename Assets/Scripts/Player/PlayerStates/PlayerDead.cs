using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : State
{
    Player player;
    Animator anim;

    public PlayerDead(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        GetReferences();

        //wait while dead
        anim.SetBool("Dead", true);

        while (player.health <= 0)
        {
            //TODO TEMPORANEO
            if (Input.GetKeyDown(KeyCode.K))
                player.Ress(player.maxHealth);

            yield return null;
        }

        //ress
        anim.SetBool("Dead", false);

        stateMachine.SetState(new PlayerBegin(stateMachine));
    }

    #region enter

    void GetReferences()
    {
        player = stateMachine as Player;
        anim = player.anim;
    }

    #endregion
}
