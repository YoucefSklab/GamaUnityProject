using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseModalWindow : MonoBehaviour
{
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
    }

    public void SetNoAnswer()
    {
        Debug.Log("Set No");
        ModalWindow.answer = "no";
    }

    public void SetCancelAnswer()
    {
        Debug.Log("Set Cancel");
        ModalWindow.answer = "cancel";
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
