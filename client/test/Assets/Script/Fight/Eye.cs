using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 挂载在敌方单位
/// </summary>
public class Eye : MonoBehaviour {

    List<GameObject> list = new List<GameObject>();
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject title;
    [SerializeField]
    private GameObject root;

	// Update is called once per frame
	void Update () {
	
        if(list.Count>0)
        {
            //是否反隐
            //是否敌方单位
            if(!head.activeSelf)
            {
                head.SetActive(true);
            }
            if(!title.activeSelf)
            {
                title.SetActive(true);
            }
            if(!root.activeSelf)
            {
                root.SetActive(true);
            }

        }
        else
        {
            if (head.activeSelf)
            {
                head.SetActive(false);
            }
            if (title.activeSelf)
            {
                title.SetActive(false);
            }
            if (root.activeSelf)
            {
                root.SetActive(false);
            }
        }
	}

    private void OnTriggerEnter(Collider c)
    {
        PlayerCon con = c.GetComponentInParent<PlayerCon>();

        if(con)
        {
            if(con.data.team!=GetComponentInParent<PlayerCon>().data.team)
            {
                list.Add(c.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if(list.Contains(c.gameObject))
        {
            list.Remove(c.gameObject);
        }
    }

}
