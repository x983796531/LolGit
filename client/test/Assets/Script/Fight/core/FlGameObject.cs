using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlGameObject : FlObject {

    public int id;
    public string name;

    public bool active;
    public int layer;
    public string tag;
    public FlTransform transform;
    public FlComponent boxCollider;
    public FlComponent sphereCollider;

    private List<FlComponent> allFlBehaviour=new List<FlComponent>();

    public List<FlComponent> GetAllBehavour()
    {
        return allFlBehaviour;
    }

    public void HandleCollision(List<FlGameObject> collisionObjs)
    {
        int[] indextArray = Utils.GenerateRandomIndexArray(allFlBehaviour.Count);

    }

    public bool hasBehaviour
    {
        get
        {
            return allFlBehaviour.Count > 0 ? true : false;
        }
    }

    public bool hasCollision
    {
        get
        {
            if (sphereCollider != null || boxCollider != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public T GetComponent<T>()
    {
        try
        {
            if (typeof(T) == typeof(FlTransform))
            {
                return (T)(transform as object);
            }

            if (typeof(T) == typeof(FlBoxCollider))
            {
                return (T)(boxCollider as object);
            }         

            if (typeof(T) == typeof(FlSphereCollider))
            {
                return (T)(sphereCollider as object);
            }
            foreach(FlComponent fc in allFlBehaviour)
            {
                if(typeof(T) == fc.GetType())
                {
                    return (T) (fc as object);
                }
            }
            return default(T);
        }
        catch
        {
            return default(T);
        }
        
        
    }

    public void AddComponent(FlComponent componentType)
    {
        allFlBehaviour.Add(componentType);
    }



}
