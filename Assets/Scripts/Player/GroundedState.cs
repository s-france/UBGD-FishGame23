using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedState : MonoBehaviour, IPlayerState
{
    HookBehavior hb;
    GameplayPlayerController gpc;
    PlayerAnimationController ac;
    PlayerStats pStats;
    Rigidbody2D rb;


    float maxSpeed;
    float acceleration;
    float decceleration;
    float jumpStrength;

    bool isAccel;
    bool isDeccel;

    void Awake()
    {
        hb = transform.parent.gameObject.GetComponentInChildren<HookBehavior>();

        gpc = GetComponentInParent<GameplayPlayerController>();
        ac = transform.parent.GetComponentInChildren<PlayerAnimationController>();
        pStats = GetComponentInParent<PlayerStats>();
        rb = GetComponentInParent<Rigidbody2D>();


        maxSpeed = pStats.groundMaxSpeed;
        acceleration = pStats.groundAcceleration;
        decceleration = pStats.groundDecceleration;
        jumpStrength = pStats.jumpStrength;
    }


    void FixedUpdate()
    {

    }

    public void StartState()
    {
        rb.gravityScale = 0;
        rb.mass = 1;

        rb.velocity.Set(rb.velocity.x, 0);

        StopAllCoroutines();
        ac.ChangeSprite(ac.groundSprites[0]);
        ac.isWalking = false;
        isAccel = false;
        isDeccel = false;
        Move(gpc.i_move);
    }
    public void EndState()
    {
        isAccel = false;
        isDeccel = false;
        StopAllCoroutines();
    }


    public void Move(Vector2 i_move)
    {
        //Debug.Log("i_move: " + i_move);

        
        if(Mathf.Abs(i_move.x) > 0)
        {
            if(i_move.x > 0) {ac.FlipX(1);}
            if(i_move.x < 0) {ac.FlipX(-1);}

            StopCoroutine(ac.WalkCycle());
            StartCoroutine(Accelerate());
            StartCoroutine(ac.WalkCycle());

        } else if (Mathf.Abs(i_move.x) < .01f)
        {
            ac.isWalking = false;
            StartCoroutine(Deccelerate());
        }
    }

    IEnumerator Accelerate()
    {
        if(!isAccel)
        {
            isAccel = true;

            WaitForFixedUpdate fuWait = new WaitForFixedUpdate();

            while (Mathf.Abs(gpc.i_move.x) > 0.01f)
            {
                float targetSpeed = gpc.i_move.x * maxSpeed;
                float accelRate = (Mathf.Abs(targetSpeed) > .01f) ? acceleration : decceleration;

                //conserve momentum
                if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
                {
                    accelRate = 0;
                }
            
                float speedDiff = targetSpeed - rb.velocity.x;
                float moveForce = speedDiff * accelRate;

                rb.AddForce(moveForce * gpc.slopeNormalPerp, ForceMode2D.Force);

                yield return fuWait;
            }
            isAccel = false;
        }
    }

    IEnumerator Deccelerate()
    {
        if(!isDeccel)
        {
            isDeccel = true;

            WaitForFixedUpdate fuWait = new WaitForFixedUpdate();

            while (Mathf.Abs(gpc.i_move.x) < .01f)
            {
                float targetSpeed = 0;
                float accelRate = decceleration;

                //conserve momentum - NOT NEEDED HERE
                if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
                {
                    accelRate = 0;
                }
            
                float speedDiff = targetSpeed - rb.velocity.x;
                float moveForce = speedDiff * accelRate;

                rb.AddForce(moveForce * gpc.slopeNormalPerp, ForceMode2D.Force);

                yield return fuWait;
            }
            isDeccel = false;
        }
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) //on pressed
        {
            gpc.isJumping = true;
            rb.velocity = new Vector2 (rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        } else if (ctx.canceled) //on released
        {
            
        }
    }


    public void Grapple(InputAction.CallbackContext ctx)
    {
        //shoot hook
        //if hook hits -> attach

        if(ctx.performed)
        {
            hb.ShootHook(gpc.i_aim);
        } else if (ctx.canceled)
        {
            hb.ResetHook();
        }
    }

    void ShootHook(Vector2 direction)
    {

    }

    void ResetHook()
    {
        
    }
}
