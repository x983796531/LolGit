using UnityEngine;
using System.Collections;
using System;
using GameProtocol;
using UnityEngine.SceneManagement;

public class MatchHandler : MonoBehaviour,IHandler {
    public void MessageReceive(SocketModel model)
    {
        if(model.command==MatchProtocol.ENTER_SELECT_BRO)
        {
            SceneManager.LoadScene("Select");
        }
    }

   
}
