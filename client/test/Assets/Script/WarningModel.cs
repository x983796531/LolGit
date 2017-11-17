using UnityEngine;
using System.Collections;

public delegate void WarrningResult();
public class WarningModel : MonoBehaviour {

    public WarrningResult result;
    public string value;
    public float delay;

    public WarningModel(string value,WarrningResult result=null,float delay=-1)
    {
        this.value = value;
        this.result = result;
        this.delay = delay;
    }
}
