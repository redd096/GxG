using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnight : PlayerMoving
{
    public PlayerKnight(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    protected override void DoAttack()
    {
        base.DoAttack();

        //TODO controlla se ha un nemico di fronte a sé, nel caso chiama
        //enemy.GetDamage(player.damage, player.isKnight);

        //set start position, direction and StartInColliders false
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y) + player.spawnAttack;
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;
        Physics2D.queriesStartInColliders = false;

        //raycast, if hit enemy do damage
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, player.rangeAttack);
        if (hit)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.GetDamage(player.damage, player.typeOfPlayer);
            }
        }

        //hit = Physics2D.Linecast(startPosition, startPosition + direction * player.rangeAttack);
    }
}
