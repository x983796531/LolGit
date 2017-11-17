using UnityEngine;
using System.Collections;

public class NumUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("remove", 2);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime);
	}

    void remove()
    {
        Destroy(gameObject);
    }
}
