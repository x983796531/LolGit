using UnityEngine;
using System.Collections;

public class AddSelf : MonoBehaviour {

    public void init(Transform parent,float delta)
    {
        Invoke("remove", delta);
    }

    private void OnTriggerEnter(Collider c)
    {
        //技能逻辑处理
    }

   void remove()
    {
        Destroy(gameObject);
    }
}
