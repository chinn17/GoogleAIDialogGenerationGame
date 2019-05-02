using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class GameMenuSetup : MonoBehaviour
{

    GameObject waitingPopup;
    GameObject joinPopup;
    // Start is called before the first frame update
    void Awake()
    {
        waitingPopup = GameObject.Find("Waiting Popup");
        joinPopup = GameObject.Find("Join Popup");
    }

    void Start()
    {
        SetWaitingPopup(false);
        SetJoinPopup(false);
    }

    public void SetWaitingPopup(bool givenBool)
    {
        waitingPopup.SetActive(givenBool);
    }

    public void SetJoinPopup(bool givenBool)
    {
        joinPopup.SetActive(givenBool);
    }
    // Update is called once per frame

    public void ConnectToServer()
    {
        string toSend = "register/%username=test%password=test%email=test@test.com%";

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(Constant.Server), 4343);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(serverAddress);

        // Sending
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
        clientSocket.Send(toSendLenBytes);
        clientSocket.Send(toSendBytes);

        // Receiving
        byte[] rcvLenBytes = new byte[4];
        clientSocket.Receive(rcvLenBytes);
        int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
        byte[] rcvBytes = new byte[rcvLen];
        clientSocket.Receive(rcvBytes);
        string rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

        Debug.Log("Client received: " + rcv);

        clientSocket.Close();
    }

    void Update()
    {

    }
}
