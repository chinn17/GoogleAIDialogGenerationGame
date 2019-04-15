using System.Collections;
using System.Collections.Generic;
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
    void Update()
    {
        
    }
}
