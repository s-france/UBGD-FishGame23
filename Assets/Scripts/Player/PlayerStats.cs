using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //list of player states:
    ///grounded
    ///airborn
    ///wallsliding
    ///grappling (will probably need to differentiate air/ground too)
    ///

    public IPlayerState[] stateList;

    //JUMPING
    [SerializeField] public int midairJumps;
    [SerializeField] public float jumpStrength;
    [SerializeField] public float coyoteTime;

    //GROUNDED MOVEMENT
    [SerializeField] public float groundMaxSpeed;
    [SerializeField] public float groundAcceleration;
    [SerializeField] public float groundDecceleration;

    //AIR MOVEMENT
    [SerializeField] public float playerGravity;
    [SerializeField] public float airMaxSpeed;
    [SerializeField] public float airAcceleration;
    [SerializeField] public float airDecceleration;

    //FISH HOOK GRAPPLING
    [SerializeField] public float grappleLaunchStrength;
    [SerializeField] public float airGrapplePullStrength;
    [SerializeField] public float maxGrappleDistance;
    [SerializeField] public float grappleJumpStrength;
    [SerializeField] public float grappleSlideMinSpeed;
    [SerializeField] public float grappleSlideMaxSpeed;
    [SerializeField] public float grappleSlideAcceleration;
    [SerializeField] public float grappleSlideDecceleration;
    [SerializeField] public float grappleLiftOffAngle;
    

    void Awake()
    {
        //SIZE SUBJECT TO CHANGE
        stateList = new IPlayerState[5];
        stateList[0] = GetComponentInChildren<InactiveState>();
        stateList[1] = GetComponentInChildren<GroundedState>();
        stateList[2] = GetComponentInChildren<AirbornState>();
        stateList[3] = GetComponentInChildren<GroundedGrappleState>();
        stateList[4] = GetComponentInChildren<AirGrappleState>();
    }
    
}
