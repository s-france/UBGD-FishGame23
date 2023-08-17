using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vc;
    Transform player;

    // Start is called before the first frame update
    void Awake()
    {
        vc = GetComponentInChildren<CinemachineVirtualCamera>();
        player = GameObject.Find("Player").transform;

        vc.m_Follow = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
