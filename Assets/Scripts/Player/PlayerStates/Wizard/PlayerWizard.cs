using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWizard : PlayerMoving
{
    public PlayerWizard(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    protected override void DoAttack1()
    {
        stateMachine.SetState(new WizardAttack1(stateMachine));
    }
}
