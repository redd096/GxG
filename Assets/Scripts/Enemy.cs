using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Shield")]
    [SerializeField] TypesOfPlayer[] whoRemoveShield = new TypesOfPlayer[1] { TypesOfPlayer.wizard };
    [SerializeField] int numberAttacksToDestroy = 1;
    [SerializeField] float delayShieldRefill = 10;

    [Header("Health")]
    [SerializeField] TypesOfPlayer[] whoKillEnemy = new TypesOfPlayer[1] { TypesOfPlayer.knight };
    [SerializeField] float health = 10;

    [Header("Attack")]
    [SerializeField] float damage = 0.5f;
    [SerializeField] float delayAttack = 3;
    [Tooltip("Red Line")]
    public Vector2 spawnAttack = new Vector2(0, 0.55f);
    [Tooltip("Red Line")]
    public float rangeAttack = 1;

    [Header("Debug")]
    [SerializeField] int shieldHealth;
    float timeLastShieldRefill;
    float timeLastAttack;
    bool isLookingRight;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    GameObject shield;

    void Start()
    {
        SetReferences();
        SetDefaults();
    }

    void Update()
    {
        Rotate();

        CheckAttack();

        CheckRefillShield();
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);

        //range attack
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position + spawnAttack, position + spawnAttack + Vector2.left * rangeAttack);
    }

    #region private API

    #region start

    void SetReferences()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();

        sprite = GetComponentInChildren<SpriteRenderer>();

        shield = transform.Find("Graphics").Find("Shield").gameObject;
    }

    void SetDefaults()
    {
        shieldHealth = numberAttacksToDestroy;
        shield.SetActive(true);
    }

    #endregion

    #region update

    void Rotate()
    {
        isLookingRight = rb.velocity.x > 0 ? true : false;

        sprite.flipX = isLookingRight;
    }

    #region attack

    void CheckAttack()
    {
        //check delay attack
        if (Time.time > timeLastAttack)
        {
            timeLastAttack = Time.time + delayAttack;
            Attack();
        }
    }

    void Attack()
    {
        //animation
        anim.SetTrigger("Attack");

        //set start position and direction
        Vector2 startPosition = new Vector2(transform.position.x, transform.position.y) + spawnAttack;
        Vector2 direction = isLookingRight ? Vector2.right : Vector2.left;

        //set layer to ignore self
        LayerMask layer = Layer.CreateLayer.LayerAllExcept("Enemy");

        //check, if hit player do damage
        //RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, rangeAttack, layer);
        RaycastHit2D hit = Physics2D.Linecast(startPosition, startPosition + direction * rangeAttack, layer);
        if (hit)
        {
            Player player = hit.transform.GetComponent<Player>();
            if (player)
            {
                player.GetDamage(damage);
            }
        }
    }

    #endregion

    void CheckRefillShield()
    {
        //refill shield
        if (Time.time > timeLastShieldRefill)
        {
            shieldHealth = numberAttacksToDestroy;
            shield.SetActive(true);
        }
    }

    #endregion

    bool HitByRightPlayer(TypesOfPlayer whoAttacked, TypesOfPlayer[] playersWhoCanHit)
    {
        foreach (TypesOfPlayer player in playersWhoCanHit)
        {
            if (player == whoAttacked)
                return true;
        }

        return false;
    }

    void Die()
    {
        anim.SetBool("Dead", true);
        Destroy(gameObject, 2);
    }

    #endregion

    public void GetDamage(float damage, TypesOfPlayer whoAttacked)
    {
        //do something only if not already dead
        if (health <= 0) return;

        //remove shield
        if(shieldHealth > 0)
        {
            if (HitByRightPlayer(whoAttacked, whoRemoveShield))
            {
                shieldHealth--;
                anim.SetTrigger("Hurt");
            }

            //if removed, start refill delay
            if (shieldHealth <= 0)
            {
                timeLastShieldRefill = Time.time + delayShieldRefill;
                shield.SetActive(false);
            }
            
            return;
        }

        //then damage
        if(HitByRightPlayer(whoAttacked, whoKillEnemy))
        {
            health -= damage;
            anim.SetTrigger("Hurt");
        }

        //check if dead
        if (health <= 0)
        {
            Die();
            return;
        }
    }
}
