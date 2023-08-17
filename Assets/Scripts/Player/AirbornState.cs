using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirbornState : MonoBehaviour, IPlayerState
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


        maxSpeed = pStats.airMaxSpeed;
        acceleration = pStats.airAcceleration;
        decceleration = pStats.airDecceleration;
        jumpStrength = pStats.jumpStrength;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartState()
    {
        rb.gravityScale = 5;
        rb.mass = 1;

        StopAllCoroutines();
        StartCoroutine(Animate());
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

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            //coyote time -> perform "grounded" jump
            if(gpc.airTime < pStats.coyoteTime)
            {
                rb.velocity = new Vector2 (rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            }


        }
    }

    public void Move(Vector2 i_move)
    {
        //Debug.Log("i_move: " + i_move);


        if(Mathf.Abs(i_move.x) > 0)
        {
            StopCoroutine(Deccelerate());
            StopCoroutine(Accelerate());
            StartCoroutine(Accelerate());
        } else if (Mathf.Abs(i_move.x) < .01f)
        {
            StopCoroutine(Deccelerate());
            StopCoroutine(Accelerate());
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

                rb.AddForce(moveForce * Vector2.right, ForceMode2D.Force);

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

                //conserve momentum
                if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
                {
                    accelRate = 0;
                }
            
                float speedDiff = targetSpeed - rb.velocity.x;
                float moveForce = speedDiff * accelRate;

                rb.AddForce(moveForce * Vector2.right, ForceMode2D.Force);

                yield return fuWait;
            }
            isDeccel = false;
        }
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            hb.ShootHook(gpc.i_aim);
        } else if (ctx.canceled)
        {
            hb.ResetHook();
        }
    }


    //MOVE THIS TO ANIMATION CONTROLLER!!!!
    IEnumerator Animate()
    {
        while (true)
        {
            if (rb.velocity.y > 0 && ac.sr.sprite != ac.airSprites[0])
            {
                ac.ChangeSprite(ac.airSprites[0]);
            } else if (rb.velocity.y < 0 && ac.sr.sprite != ac.airSprites[1])
            {
                ac.ChangeSprite(ac.airSprites[1]);
            }

            yield return null;
        }
    }

    
}
