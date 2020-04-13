using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack1 : PlayerKnight
{
    List<Enemy> enemiesHitted = new List<Enemy>();
    float timeToEndAttack;

    public KnightAttack1(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        //start animation and set endTime
        anim.SetTrigger("Attack");
        timeToEndAttack = Time.time + player.attack1Duration;

        return base.Enter();
    }

    public override void Execution()
    {
        //only movement
        Movement(Inputs.GetAxis(player.inputs.movementX, player.inputs.movementXKeyboard), Inputs.GetAxis(player.inputs.movementY, player.inputs.movementYKeyboard) > 0.5f);

        //if ended attack, change state and return 
        if(IsEndedAttack())
        {
            EndAttack();
            return;
        }

        //check who hit
        Attacking();
    }

    protected virtual void EndAttack()
    {
        stateMachine.SetState(new PlayerKnight(stateMachine));
    }

    #region execution

    protected virtual bool IsEndedAttack()
    {
        return Time.time >= timeToEndAttack;
    }

    protected virtual void Attacking()
    {
        //set start position and direction
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y) + player.spawnAttack;
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;

        //set layer to ignore self
        LayerMask layer = Layer.CreateLayer.LayerAllExcept("Player");

        //check if hit
        //RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, player.rangeAttack, layer);
        RaycastHit2D hit = Physics2D.Linecast(startPosition, startPosition + direction * player.rangeAttack, layer);
        if (hit)
        {
            //if hit an enemy that is not in the list - do damage and add to list
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy && enemiesHitted.Contains(enemy) == false)
            {
                enemy.GetDamage(player.attack1Damage, player.typeOfPlayer);
                enemiesHitted.Add(enemy);
            }
        }
    }

    #endregion
}