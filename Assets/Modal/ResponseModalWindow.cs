using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseModalWindow : MonoBehaviour
{

    public Action onYes;
    public Action onNo;
    public Action onCancel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetYesAnswer()
    {
        Debug.Log("Set Yes");
        ModalWindow.answer = "yes";
        Debug.Log("Yes clicked");
        if (onYes != null)
            onYes();
    }

    public void SetNoAnswer()
    {
        Debug.Log("Set No");
        ModalWindow.answer = "no";
        Debug.Log("No clicked");
        if (onNo != null)
            onNo();
    }

    public void SetCancelAnswer()
    {
        Debug.Log("Set Cancel");
        ModalWindow.answer = "cancel";
        Debug.Log("No clicked");
        if (onCancel != null)
            onCancel();
    }

    public void FreezeLittosim()
    {
        Debug.Log("Freeze littosim here!");
    }

    public void UnFreezeLittosim()
    {
        Debug.Log("UnFreeze littosim here!");
    }
}
