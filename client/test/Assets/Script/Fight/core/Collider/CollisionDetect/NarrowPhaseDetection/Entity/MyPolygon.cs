using FixedPointy;

public class MyPolygon
{
    public Line[] lines = new Line[4];
    public FixVec2 center=new FixVec2();

    public Point[] points=new Point[4];

    public Rectangle GetRectangle()
    {
        Fix minX = points[0].X;
        Fix maxX = points[0].X;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].X < minX)
            {
                minX = points[i].X;
            }
            if (points[i].X > maxX)
                maxX = points[i].X;
        }
        Fix minY = points[0].Y;
        Fix maxY = points[0].Y;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].Y < minY)
                minY = points[i].Y;
            if (points[i].Y > maxY)
                maxY = points[i].Y;
        }

        Rectangle rectangle = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        return rectangle;
        
    }
    
    
}