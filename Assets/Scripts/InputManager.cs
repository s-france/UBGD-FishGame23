using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput pi;
    //public static InputActionMap gameplay;

    public static InputManager Instance {get; private set;}
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

        pi = GetComponent<PlayerInput>();
    }



    public void OnDeviceLost(PlayerInput pi)
    {
        

    }

    public void OnDeviceRegained(PlayerInput pi)
    {

    }

    public void OnControlsChanged(PlayerInput pi)
    {
        Debug.Log("controls changed!");
        Debug.Log("current control scheme: " + pi.currentControlScheme);
    }

}
