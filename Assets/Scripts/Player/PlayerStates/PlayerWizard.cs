using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWizard : PlayerMoving
{
    public PlayerWizard(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    //TODO MODIFICARE ATTACCO DELLA MAGA - PER ORA è UGUALE AL CAVALIERE
    protected override void DoAttack()
    {
        base.DoAttack();

        //set start position and direction
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y) + player.spawnAttack;
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;

        //set layer to ignore self
        LayerMask layer = Layer.CreateLayer.LayerAllExcept("Player");

        //check, if hit enemy do damage
        //RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, player.rangeAttack, layer);
        RaycastHit2D hit = Physics2D.Linecast(startPosition, startPosition + direction * player.rangeAttack, layer);
        if (hit)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.GetDamage(player.damage, player.typeOfPlayer);
            }
        }
    }
}
