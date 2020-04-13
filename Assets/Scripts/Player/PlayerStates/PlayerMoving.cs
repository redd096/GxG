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
        Movement(Player.GetAxis(player.inputs.movementX, player.inputs.movementXKeyboard), Player.GetAxis(player.inputs.movementY, player.inputs.movementYKeyboard) > 0.5f);

        NormalAnimation();

        Switch(Player.GetButtonDown(player.inputs.switchPlayer, player.inputs.switchPlayerKeyboard));

        Attack(Player.GetButtonDown(player.inputs.attack1, player.inputs.attack1Keyboard));
    }

    #region enter

    protected virtual void GetReferences()
    {
        gm = GameManager.instance;

        player = stateMachine as Player;
        transform = player.transform;
        rb = player.rb;
        anim = player.anim;
        sprite = player.sprite;
    }

    #endregion

    #region execution

    #region movement

    protected virtual void Movement(float inputMovement, bool inputJump)
    {
        Vector2 velocity = rb.velocity;

        //speed right or left
        velocity.x = inputMovement * player.speed * Time.deltaTime;

        //speed up or down
        velocity.y = Jump(inputJump);

        rb.velocity = velocity;
    }

    protected virtual float Jump(bool inputJump)
    {
        //if press input and is grounded - add jump speed
        if (inputJump && IsGrounded())
        {
            return player.jump;
        }

        //else - add gravity
        return AddGravity();
    }

    protected virtual float AddGravity()
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

    protected virtual bool IsGrounded()
    {
        //overlap box at player foots. If hit more than 1 (one is player), then is grounded
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return Physics2D.OverlapBoxAll(position + player.position_groundChecker, new Vector2(player.width_groundChecker, player.height_groundChecker), 0).Length > 1;
    }

    #endregion

    protected virtual void NormalAnimation()
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

        //set animation
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

    protected virtual void Switch(bool inputSwitch)
    {
        //if press input, try to switch 
        if (inputSwitch)
        {
            gm.playerSelector.SwitchPlayer(player.typeOfPlayer);
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
