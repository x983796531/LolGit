using UnityEngine;
using System.Collections;
using FixedPointy;

public class CircleBoxDetectImpl  {
    #region test
    public Transform pointA, pointB, circle;
    public Transform rectBox, point;


    private void Update()
    {
        // bool result = LineCircleIntersect(new Vector2(pointA.position.x, pointA.position.z), new Vector2(pointB.position.x, pointB.position.z), new Vector2(circle.position.x, circle.position.z), circle.localScale.x / 2);

        //Debug.Log(result);

        //bool pointInrect = PointInRect(rectBox, point);
        //Debug.Log(pointInrect);

        //bool circleRectInter = CircleRectDetect(rectBox, circle);
       // Debug.Log(TestCirBoxDetect(rectBox, circle));
        //Debug.Log(circleRectInter);
    }

    static Circle2D GetCircle2d(Transform circle)
    {
        Circle2D circle2d = new global::Circle2D(new FixVec2(circle.position.x.ToFix(), circle.position.z.ToFix()), (circle.lossyScale.x / 2).ToFix());
        return circle2d;
    }

    static bool TestCirBoxDetect(Transform box, Transform circle)
    {
        MyPolygon poly = GetBoxPoly(box);
        Circle2D circle2d = GetCircle2d(circle);
        bool result = CircleRectDetect(poly, circle2d);
        return result;
    }

    #endregion


    public static bool CircleRectDetect(MyPolygon polygon, Circle2D circle)
    {
        if (PointInRect(polygon, circle))
            return true;
        Point[] recPoints = polygon.points;


        if (LineCircleIntersect(new FixVec2(recPoints[0].X, recPoints[0].Y), new FixVec2(recPoints[1].X, recPoints[1].Y), circle.center, circle.radius))
            return true;

        if (LineCircleIntersect(new FixVec2(recPoints[1].X, recPoints[1].Y), new FixVec2(recPoints[2].X, recPoints[2].Y), circle.center, circle.radius))
            return true;

        if (LineCircleIntersect(new FixVec2(recPoints[2].X, recPoints[2].Y), new FixVec2(recPoints[3].X, recPoints[3].Y), circle.center, circle.radius))
            return true;

        if (LineCircleIntersect(new FixVec2(recPoints[0].X, recPoints[0].Y), new FixVec2(recPoints[3].X, recPoints[3].Y), circle.center, circle.radius))
            return true;

        if (rectInCircle(recPoints, circle))
            return true;


        return false;

    }

    static bool rectInCircle(Point[] boxPoint, Circle2D circle)
    {
        FixVec2 circleCenter = circle.center;

        Fix radius = circle.radius;

        for (int i = 0; i < 4; i++)
        {
            if ((new FixVec2(boxPoint[i].X, boxPoint[i].Y)- circleCenter).GetMagnitude() > radius)
                return false;
        }
        return true;
    }

