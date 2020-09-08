using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public bool GameReady { get; set; }

    private Network networkController;
    private Vector2 lastInput;
        
    // Start is called before the first frame update
    void Start()
    {
        networkController = GameObject.Find("SocketIO").GetComponent<Network>();
        networkController.onGameReady += OnGameReady;

        lastInput = Vector2.zero;

    }

    void OnGameReady(bool state)
    {
        this.GameReady = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameReady)
        {
            #if UNITY_EDITOR
                AxisInput();
            #else            
                 AccelerometerInput();
            #endif

        }
    }

    void AccelerometerInput()
    {
        Vector2 dir = Vector2.zero;

        dir.x = 0;
        dir.y = Input.acceleration.y;

        if (Math.Abs(dir.y) > 0.3)
            dir.y = dir.y / Math.Abs(dir.y);
        else
            dir.y = 0;

        if (dir != lastInput)
        {
            networkController.SendInput("MOVE", dir);
            lastInput = dir;
        }
    }

    void AxisInput()
    {
        Vector2 dir = Vector2.zero;

        dir.y = Input.GetAxis("Vertical");

        if (Math.Abs(dir.y) > 0.4)
            dir.y = dir.y / Math.Abs(dir.y);
        else
            dir.y = 0;
        if (dir != lastInput)
        {
            networkController.SendInput("MOVE", dir);
            lastInput = dir;
        }
        
    }
}
