using UnityEngine;
using System.Collections;
using FixedPointy;

public class BoxDetectImpl
{
    public Transform box1, box2;
    #region test
    public Point[] GetBoxPoint(Transform box1)
    {
        float harfBox1Wid = box1.localScale.x / 2;
        float harfBox1Hei = box1.localScale.z / 2;
        Vector3 pointLeftTopVec3 = -box1.right.normalized * harfBox1Wid + box1.forward.normalized * harfBox1Hei;
        Vector2 pointLeftTopVec = new Vector2(pointLeftTopVec3.x, pointLeftTopVec3.z);

        Vector3 pointRightTopVec3 = box1.right.normalized * harfBox1Wid + box1.forward.normalized * harfBox1Hei;
        Vector2 pointRightTopVec = new Vector2(pointRightTopVec3.x, pointRightTopVec3.z);

        Vector2 lt1Vec = new Vector2(box1.position.x, box1.position.z) + pointLeftTopVec;
        Point lt1 = new Point();
        lt1.X = lt1Vec.x.ToFix();
        lt1.Y = lt1Vec.y.ToFix();

        Vector2 rb1Vec = new Vector2(box1.position.x, box1.position.z) - pointLeftTopVec;
        Point rb1 = new Point();
        rb1.X = rb1Vec.x.ToFix();
        rb1.Y = rb1Vec.y.ToFix();



        Vector2 rt1Vec = new Vector2(box1.position.x, box1.position.z) + pointRightTopVec;
        Point rt1 = new Point();
        rt1.X = rt1Vec.x.ToFix();
        rt1.Y = rt1Vec.y.ToFix();

        Vector2 lb1Vec = new Vector2(box1.position.x, box1.position.z) - pointRightTopVec;
        Point lb1 = new Point();
        lb1.X = lb1Vec.x.ToFix();
        lb1.Y = lb1Vec.y.ToFix();

        Point[] rec = new Point[4];
        rec[1] = lt1;
        rec[3] = rb1;
        rec[0] = rt1;
        rec[2] = lb1;

        return rec;
    }

    MyPolygon GetMyPolygon(Transform box)
    {
        MyPolygon myPolygon = new global::MyPolygon();
        myPolygon.lines = GetLines(GetBoxPoint(box));
        myPolygon.center = new Vector2(box.position.x, box.position.z).toFixVec2();
        return myPolygon;
    }


    private void Update()
    {
        Test();
    }
    public void Test()
    {
        bool ifCollision = BoxCollisionDetect(GetMyPolygon(box1), GetMyPolygon(box2));
         Debug.Log(ifCollision);
    }

    #endregion

    static Line[] GetLines(Point[] points)
    {
        Line[] Line = new global::Line[4];

        for (int i = 0; i < 4; i++)
        {
            if (i < 3)
            {
                Line newLine = new Line();
                newLine.X1 = points[i].X;
                newLine.Y1 = points[i].Y;
                newLine.X2 = points[i + 1].X;
                newLine.Y2 = points[i + 1].Y;
                Line[i] = newLine;
            }
            else
            {
                Line newLine2 = new Line();
                newLine2.X1 = points[i].X;
                newLine2.Y1 = points[i].Y;
                newLine2.X2 = points[0].X;
                newLine2.Y2 = points[0].Y;
                Line[i] = newLine2;
            }

        }

        return Line;

    }


    static Line[] GetLines2(Point[] points)
    {
        Line[] Line = new global::Line[6];

        for (int i = 0; i < 4; i++)
        {
            if (i < 3)
            {
                Line newLine = new Line();
                newLine.X1 = points[i].X;
                newLine.Y1 = points[i].Y;
                newLine.X2 = points[i + 1].X;
                newLine.Y2 = points[i + 1].Y;
                Line[i] = newLine;
            }
            else
            {
                Line newLine2 = new Line();
                newLine2.X1 = points[i].X;
                newLine2.Y1 = points[i].Y;
                newLine2.X2 = points[0].X;
                newLine2.Y2 = points[0].Y;
                Line[i] = newLine2;
            }

        }

        return Line;

    }

    public static bool BoxCollisionDetect(MyPolygon poly1, MyPolygon poly2)
    {
        bool intersect = false;
        foreach (Line line1 in poly1.lines)
        {
            foreach (Line line2 in poly2.lines)
            {
                intersect = DoIntersect(new Point(line1.X1, line1.Y1), new Point(line1.X2, line1.Y2), new Point(line2.X1, line2.Y1), new Point(line2.X2, line2.Y2));

                if (intersect)
                {
                    return true;
                }
            }
        }

        Fix inState = GetInPoly(poly1, poly2);
        if (inState != -1)
        {
            //poly1MayInPoly2
            if (inState == 0)
            {
                bool confirmPoly1InPoly2 = ConfirmInRelation(poly1, poly2);
                if (confirmPoly1InPoly2)
                    return true;
            }
            //poly2MayInPoly1
            else
            {
                bool confirmPoly2InPoly1 = ConfirmInRelation(poly2, poly1);
                if (confirmPoly2InPoly1)
                    return true;
            }
        }



        return false;
    }

