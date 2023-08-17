using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InactiveState : MonoBehaviour, IPlayerState
{
    GameplayPlayerController gpc;
    PlayerStats pStats;
    PlayerAnimationController ac;
    PlayerInput pi;
    Rigidbody2D rb;


    void Awake()
    {
        gpc = GetComponentInParent<GameplayPlayerController>();
        pStats = GetComponentInParent<PlayerStats>();
        ac = transform.parent.GetComponentInChildren<PlayerAnimationController>();
        rb = GetComponentInParent<Rigidbody2D>();

        pi = GameObject.Find("InputManager").GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartState()
    {
        Debug.Log("Deactivating player");
        //reset+hide sprite
        ac.ChangeSprite(ac.groundSprites[0]);
        ac.HidePlayer();
        //disable movement
        pi.DeactivateInput();
        //reset all variables
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        transform.parent.position = Vector3.zero;
    }


    public void EndState()
    {
        Debug.Log("Reactivating player");
        //hide sprite
        ac.ShowPlayer();
        //disable movement
        pi.ActivateInput();
        //reset all variables
        rb.gravityScale = pStats.playerGravity;
    }


    public void Move(Vector2 i_move)
    {
        Debug.Log("can't move! player is inactive!");
    }


    public void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log("can't jump! player is inactive!");
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        Debug.Log("can't grapple! player is inactive!");
    }
}
