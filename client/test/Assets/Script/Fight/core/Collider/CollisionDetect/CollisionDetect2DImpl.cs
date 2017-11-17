using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

public class CollisionDetect2DImpl : ICollisionDetect2D
{
    public QuadTree root = new QuadTree(0, new Rectangle(0, 0, 100, 100));

    public Dictionary<int, Rectangle> allRectDic = new Dictionary<int, Rectangle>();
    public List<Rectangle> allRectList = new List<Rectangle>();

    private MethodInfo onCollisionEnterMethodInfo = typeof(FlBehaviour).GetMethod("OnCollisionEnter", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo onCollisionStayMethodInfo = typeof(FlBehaviour).GetMethod("OnCollisionStay", BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo onCollisionExitMethodInfo = typeof(FlBehaviour).GetMethod("OnCollisionExit", BindingFlags.NonPublic | BindingFlags.Instance);
    public void InitAllRect(List<FlGameObject> allFlGameObject)
    {
        
    }

    public void Init(List<FlGameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            allRectList.Add(gameObjects[i].GetComponent<FlCollicer>().GetRectangle());
        }
    }

    public void AddRect(Rectangle rect)
    {
        allRectDic.Add(rect.collider.gameObject.id, rect);
    }

    //public void DoCollisionDetect()
    //{

    //}

    public void DoCollisionDetect()
    {
        int[] allId = new int[allRectDic.Count];
        int index = 0;
        foreach (int i in allRectDic.Keys)
        {
            allId[index] = i;
            index++;
        }
        //id随机
        Utils.GenerateRandomSquence(allId);
        for (int i = 0; i < allId.Length; i++)
        {
            int id = allId[i];
            FlGameObject curCalculateGameObject = allRectDic[id].collider.gameObject;
            if (curCalculateGameObject.hasBehaviour)
            {
                List<Rectangle> collisionRects = root.Retrive(allRectDic[id]);
                collisionRects.Remove(allRectDic[id]);

                for (int j = 0; j < collisionRects.Count; j++)
                {


                    if (allRectDic[id].collider.contactGameObject.Contains(collisionRects[j].collider.gameObject))
                    {
                        allRectDic[id].collider.contactGameObject.Remove(collisionRects[j].collider.gameObject);
                        //stay
                        onCollisionStayMethodInfo.Invoke(curCalculateGameObject, new object[] { collisionRects[j].collider.gameObject });
                    }
                    else
                    {
                        allRectDic[id].collider.contactGameObject.Remove(collisionRects[j].collider.gameObject);
                        //enter
                        onCollisionEnterMethodInfo.Invoke(curCalculateGameObject, new object[] { collisionRects[j].collider.gameObject });
                    }

                    for (int k = 0; k < allRectDic[id].collider.contactGameObject.Count; k++)
                    {
                        //exit
                        onCollisionExitMethodInfo.Invoke(curCalculateGameObject, new object[] { allRectDic[id].collider.contactGameObject[k] });
                    }
                    allRectDic[id].collider.contactGameObject.Clear();
                    for (int m = 0; m < collisionRects.Count; m++)
                    {
                        allRectDic[id].collider.contactGameObject.Add(collisionRects[m].collider.gameObject);
                    }
                   
                }
                allRectDic[id].collider.gameObject.HandleCollision(allRectDic[id].collider.contactGameObject);
            }

        }

    }



    public void RemoveRect(Rectangle rect)
    {
        allRectDic.Remove(rect.collider.gameObject.id);
    }
}