    static bool ConfirmInRelation(MyPolygon poly1, MyPolygon poly2)
    {
        Line line = new global::Line();
        line.X1 = poly1.center.X;
        line.Y1 = poly1.center.Y;

        line.X2 = poly2.center.X;
        line.Y2 = poly2.center.Y;

        foreach (Line line1 in poly2.lines)
        {
            bool intersect = DoIntersect(new Point(poly1.center.X, poly1.center.Y), new Point(poly2.center.X, poly2.center.Y), new Point(line1.X1, line1.Y1), new Point(line1.X2, line1.Y2));
            if (intersect)
                return false;
        }

        return true;
    }

    static Fix GetPolyLineMinLength(MyPolygon poly1)
    {
        Fix polyMinLength = FixMath.Min( (new FixVec2(poly1.lines[0].X1, poly1.lines[0].Y1)- new FixVec2(poly1.lines[0].X2, poly1.lines[0].Y2)).GetMagnitude(), (new FixVec2(poly1.lines[1].X1, poly1.lines[1].Y1)-new FixVec2(poly1.lines[1].X2, poly1.lines[1].Y2)).GetMagnitude());
        return polyMinLength;
    }

    static Fix GetPolyLineMaxLength(MyPolygon poly1)
    {
        Fix polyMinLength = FixMath.Max((new FixVec2(poly1.lines[0].X1, poly1.lines[0].Y1)- new FixVec2(poly1.lines[0].X2, poly1.lines[0].Y2)).GetMagnitude(), (new FixVec2(poly1.lines[1].X1, poly1.lines[1].Y1)- new FixVec2(poly1.lines[1].X2, poly1.lines[1].Y2)).GetMagnitude());
        return polyMinLength;
    }

    static Fix GetInPoly(MyPolygon poly1, MyPolygon poly2)
    {
        Fix poly1MinLineLength = GetPolyLineMinLength(poly1);
        Fix poly1MaxLineLength = GetPolyLineMaxLength(poly1);

        Fix poly2MinLineLength = GetPolyLineMinLength(poly2);
        Fix poly2MaxLineLength = GetPolyLineMaxLength(poly2);

        bool poly1CanInPoly2 = poly1MaxLineLength < poly2MinLineLength;
        bool poly2CanInPoly1 = poly2MaxLineLength < poly1MinLineLength;
        if (poly1CanInPoly2)
            return 0;
        if (poly2CanInPoly1)
            return 1;

        return -1;
    }

    //private static bool MyNarrowPhaseDetection(MyPolygon poly1, MyPolygon poly2)
    //{
    //    bool intersect = false;
    //    foreach (Line line1 in poly1.lines)
    //    {
    //        foreach (Line line2 in poly2.lines)
    //        {
    //            intersect = DoIntersect(new Point(line1.X1, line1.Y1), new Point(line1.X2, line1.Y2), new Point(line2.X1, line2.Y1), new Point(line2.X2, line2.Y2));

    //            if (intersect)
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}


    /// <summary>
    /// This code was found here: http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="q1"></param>
    /// <param name="p2"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    private static bool DoIntersect(Point p1, Point q1, Point p2, Point q2)
    {
        // Find the four orientations needed for general and
        // special cases
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);

        // General case
        if (o1 != o2 && o3 != o4)
            return true;

        // Special Cases
        // p1, q1 and p2 are colinear and p2 lies on segment p1q1
        if (o1 == 0 && onSegment(p1, p2, q1))
            return true;

        // p1, q1 and p2 are colinear and q2 lies on segment p1q1
        if (o2 == 0 && onSegment(p1, q2, q1))
            return true;

        // p2, q2 and p1 are colinear and p1 lies on segment p2q2
        if (o3 == 0 && onSegment(p2, p1, q2))
            return true;

        // p2, q2 and q1 are colinear and q1 lies on segment p2q2
        if (o4 == 0 && onSegment(p2, q1, q2))
            return true;

        return false; // Doesn't fall in any of the above cases
    }

    /// <summary>
    /// This code was found here: http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    /// </summary>
    /// <param name="p"></param>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private static int orientation(Point p, Point q, Point r)
    {
        // See http://www.geeksforgeeks.org/orientation-3-ordered-points/
        // for details of below formula.
        Fix val = ((q.Y - p.Y) * (r.X - q.X) -
                  (q.X - p.X) * (r.Y - q.Y));

        if (val == 0) return 0;  // colinear

        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }

    /// <summary>
    /// This code was found here: http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
    /// </summary>
    /// <param name="p"></param>
    /// <param name="q"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private static bool onSegment(Point p, Point q, Point r)
    {
        if (q.X <= FixMath.Max(p.X, r.X) && q.X >= FixMath.Min(p.X, r.X) &&
            q.Y <= FixMath.Max(p.Y, r.Y) && q.Y >= FixMath.Min(p.Y, r.Y))
            return true;

        return false;
    }

    
}