    static bool LineCircleIntersect(FixVec2 L, FixVec2 E, FixVec2 C, Fix r)
    {
        FixVec2 d = L - E;
        FixVec2 f = E - C;

        //float a = d.Dot(d);
        //float b = 2 * f.Dot(d);
        //float c = f.Dot(f) - r * r;


        Fix a = d.Dot(d);
        Fix b = 2 * f.Dot(d);
        Fix c = f.Dot(f) - r * r;

        Fix discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return false;
        }
        else
        {
            // ray didn't totally miss sphere,
            // so there is a solution to
            // the equation.

            discriminant = FixMath.Sqrt(discriminant);

            // either solution may be on or off the ray so need to test both
            // t1 is always the smaller value, because BOTH discriminant and
            // a are nonnegative.
            Fix t1 = (-b - discriminant) / (2 * a);
            Fix t2 = (-b + discriminant) / (2 * a);

            // 3x HIT cases:
            //          -o->             --|-->  |            |  --|->
            // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

            // 3x MISS cases:
            //       ->  o                     o ->              | -> |
            // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

            if (t1 >= 0 && t1 <= 1)
            {
                // t1 is the intersection, and it's closer than t2
                // (since t1 uses -b - discriminant)
                // Impale, Poke
                return true;
            }

            // here t1 didn't intersect so we are either started
            // inside the sphere or completely past it
            if (t2 >= 0 && t2 <= 1)
            {
                // ExitWound
                return true;
            }

            // no intn: FallShort, Past, CompletelyInside
            return false;
        }
    }

    static bool PointInRect(MyPolygon box, Circle2D circle)
    {
        Point[] recPoint = box.points;
        bool result = isContain(recPoint[0], recPoint[1], recPoint[2], recPoint[3], new Point(circle.center.X, circle.center.Y));
        return result;

    }

    //public bool isContain(Vector2 mp1, Vector2 mp2, Vector2 mp3, Vector2 mp4, Vector2 mp)
    //{
    //    if (Multiply(mp, mp1, mp2) * Multiply(mp, mp4, mp3) <= 0

    //        && Multiply(mp, mp4, mp1) * Multiply(mp, mp3, mp2) <= 0)
    //        return true;

    //    return false;
    //}
    //// 计算叉乘 |P0P1| × |P0P2|
    //private double Multiply(Vector2 p1, Vector2 p2, Vector2 p0)
    //{
    //    return ((p1.x - p0.x) * (p2.y - p0.y) - (p2.x - p0.x) * (p1.y - p0.y));
    //}

    static bool isContain(Point mp1, Point mp2, Point mp3, Point mp4, Point mp)
    {
        if (Multiply(mp, mp1, mp2) * Multiply(mp, mp4, mp3) <= 0

            && Multiply(mp, mp4, mp1) * Multiply(mp, mp3, mp2) <= 0)
            return true;

        return false;
    }
    // 计算叉乘 |P0P1| × |P0P2|
    static Fix Multiply(Point p1, Point p2, Point p0)
    {
        return ((p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y));
    }


    //public bool isContain(Point mp1, Point mp2, Point mp3, Point mp4, Point mp)
    //{
    //    Fix fix1 = Multiply(mp, mp1, mp2);
    //    float q = (float)fix1;
    //    float a = testMultiply(new Vector2((float)mp.X, (float)mp.Y), new Vector2((float)mp1.X, (float)mp1.Y), new Vector2((float)mp2.X, (float)mp2.Y));

    //    Fix fix2 = Multiply(mp, mp4, mp3);
    //    Fix fix3 = Multiply(mp, mp4, mp1);
    //    Fix fix4 = Multiply(mp, mp3, mp2);
    //    if (fix1 * fix2 <= 0.ToFix()

    //        && fix3 * fix4 <= 0.ToFix())
    //        return true;

    //    return false;
    //}
    //// 计算叉乘 |P0P1| × |P0P2|
    //private Fix Multiply(Point p1, Point p2, Point p0)
    //{
    //    Fix a = p1.X - p0.X;
    //    Fix b = (p2.Y - p0.Y);
    //    Fix c = p2.X - p0.X;
    //    Fix d = p1.Y - p0.Y;
    //    Fix f = a * b;
    //    Fix g = c * d;
    //    Fix h = f - g;
    //    Fix e = a * b - c * d;
    //    return (((p1.X - p0.X) * (p2.Y - p0.Y)) - ((p2.X - p0.X) * (p1.Y - p0.Y)));
    //}

    //private float testMultiply(Vector2 p1,Vector2 p2,Vector2 p0)
    //{
    //    return ((p1.x - p0.x) * (p2.y- p0.y) - (p2.x - p0.x) * (p1.y - p0.y));
    //}


    //test
    static MyPolygon GetBoxPoly(Transform box1)
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
        rec[0] = rt1;
        rec[1] = lt1;
        rec[2] = lb1;
        rec[3] = rb1;
        
        

        MyPolygon poly = new global::MyPolygon();
        poly.points = rec;
        poly.lines[0] = new global::Line(rt1.X, rt1.Y, lt1.X, lt1.Y);
        poly.lines[1] = new global::Line(lt1.X, lt1.Y, lb1.X, lb1.Y);
        poly.lines[2] = new global::Line(lb1.X, lb1.Y, rb1.X, rb1.Y);
        poly.lines[3] = new global::Line(rb1.X, rb1.Y, rt1.X, rt1.Y);

        poly.center = new FixVec2(box1.position.x.ToFix(), box1.position.z.ToFix());
        return poly;
    }

   
}
