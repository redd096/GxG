using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : PlayerState
{
    public PlayerMoving(StateMachine _stateMachine) : base(_stateMachine)
    {
    }

    public override void Execution()
    {
        Movement(Inputs.GetAxis(player.inputs.movementX, player.inputs.movementXKeyboard), Inputs.GetAxis(player.inputs.movementY, player.inputs.movementYKeyboard) > 0.5f);

        RotatePlayer();
        NormalAnimation();

        Switch(Inputs.GetButtonDown(player.inputs.switchPlayer, player.inputs.switchPlayerKeyboard));

        Attack1(Inputs.GetButtonDown(player.inputs.attack1, player.inputs.attack1Keyboard));
    }

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

    #region animation

    protected virtual void RotatePlayer()
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
    }

    protected virtual void NormalAnimation()
    {
        float speed = rb.velocity.x;

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

    #endregion

    protected virtual void Switch(bool inputSwitch)
    {
        //if press input, try to switch 
        if (inputSwitch)
        {
            gm.playerSelector.SwitchPlayer(player.typeOfPlayer);
        }
    }

    #region attack

    protected virtual void Attack1(bool inputAttack)
    {
        //if press input after delay, do attack
        if (inputAttack && Time.time > player.timeLastAttack)
        {
            player.timeLastAttack = Time.time + player.attack1Delay;

            DoAttack1();
        }
    }

    protected virtual void DoAttack1()
    {
        anim.SetTrigger("Attack");
        //do something
    }

    #endregion

    #endregion
}
