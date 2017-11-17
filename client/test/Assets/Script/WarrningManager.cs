using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarrningManager : MonoBehaviour
{

    public static List<WarningModel> errors = new List<WarningModel>();

    [SerializeField]
    private WarringWindow window;
    // Update is called once per frame
    void Update()
    {
        if(errors.Count>0)
        {
            WarningModel err = errors[0];
            errors.RemoveAt(0);
            window.active(err);
        }
    }
}
