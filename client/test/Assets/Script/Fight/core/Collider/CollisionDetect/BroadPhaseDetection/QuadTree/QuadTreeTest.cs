using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeTest : MonoBehaviour {
    public QuadTree root = new QuadTree(0, new Rectangle(0, 0, 50, 50));
    public List<Rectangle> allRects = new List<Rectangle>();

    public static QuadTreeTest instance;
    public  Material m;
    List<SplitLine> splitLines = new List<SplitLine>();
    public bool initFinished = false;
    private void Awake()
    {
        instance = this;
    }
    void Start () {
        StartCoroutine(Init());
	}
	
    IEnumerator Init()
    {
        yield return new WaitForSeconds(1);
        foreach (Rectangle rect in allRects)
        {
            root.insert(rect);
        }
        initFinished = true;
    }
	
	void Update () {
        if (initFinished)
        {
            //foreach (Rectangle rect in allRects)
            //{
            //    root.insert(rect);
            //}
            //foreach (Rectangle rect in allRects)
            //{
            //    rect.tr.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1);
            //}

            List<Rectangle> nearRect = root.Retrive(allRects[0]);
            //foreach (Rectangle rect in nearRect)
            //{
            //    rect.tr.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
            //}


            //splitLines = root.GetSplitLines();


            //root.clear();

            root.Refresh(root);
        }

       
	}

    void OnPostRender()
    {
        foreach(SplitLine line in splitLines)
        {
            DrawLine(line);
        }
    }

    void DrawLine(SplitLine line)
    {
       
        GL.PushMatrix();//保存摄像机变换矩阵
        m.SetPass(0);
        //GL.LoadPixelMatrix();

        GL.Begin(GL.LINES);

        //GL.Color(line.lineColor);

        GL.Vertex3(line.lines[0].x, 0, line.lines[0].y);
        GL.Vertex3(line.lines[1].x, 0, line.lines[1].y);

        GL.End();

        GL.PopMatrix();//恢复摄像机投影矩阵
    }
}
