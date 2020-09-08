using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;

public delegate void MessageDelegate(string mensagge);
public delegate void BoolDelegate(bool state);

public delegate void InputDelegate(string playerId, string command);

public class Network : MonoBehaviour
{

    SocketIOController socket;

    public event MessageDelegate onConnectedToServer;
    public event BoolDelegate onJoinedRoom;
    public event BoolDelegate onGameReady;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        socket = gameObject.GetComponent<SocketIOController>();
    }

    public void ConnectedToServer()
    {

        socket.On("onConnection", OnConnection);
        socket.On("joinedToRoom", JoinedToRoom);
        socket.On("gameReady", GameReady);
        socket.Connect();

    }

   

    void OnConnection(SocketIOEvent evt)
    {
        // FormatField(evt.data);
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        Debug.Log(data.message);
        onConnectedToServer(data.message);

    }

    void JoinedToRoom(SocketIOEvent evt)
    {
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        onJoinedRoom(data.state);
    }

    void GameReady(SocketIOEvent evt)
    {
        JsonData data = JsonUtility.FromJson<JsonData>(evt.data);
        onGameReady(data.state);
    }

    public void JoinRoom(string room)
    {
        JsonData data = new JsonData();
        data.room = room;
        socket.Emit("joinRoom", JsonUtility.ToJson(data));
    }

    public void SetReady()
    {
        socket.Emit("playerReady");
    }

    public void SendInput(string command)
    {
        PlayerInputData data = new PlayerInputData();
        data.command = command;
        socket.Emit("playerInput", JsonUtility.ToJson(data));
    }
    public void SendInput(string command,Vector2 dir)
    {
        PlayerInputData data = new PlayerInputData();
        data.command = command;
        data.axisHorizontal = dir.x;
        data.axisVertical = dir.y;
        socket.Emit("playerInput", JsonUtility.ToJson(data));
    }


}

class JsonData
{
    public string message;
    public string room;
    public bool state;
}
public class PlayerInputData
{
    public string command;
    public float axisHorizontal;
    public float axisVertical;
}


