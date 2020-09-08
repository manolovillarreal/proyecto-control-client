using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControlManager : MonoBehaviour
{
    Network networkController;

    // Start is called before the first frame update
    void Start()
    {

        networkController = GameObject.Find("SocketIO").GetComponent<Network>();

    }


    public void SetReady()
    {
        networkController.SetReady();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
