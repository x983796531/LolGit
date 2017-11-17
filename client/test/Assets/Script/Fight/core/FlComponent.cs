using UnityEngine;
using System.Collections;

public class FlComponent : FlObject {
    public FlTransform transform
    {
        get
        {
            return GetComponent<FlTransform>();
        }
    }
    public FlGameObject gameObject;
    public T GetComponent<T>()
    {
        return gameObject.GetComponent<T>();
    }
}
