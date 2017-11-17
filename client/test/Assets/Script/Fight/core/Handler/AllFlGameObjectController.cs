using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol.dto.fight;

public class AllFlGameObjectController  {

    private static AllFlGameObjectController instance;
    Dictionary<int, FlGameObject> allFlGameObject = new Dictionary<int, FlGameObject>();
    public static AllFlGameObjectController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AllFlGameObjectController();
            }
            return instance;
        }
    }

    AllFlGameObjectController()
    {
        allFlGameObject = FlGameObjectFactory.instance.allFlGameObject;
    }

    public void SendAllFlGameObjectMessage(AllClientOp ao)
    {
        //if (ao.allClientOp!= null)
        //{
            //Debug.Log(ao);
            foreach (FlGameObject fg in allFlGameObject.Values)
            {
                FlPlayer fp = fg.GetComponent<FlPlayer>();
                if (fp != null)
                {
                    //fp.Update(ao);
                }
            }
        //}
        
    }

}
