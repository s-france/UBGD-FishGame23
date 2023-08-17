using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirGrappleState : MonoBehaviour, IPlayerState
{
    HookBehavior hb;
    GameplayPlayerController gpc;
    PlayerAnimationController ac;
    PlayerStats pStats;
    Rigidbody2D rb;

    float pullStrength;
    float maxDistance;

    float jumpStrength;


    // Start is called before the first frame update
    void Awake()
    {
        hb = transform.parent.GetComponentInChildren<HookBehavior>();

        gpc = GetComponentInParent<GameplayPlayerController>();
        ac = transform.parent.GetComponentInChildren<PlayerAnimationController>();
        pStats = GetComponentInParent<PlayerStats>();
        rb = GetComponentInParent<Rigidbody2D>();

        pullStrength = pStats.airGrapplePullStrength;
        maxDistance = pStats.maxGrappleDistance;

        jumpStrength = pStats.grappleJumpStrength;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartState()
    {
        Debug.Log("entered air grapple state!");

        rb.gravityScale = 2;

        rb.mass = 2;

        StopAllCoroutines();
        StartCoroutine(Pull());
        StartCoroutine(ac.Swing(hb.transform));
        Move(gpc.i_move);
    }

    public void EndState()
    {
        ac.isSwinging = false; //ends ac coroutine - probably redundant
        ac.ResetSpriteRotation();
        StopAllCoroutines();
    }

    IEnumerator Pull()
    {
        WaitForFixedUpdate fuWait = new WaitForFixedUpdate();

        float targetV = 15f;
        float accel = 3f;

        float pull = pullStrength;

        while (true)
        {


            Vector2 direction = (hb.transform.position - transform.position).normalized;
            Vector2 perp = -Vector2.Perpendicular(direction).normalized;

            float yComp = rb.velocity.magnitude * Mathf.Sin(Vector2.SignedAngle(perp, rb.velocity.normalized));

            Debug.DrawRay(transform.position, yComp*direction, Color.red);

            //Debug.Log("yComp: " + yComp);

            if(/*rb.velocity.magnitude < 20 ||*/ Vector2.Angle(rb.velocity, direction) > 90)
            {
                rb.AddForce(direction * pullStrength, ForceMode2D.Force);
            } /*else*/
            {
                float diff = targetV - yComp;
                float force = diff * accel;

                rb.AddForce(direction * force, ForceMode2D.Force);

                //Debug.Log("force: " + (direction * force));
            }

            

            yield return fuWait;
        }
    }



    public void Move(Vector2 i_move)
    {
        //throw new System.NotImplementedException();

        //if (perpV < small)
        //{apply impulse + addConstantForce}
        //else {addConstantForce}
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(ctx.performed) //on pressed
        {
            gpc.isJumping = true;
            if(rb.velocity.y < 0) {rb.velocity.Set(rb.velocity.x, 0);}
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

            hb.ResetHook();
            gpc.ChangeState(2);
        } else if (ctx.canceled) //on released
        {
            
        }
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            hb.ResetHook();

            gpc.ChangeState(2);
        }
    }
}
