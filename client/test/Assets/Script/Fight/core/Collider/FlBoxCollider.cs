using UnityEngine;
using System.Collections;
using FixedPointy;

public class FlBoxCollider : FlCollicer {

    public FixVec2 center;
    public FixVec2 size;
    private MyPolygon myPolygon = new MyPolygon();
    public MyPolygon rect
    {
        get
        {
            
           
            FixVec2 rectCenter = transform.position + transform.right * center.X + transform.forword * center.Y;
            myPolygon.center = rectCenter;
            //myPolygon.lines = new Line[4];
            FixVec2 rtVec = rectCenter + transform.right * size.X + transform.forword * size.Y;
            Point rtP = new Point(rtVec.X, rtVec.Y);

            FixVec2 ltVec = rectCenter + transform.right *-1*size.X + transform.forword * size.Y;
            Point ltP = new Point(ltVec.X, ltVec.Y);

            FixVec2 lbVec = rectCenter + transform.right * -1 * size.X + transform.forword *-1* size.Y;
            Point lbP = new Point(lbVec.X, lbVec.Y);

            FixVec2 rbVec = rectCenter + transform.right * size.X + transform.forword *-1* size.Y;
            Point rbP = new Point(rbVec.X, rbVec.Y);


            Line tLine = new Line(rtVec.X, rtVec.Y, ltVec.X, ltVec.Y);
            

            Line lLine = new Line(ltVec.X, ltVec.Y, lbVec.X, lbVec.Y);

            Line bLine = new Line(lbVec.X, lbVec.Y, rbVec.X, rbVec.Y);

            Line rLine = new Line(rbVec.X, rbVec.Y, rtVec.X, rtVec.Y);

            myPolygon.lines[0] = tLine;
            myPolygon.lines[1] = lLine;
            myPolygon.lines[2] = bLine;
            myPolygon.lines[3] = rLine;
            myPolygon.points[0] = new Point(rtP.X, rtP.Y);
            myPolygon.points[1] = new Point(ltP.X, ltP.Y);
            myPolygon.points[2] = new Point(lbP.X, lbP.Y);
            myPolygon.points[3] = new Point(rbP.X, rbP.Y);
            return myPolygon;
        }
    }

    public override Rectangle GetRectangle()
    {
        return rect.GetRectangle();
    }
}
