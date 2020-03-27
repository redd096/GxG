using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWizard : PlayerKnight
{
    public PlayerWizard(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    //protected override void DoAttack()
    //{
    //    base.DoAttack();
    //
    //    //TODO spara palla di fuoco davanti a sé, se becca nemico chiamare 
    //    //enemy.GetDamage(player.damage, player.isKnight);  
    //}
}
