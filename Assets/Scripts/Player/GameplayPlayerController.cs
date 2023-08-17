using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayPlayerController : MonoBehaviour
{
    [HideInInspector] public Camera cam; //updates handled in LevelManager

    PlayerInput pi;
    PlayerController pc;
    IPlayerState pState;
    PlayerStats pStats;
    Rigidbody2D rb;

    [HideInInspector] public Vector2 i_move;
    [HideInInspector] public Vector2 i_aim;

    //all updates handled in GroundCheck script
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isOnSlope;
    [HideInInspector] public Vector2 slopeNormalPerp;
    [HideInInspector] public float airTime;
    [HideInInspector] public float groundTime;

    //FOR TESTING PURPOSES
    [HideInInspector] public string activeSlopeCheck;


    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isGSliding;

    void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        pi = GetComponentInChildren<PlayerInput>();
        pc = GetComponent<PlayerController>();
        pStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();

        i_move = Vector2.zero;

        ChangeState(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(pi.currentControlScheme == "Keyboard+Mouse")
        {
            Vector2 pos = cam.WorldToScreenPoint(transform.position);

            i_aim = ( (Vector2)Input.mousePosition - pos).normalized;

            Debug.DrawRay(transform.position, i_aim, Color.black);
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("KillPlane"))
        {
            pc.KillPlayer();
        }
    }

    public void ChangeState(int state)
    {
        if(pState != null) {pState.EndState();}
        pState = pStats.stateList[state];
        pState.StartState();
        //Debug.Log("player state changed to state " + state + "!");
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        i_move = ctx.ReadValue<Vector2>();
        //Debug.Log("Move action performed!");
        pState.Move(i_move);
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        if(pi.currentControlScheme == "Controller")
        {
            i_aim = ctx.ReadValue<Vector2>();

            Debug.DrawRay(transform.position, i_aim, Color.black);
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Jump action performed!");
        pState.Jump(ctx);
    }

    public void OnSpecial(InputAction.CallbackContext ctx)
    {
        pState.Grapple(ctx);
    }

    public void OnL(InputAction.CallbackContext ctx)
    {
        //Debug.Log("L action performed!");
    }

    public void OnR(InputAction.CallbackContext ctx)
    {
        //Debug.Log("R action performed!");
    }

    public void OnStart(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Start action performed!");
        if(ctx.performed) //pressed
        {
            
        } else if (ctx.canceled) //released
        {
            
        }
    }

}
