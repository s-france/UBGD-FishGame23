using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundedGrappleState : MonoBehaviour, IPlayerState
{
    HookBehavior hb;
    GameplayPlayerController gpc;
    PlayerAnimationController ac;
    PlayerStats pStats;
    Rigidbody2D rb;

    Vector2 direction;
    Vector2 perp;
    float groundGrapAngle;
    float dashCharge;

    float jumpStrength;
    float slideMinSpeed;
    float slideMaxSpeed;
    float slideAccel;
    float slideDeccel;
    float liftOffAngle;

    bool isSliding;
    bool isWalking;
    bool isSlideAccel;
    bool isSlideDeccel;


    void Awake()
    {
        hb = transform.parent.gameObject.GetComponentInChildren<HookBehavior>();

        gpc = GetComponentInParent<GameplayPlayerController>();
        ac = transform.parent.GetComponentInChildren<PlayerAnimationController>();
        pStats = GetComponentInParent<PlayerStats>();
        rb = GetComponentInParent<Rigidbody2D>();

        jumpStrength = pStats.jumpStrength;
        slideMinSpeed = pStats.grappleSlideMinSpeed;
        slideMaxSpeed = pStats.grappleSlideMaxSpeed;
        slideAccel = pStats.grappleSlideAcceleration;
        slideDeccel = pStats.grappleSlideDecceleration;
        liftOffAngle = pStats.grappleLiftOffAngle;

        isSliding = false;
        isWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (hb.transform.position - transform.position).normalized;
        perp = -Vector2.Perpendicular(direction).normalized;

        //NEED TO ACCOUNT FOR HORIZONTAL
        groundGrapAngle = Vector2.Angle(gpc.slopeNormalPerp, perp);
    }


    public void StartState()
    {
        Debug.Log("entered ground grapple state!");

        rb.gravityScale = 0;

        rb.mass = 1;

        StopAllCoroutines();
        isSliding = false;

        //TEMPORARY
        Move(gpc.i_move);

        if(Mathf.Abs(rb.velocity.x) > slideMinSpeed)
        {
            StartCoroutine(Slide());
        }

    }

    public void EndState()
    {
        StopAllCoroutines();
    }



    public void Move(Vector2 i_move)
    {
        if(!isSliding)

        if(Mathf.Abs(rb.velocity.x) > slideMinSpeed)
        {
            StartCoroutine(Slide());
        } else if(Mathf.Abs(i_move.x) > 0)
        {
            //START GRAPPLE WALK COROUTINE
        }

        if(Mathf.Abs(i_move.x) > 0)
        {
            if(Mathf.Sign(i_move.x) == Mathf.Sign(direction.x))
            {

            } else if(groundGrapAngle > liftOffAngle) //holding away from grapple point
            {

            }

        } else if (Mathf.Abs(i_move.x) < .01f)
        {

        }
    }

    IEnumerator Slide()
    {
        if(!isSliding)
        {
            isSliding = true;

            WaitForFixedUpdate fuWait = new WaitForFixedUpdate();

            ac.ChangeSprite(ac.groundSprites[3]); //TEMPORARY ANIMAITON
            if(rb.velocity.x > 0) {ac.FlipX(1);} //face slide direction
            if(rb.velocity.x < 0) {ac.FlipX(-1);}

            while(Mathf.Abs(rb.velocity.x) > slideMinSpeed && isSliding)
            {
                //Debug.Log("sliding!");
                if(Mathf.Sign(rb.velocity.x) == Mathf.Sign(direction.x))
                {
                    if(Mathf.Abs(gpc.i_move.x) > .01f && Mathf.Sign(gpc.i_move.x) != Mathf.Sign(rb.velocity.x))
                    {
                        SlideDeccel();
                    } else //TRY THIS: only accel when holding forward
                    {
                        SlideAccel();
                    }
                } else if(groundGrapAngle > liftOffAngle && direction.y > 0)
                {
                    LiftOff();
                } else if(Mathf.Abs(gpc.i_move.x) > .01f && Mathf.Sign(gpc.i_move.x) != Mathf.Sign(rb.velocity.x))
                {
                    SlideDeccel();
                } else if(Mathf.Abs(gpc.i_move.x) > .01f && Mathf.Sign(gpc.i_move.x) == Mathf.Sign(rb.velocity.x))
                {
                    //CHARGE DASH
                } else
                {
                    //CHECK - I believe this is already done
                    //conserve momentum
                }

                yield return fuWait;
            }
            isSliding = false;


            //SWAP TO GROUND GRAPPLE WALK COROUTINE


        }
    }

    //runs when sliding toward grapple point
    void SlideAccel()
    {
        //ADD I_MOVE TO THIS
        //if(i_move direction == slide direction) -> increase speed

        float targetSpeed = Mathf.Sign(rb.velocity.x) * slideMaxSpeed;
        float accelRate = slideAccel * Mathf.Abs(direction.x);

        //conserve momentum
        if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = 0;
        }

        float speedDiff = targetSpeed - rb.velocity.x;
        float moveForce = speedDiff * accelRate;


        rb.AddForce(moveForce * gpc.slopeNormalPerp, ForceMode2D.Force);
    }

    void SlideDeccel()
    {
        float targetSpeed = 0;
        float accelRate = slideDeccel;

        //conserve momentum - NOT NEEDED HERE
        if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = 0;
        }

        float speedDiff = targetSpeed - rb.velocity.x;
        float moveForce = speedDiff * accelRate;

        rb.AddForce(moveForce * gpc.slopeNormalPerp, ForceMode2D.Force);   
    }

    void LiftOff()
    {
        Debug.Log("liftoff!");
        rb.velocity = perp * rb.velocity.magnitude * Mathf.Sign(rb.velocity.x);
    }

    /*IEnumerator Walk()
    {

    }*/

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) //on pressed
        {
            gpc.isJumping = true;
            rb.velocity.Set(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

            hb.ResetHook();
        } else if (ctx.canceled) //on released
        {
            
        }
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            hb.ResetHook();

            //if (dash charged) -> dash

            gpc.ChangeState(1);
        }
    }
    

    
}
