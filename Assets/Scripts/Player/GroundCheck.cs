using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    GameplayPlayerController gpc;
    HookBehavior hb;
    SlopeCheck sc;
    Collider2D groundCheck;

    List<GameObject> currentCollisions;


    void Awake()
    {
        gpc = GetComponentInParent<GameplayPlayerController>();
        hb = transform.parent.GetComponentInChildren<HookBehavior>();
        sc = GetComponent<SlopeCheck>();
        groundCheck = GetComponent<BoxCollider2D>();

        currentCollisions = new List<GameObject>();
        gpc.airTime = 0;
        gpc.groundTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        currentCollisions.Add(col.gameObject);

        if (col.gameObject.layer == LayerMask.NameToLayer("Ground") && !gpc.isGrounded)
        {
            Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
            rb.velocity.Set(rb.velocity.x, 0);

            gpc.isJumping = false;

            if(hb.isAttached)
            {
                gpc.ChangeState(3); //grounded grapple state
            } else
            {
                gpc.ChangeState(1); //ground state
            }

            gpc.isGrounded = true;
            StartCoroutine(groundCounter());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        currentCollisions.Remove(col.gameObject);

        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            foreach (GameObject obj in currentCollisions.Where(obj => obj.layer == LayerMask.NameToLayer("Ground")))
            {
                return;
            }
            
            if (gpc.isJumping)
            {
                StopCoroutine(ChangeDelay(.1f));
                gpc.ChangeState(2); //air state
                gpc.isGrounded = false;
                StartCoroutine(airCounter());
            } else
            {
                //TEMPORARY!!!
                if(!hb.isAttached /*|| gpc.isGSliding*/)
                {
                    StartCoroutine(ChangeDelay(.1f));
                } else if (hb.isAttached)
                {
                    StopCoroutine(ChangeDelay(.1f));
                    gpc.ChangeState(4);
                    gpc.isGrounded = false;
                    StartCoroutine(airCounter());
                }

            }
            
        }

    }

    IEnumerator ChangeDelay(float delayTime)
    {
        //Debug.Log("slipped off!");
        //Debug.Log("slope angle: " + gpc.slopeNormalPerp);

        float count = 0;

        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();

        while (count <= delayTime && currentCollisions.Find(obj => obj.layer == LayerMask.NameToLayer("Ground")) == null && !gpc.isJumping && gpc.isGrounded && gpc.isOnSlope)
        {
            float offset = .1f;
            if(Mathf.Abs(gpc.slopeNormalPerp.y) > .01f)
            {
                offset = Mathf.Abs(gpc.slopeNormalPerp.y*.2f);
            }
            //Debug.Log("slopeNormalPerp: " + gpc.slopeNormalPerp);
            //Debug.Log("offset: " + offset);

            if(rb.velocity.x > 0)
            {
                rb.velocity = Mathf.Abs(rb.velocity.x) * (new Vector2(gpc.slopeNormalPerp.x, gpc.slopeNormalPerp.y - offset)).normalized;
            } else if (rb.velocity.x < 0)
            {
                rb.velocity = Mathf.Abs(rb.velocity.x) * (new Vector2(-gpc.slopeNormalPerp.x, -gpc.slopeNormalPerp.y - offset)).normalized;
            }

            /*float ang = Vector2.Angle(rb.velocity, gpc.slopeNormalPerp);

            rb.velocity = rotate(rb.velocity, ang);*/


            //Debug.Log("rb.velocity: " + rb.velocity);

                
             
            
            count += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }



        if(currentCollisions.Find(obj => obj.layer == LayerMask.NameToLayer("Ground")) == null)
        {

            if(hb.isAttached)
            {
                //ADD THIS WHEN IMPLEMENTED!!!!!
                gpc.ChangeState(4); //air grapple state
            } else
            {
                gpc.ChangeState(2); //air state
            }

            gpc.isGrounded = false;
            StartCoroutine(airCounter());
        }

    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        float rad = Mathf.Deg2Rad * delta;

    return new Vector2(
        v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
        v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
    );
    }

    IEnumerator airCounter()
    {
        while (!gpc.isGrounded)
        {
            gpc.airTime += Time.deltaTime;
            yield return null;
        }

        gpc.airTime = 0;
    }

    IEnumerator groundCounter()
    {
        while (gpc.isGrounded)
        {
            gpc.groundTime += Time.deltaTime;
            yield return null;
        }

        gpc.groundTime = 0;
    }
}
