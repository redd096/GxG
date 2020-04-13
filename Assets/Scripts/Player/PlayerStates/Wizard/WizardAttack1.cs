using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack1 : KnightAttack1
{
    public WizardAttack1(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    //TODO per ora è uguale all'attacco del cavaliere, deve invece castare le varie skill
    protected override void EndAttack()
    {
        stateMachine.SetState(new PlayerWizard(stateMachine));
    }
}
