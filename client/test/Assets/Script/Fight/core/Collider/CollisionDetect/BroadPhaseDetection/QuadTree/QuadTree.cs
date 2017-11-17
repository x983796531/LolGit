using FixedPointy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree  {
    public const int MAX_OBJECTS = 5;
    public const int MAX_LEVELS = 5;
    public List<Rectangle> objects = new List<Rectangle>();
    public QuadTree[] nodes = new QuadTree[4];
    public int level;
    public Rectangle bounds = new Rectangle();
#if test1
    public List<SplitLine> splitLines = new List<SplitLine>();
#endif
    public QuadTree(int level, Rectangle rectangle)
    {
        this.level = level;
        this.bounds = rectangle;
    }

    /*
 * 清空四叉树
 */
    public void clear()
    {
        objects.Clear();
#if test1
        splitLines.Clear();
#endif
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] != null)
            {
                nodes[i].clear();
                nodes[i] = null;
            }
        }
    }
#if test1
    public List<SplitLine> GetSplitLines()
    {

        List<SplitLine> allSplitLine = new List<global::SplitLine>();
        allSplitLine.AddRange(splitLines);

        if (nodes[0] != null)
        {
            for (int i = 0; i < 4; i++)
            {
                allSplitLine.AddRange(nodes[i].GetSplitLines());
            }
        }
        return allSplitLine;
    }
#endif

    public int getIndex(Rectangle pRect)
    {
        int index = -1;
        // 中线
        Fix verticalMidpoint = bounds.getX() + (bounds.getWidth() / 2);
        Fix horizontalMidpoint = bounds.getY() + (bounds.getHeight() / 2);

        // 物体完全位于上面两个节点所在区域
        bool topQuadrant = (pRect.getY() >= horizontalMidpoint);
        // 物体完全位于下面两个节点所在区域
        bool bottomQuadrant = (pRect.getY()+pRect.getHeight() <= horizontalMidpoint);

        // 物体完全位于左面两个节点所在区域
        if (pRect.getX() <= verticalMidpoint && pRect.getX() + pRect.getWidth() <= verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 1; // 处于左上节点 
            }
            else if (bottomQuadrant)
            {
                index = 2; // 处于左下节点
            }
        }
        // 物体完全位于右面两个节点所在区域
        else if (pRect.getX() >= verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 0; // 处于右上节点
            }
            else if (bottomQuadrant)
            {
                index = 3; // 处于右下节点
            }
        }

        return index;
    }

    private void split()
    {
        int subWidth = (int)(bounds.getWidth() / 2);
        int subHeight = (int)(bounds.getHeight() / 2);
        int x = (int)bounds.getX();
        int y = (int)bounds.getY();

        nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        nodes[1] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
        nodes[2] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
        nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
       
        
       

//        SplitLine line1 = new global::SplitLine();
//        line1.lines[0] = new Vector2(x, y + subHeight);
//        line1.lines[1] = new Vector2(x + bounds.width, y + subHeight);
//        SplitLine line2 = new global::SplitLine();
//        line2.lines[0] = new Vector2(x + subWidth, y);
//        line2.lines[1] = new Vector2(x + subWidth, y + bounds.height);
//        line1.SetRandomColor();
//        line2.SetRandomColor();
//#if test1
//        splitLines.Add(line1);
//        splitLines.Add(line2);
//#endif
    }


    public void insert(Rectangle pRect)
    {

        // 插入到子节点
        if (nodes[0] != null)
        {
            int index = getIndex(pRect);

            if (index != -1)
            {
                nodes[index].insert(pRect);

                return;
            }
        }

        // 还没分裂或者插入到子节点失败，只好留给父节点了
        objects.Add(pRect);

        // 超容量后如果没有分裂则分裂
        if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
        {
            if (nodes[0] == null)
            {
                split();
            }
            // 分裂后要将父节点的物体分给子节点们
            int i = 0;
            //List<Rectangle> toRemoveIndex = new List<Rectangle>();
            while (i < objects.Count)
            {
                int index = getIndex(objects[i]);
                if (index != -1)
                {
                    //toRemoveIndex.Add(objects[i]);
                    nodes[index].insert(objects[i]);
                    objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            //foreach (Rectangle item in toRemoveIndex)
            //{
            //    objects.Remove(item);
            //}
        }
    }

    public List<Rectangle> Retrive(Rectangle rect)
    {
        List<Rectangle> result = new List<global::Rectangle>();

        if (nodes[0] != null)
        {
            int index = getIndex(rect);
            if (index != -1)
            {
                result.AddRange(nodes[index].Retrive(rect));
            }
            else
            {
                List<Rectangle> arr = rect.carve(bounds);

                for (int i = 0; i < arr.Count; i++)
                {
                    index = getIndex(arr[i]);
                    result.AddRange(nodes[index].Retrive(rect));
                    try
                    {
                        
                    }
                    catch
                    {

                    }
                }
            }
        }

        result.AddRange(objects);
        return result;
    }

    public void Refresh(QuadTree root)
    {

        try
        {
            //List<Rectangle> objs = new List<Rectangle>();
            //objs = this.objects;
            List<Rectangle> toRemoveRect = new List<Rectangle>();
            for (int i = 0; i < objects.Count; i++)
            {
                Rectangle rect = objects[i];
                int index = getIndex(rect);

                if (!IsInner(rect, bounds))
                {
                    if (this != root)
                    {
                        root.insert(rect);
                        toRemoveRect.Add(rect);
                        //objects.RemoveAt(i);
                    }
                }
                else if (nodes[0] != null && index != -1)
                {
                    nodes[index].insert(rect);
                    toRemoveRect.Add(rect);
                    //objects.RemoveAt(i);
                }

            }

            foreach (Rectangle item in toRemoveRect)
            {
                objects.Remove(item);
            }
            if (nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    nodes[i].Refresh(root);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
        
    }

    private bool IsInner(Rectangle rect,Rectangle bounds)
    {
        return rect.x >= bounds.x &&
        rect.x + rect.width <= bounds.x + bounds.width &&
        rect.y >= bounds.y &&
        rect.y + rect.height <= bounds.y + bounds.height;
    }
}



public class SplitLine
{
    public Color lineColor;
    
    public void SetRandomColor()
    {
        lineColor = new Color(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1));
    }
    public Vector2[] lines = new Vector2[2];
}
