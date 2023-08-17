using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

//handles loading / switching between levels
public class LevelManager : MonoBehaviour
{
    [HideInInspector] public ILevelController lc;

    public static LevelManager Instance {get; private set;}
    void Awake()
    {
        Debug.Log("LEVEL MANAGER AWAKE!!");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else 
        {
            Destroy(gameObject);
        }

        if(lc == null)
        {
            lc = GameObject.Find("LevelController").GetComponent<ILevelController>();
        }
        //lc.StartLevel();
    }

    public async void LoadScene(string sceneName)
    {
        lc.EndLevel();
        

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do {
            //loading stuff
            //TEMPORARY:
            Debug.Log("loading " + sceneName + "...");
            await Task.Delay(50);

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;


        //lc = GameObject.Find("LevelController").GetComponent<ILevelController>();
        //lc.StartLevel();
    }

}
