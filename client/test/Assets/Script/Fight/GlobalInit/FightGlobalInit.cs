using UnityEngine;
using System.Collections;

public class FightGlobalInit  {
    private static FightGlobalInit instance;
    public float inputSendInterval;
    public static FightGlobalInit Instance
    {
        get
        {
            if (instance == null)
            {
                instance =new  FightGlobalInit();
            }
            return instance;
        }
    }

    FightGlobalInit()
    {
        inputSendInterval = 0.05f;
    }
}
