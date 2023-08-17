using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelController : MonoBehaviour, ILevelController
{
    LevelManager lm;
    GameplayPlayerController gpc;

    [SerializeField] int levelID;


    void Awake()
    {
        Debug.Log("MAIN MENU LEVEL AWAKE!!");

        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lm.lc = this;
        gpc = GameObject.Find("Player").GetComponent<GameplayPlayerController>();

        StartLevel();
    }

    public void StartLevel()
    {
        gpc = GameObject.Find("Player").GetComponent<GameplayPlayerController>();
        gpc.cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        ResetLevel();
        Debug.Log("Main Menu started!");
    }
    public void EndLevel()
    {
        //closing/transition animations (if not handled in LevelManager)
        Debug.Log("Main Menu ended!");
    }

    
    public void ResetLevel()
    {
        //close possible sub menus
        //reset cursor/selection position
        //run opening animations
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Transform> GetCheckpoints()
    {
        return null;
    }

    public Transform GetCurrentCP()
    {
        return null;
    }
}
