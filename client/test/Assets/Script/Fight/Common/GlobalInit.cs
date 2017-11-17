using UnityEngine;
using System.Collections;

public class GlobalInit {
    private static GlobalInit _instance;
    public int randomSeed;
	public static GlobalInit instace
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalInit();
            }
            return _instance;
        }
    }
}
