using UnityEngine;
using System.Collections;
using GameProtocol;

public class HeartBeat : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("send", 30, 30);
	}
	
    void send()
    {
        this.WriteMessage(Protocol.HEART_BEAT, 0, 0, null);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
