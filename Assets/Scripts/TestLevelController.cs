using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelController : MonoBehaviour, ILevelController
{
    LevelManager lm;
    PlayerController pc;
    GameplayPlayerController gpc;

    [SerializeField] int levelID;
    List<Transform> checkpoints;

    [HideInInspector] public Transform currentCP;


    void Awake()
    {
        Debug.Log("TEST LEVEL AWAKE!!");

        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lm.lc = this;
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        gpc = GameObject.Find("Player").GetComponent<GameplayPlayerController>();


        int CPcount = 1;
        Transform check = transform.Find("Checkpoints/CP" + CPcount);

        if(check != null) 
        {
            checkpoints = new List<Transform>();
            
            while (check != null)
            {
                if(check != null)
                {
                    Debug.Log("got CP" + CPcount);
                }

                checkpoints.Add(check);

                CPcount++;

                check = transform.Find("Checkpoints/CP" + CPcount);
            }

            //TEMPORARY
            currentCP = checkpoints[2];
        }
        
        StartLevel();
    }

    //resets all level elements
    public void ResetLevel()
    {
        //reset camera, flags, and level elements

        //respawn player
        pc.RespawnPlayer(currentCP);
    }

    //runs at start of level
    public void StartLevel()
    {
        gpc = GameObject.Find("Player").GetComponent<GameplayPlayerController>();
        gpc.cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        ResetLevel();
    }

    //runs at end of level
    public void EndLevel()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Transform> GetCheckpoints()
    {
        return checkpoints;
    }
    public Transform GetCurrentCP()
    {
        return currentCP;
    }
}
