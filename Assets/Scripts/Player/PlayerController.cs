using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerAnimationController pac;
    GameplayPlayerController gpc;
    [HideInInspector] public ILevelController lc; //updates handled by levelcontrollers, //only referenced in regards to respawn points (pc does not start/end levels)
    PlayerInput pi;
    Rigidbody2D rb;
    BoxCollider2D col;

    public static PlayerController Instance {get; private set;}
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else 
        {
            Destroy(gameObject);
        }

        pac = GetComponentInChildren<PlayerAnimationController>();
        gpc = GetComponent<GameplayPlayerController>();
        pi = GameObject.Find("InputManager").GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        if(lc == null)
        {
            lc = GameObject.Find("LevelController").GetComponent<ILevelController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnPlayer(Transform spawnpoint)
    {
        gpc.ChangeState(1); //grounded state
        pi.transform.parent.position = spawnpoint.position;
    }

    public void KillPlayer()
    {
        gpc.ChangeState(0);

        //run some dying animation here

        RespawnPlayer(lc.GetCurrentCP());
    }

    //for when menus are opened
    void DeactivatePlayer()
    {

    }

    void ReactivatePlayer()
    {

    }
}
