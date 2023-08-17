using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehavior : MonoBehaviour
{
    Transform player;
    GameplayPlayerController gpc;
    Rigidbody2D playerRB;

    PlayerStats pStats;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sr;
    LineRenderer lr;

    bool isShot;
    [HideInInspector] public bool isAttached;
    bool isRecalling;
    Vector2 hookPos;


    void Awake()
    {
        player = transform.parent;
        playerRB = player.GetComponent<Rigidbody2D>();
        gpc = player.GetComponent<GameplayPlayerController>();

        pStats = GetComponentInParent<PlayerStats>();

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();

        Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 hookPoint = col.GetContact(0).point;

        AttachHook(hookPoint);
    }

    void AttachHook(Vector2 pos)
    {
        isAttached = true;
        hookPos = pos;

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        if(gpc.isGrounded) {gpc.ChangeState(3);} //ground grapple state
        else {gpc.ChangeState(4);} //air grapple state
    }


    public void ShootHook(Vector2 direction)
    {
        if(!isShot)
        {
            isShot = true;
            Enable();
            //rb.velocity = playerRB.velocity;
            rb.AddForce(direction * pStats.grappleLaunchStrength, ForceMode2D.Impulse);
        }
    }

    IEnumerator RecallHook()
    {
        isRecalling = true;
        isAttached = false;
        rb.isKinematic = false;
        col.enabled = false;

        float recallSpeed = .4f;

        while(Mathf.Abs(transform.position.x - player.position.x) > .1f && Mathf.Abs(transform.position.y - player.position.y) > .1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, recallSpeed);
            recallSpeed += .02f;
            yield return new WaitForFixedUpdate();
        }

        Disable();
    }

    //runs when grapple button released
    public void ResetHook()
    {
        if(isShot && !isRecalling)
        {
            StartCoroutine(RecallHook());
        } else {Disable();}
    }

    void Enable()
    {
        transform.localPosition = Vector2.zero;
        transform.SetParent(null);

        sr.enabled = true;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        col.enabled = true;

        StartCoroutine(RenderLine());
    }

    void Disable()
    {
        isRecalling = false;
        isShot = false;
        isAttached = false;

        transform.SetParent(player);

        sr.enabled = false;
        transform.localPosition = Vector2.zero;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        lr.enabled = false; //ends coroutine
    }

    IEnumerator RenderLine()
    {
        lr.enabled = true;
        
        while(lr.enabled)
        {
            if(!isAttached)
            {
                hookPos = transform.position;
            }

            lr.SetPosition(0, player.position);
            lr.SetPosition(1, hookPos);

            yield return null;
        }
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, player.position);
    }
}
