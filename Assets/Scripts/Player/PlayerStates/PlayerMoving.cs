using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : State
{
    protected GameManager gm;
    protected Player player;
    protected Transform transform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool isLookingRight;
    protected float timeLastAttack;

    public PlayerMoving(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        GetReferences();

        return base.Enter();
    }

    public override void Execution()
    {
        Movement(Input.GetAxis("Horizontal"), Input.GetButton("Jump"));

        NormalAnimation();

        Switch(Input.GetKeyDown(KeyCode.LeftShift));

        Attack(Input.GetKeyDown(KeyCode.Mouse0));
    }

    #region private API

    #region enter

    void GetReferences()
    {
        gm = GameManager.instance;

        player = stateMachine as Player;
        transform = player.transform;
        rb = player.rb;
        anim = player.anim;
        sprite = player.sprite;
    }

    #endregion

    #region movement

    void Movement(float inputMovement, bool inputJump)
    {
        Vector2 velocity = rb.velocity;

        //speed right or left
        velocity.x = inputMovement * player.speed * Time.deltaTime;

        //speed up or down
        velocity.y = Jump(inputJump);

        rb.velocity = velocity;
    }

    float Jump(bool inputJump)
    {
        //if press input and is grounded - add jump speed
        if (inputJump && IsGrounded())
        {
            return player.jump;
        }

        //else - add gravity
        return AddGravity();
    }

    float AddGravity()
    {
        if (IsGrounded())
        {
            //if grounded - remove y velocity (just a bit to be sure is grounded)
            return -9.81f * Time.deltaTime;
        }
        else
        {
            //if not grounded - add gravity with a limit  
            float _fallingSpeed = rb.velocity.y - player.fallSpeed * Time.deltaTime;
            if (_fallingSpeed < -player.maxFallSpeed)
                _fallingSpeed = -player.maxFallSpeed;

            return _fallingSpeed;
        }
    }

    bool IsGrounded()
    {
        //overlap circle at player foots. If hit more than 1 (one is player), then is grounded
        return Physics2D.OverlapCircleAll(transform.position + Vector3.up * player.height_groundChecker, player.radius_groundChecker).Length > 1;
    }

    #endregion

    void NormalAnimation()
    {
        float speed = rb.velocity.x;

        //right or left
        if (speed > 0)
        {
            isLookingRight = true;
            sprite.flipX = true;
        }
        else if (speed < 0)
        {
            isLookingRight = false;
            sprite.flipX = false;
        }

        if (IsGrounded())
        {
            anim.SetTrigger("Grounded");
            anim.SetFloat("Speed", speed);
        }
        else
        {
            anim.SetTrigger("Fall");
        }
    }

    void Switch(bool inputSwitch)
    {
        PlayerSelector playerSelector = gm.playerSelector;

        //if press input after delay, try to switch 
        if (inputSwitch && Time.time > playerSelector.timeLastSwitch)
        {
            playerSelector.timeLastSwitch = Time.time + playerSelector.delaySwitch;

            playerSelector.SwitchPlayer(player.isKnight);
        }
    }

    #region attack

    protected virtual void Attack(bool inputAttack)
    {
        //if press input after delay, do attack
        if (inputAttack && Time.time > timeLastAttack)
        {
            timeLastAttack = Time.time + player.delayAttack;

            DoAttack();
        }
    }

    protected virtual void DoAttack()
    {
        anim.SetTrigger("Attack");
        //do something
    }

    #endregion

    #endregion
}
