using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarringWindow : MonoBehaviour {
    [SerializeField]
    private Text text;

    WarrningResult result;

    public void active(WarningModel value)
    {
        text.text = value.value;
        this.result = value.result;
        if(value.delay>0)
        {
            Invoke("Close", value.delay);
        }
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (IsInvoking("Close")) CancelInvoke("Close");
        gameObject.SetActive(false);
        if(result!=null)
        {
            result();
        }
    }
}
