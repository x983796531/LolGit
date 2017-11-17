using FixedPointy;
using System.Collections.Generic;

public class Rectangle
{

    public FlCollicer collider;

    public Rectangle()
    {

    }

    public Rectangle(Fix x, Fix y, Fix width, Fix height)
    {
        this.x = x;
        this.y = y; 
        this.width = width;
        this.height = height;

    }

    public List<Rectangle> carve(Rectangle bounds)
    {
        List<Rectangle> rectList = new List<Rectangle>();

        // 中线
        Fix verticalMidpoint = bounds.getX() + (bounds.getWidth() / 2);
        Fix horizontalMidpoint = bounds.getY() + (bounds.getHeight() / 2);

        // 物体完全位于上面两个节点所在区域
        bool topQuadrant = (this.getY() >= horizontalMidpoint);
        // 物体完全位于下面两个节点所在区域
        bool bottomQuadrant = (this.getY() + this.getHeight() <= horizontalMidpoint);
        //完全右面
        bool fullyRight = this.getX() >= verticalMidpoint;
        //完全左面
        bool fullyLeft = getX() <= verticalMidpoint && getX() + getWidth() <= verticalMidpoint;
        //只纵跨两象限
        if (!topQuadrant && !bottomQuadrant && (fullyLeft || fullyRight))
        {
            Rectangle topRectangle = new Rectangle(x, y, width, horizontalMidpoint - y);
            Rectangle bottomRectangle = new Rectangle(x, horizontalMidpoint, width, y + height - horizontalMidpoint);
            rectList.Add(topRectangle);
            rectList.Add(bottomRectangle);
        }

        //只横跨两象限
        if (!fullyLeft && !fullyRight && (topQuadrant || bottomQuadrant))
        {
            Rectangle leftRectangle = new Rectangle(x, y, verticalMidpoint - x, height);
            Rectangle rightRectangle = new Rectangle(verticalMidpoint, y, x + width - verticalMidpoint, height);

            rectList.Add(leftRectangle);
            rectList.Add(rightRectangle);
        }

        if (!topQuadrant && !bottomQuadrant && !fullyLeft && !fullyRight)
        {
            Rectangle rightTopRect = new Rectangle(verticalMidpoint, horizontalMidpoint, x + width - verticalMidpoint, y + height - horizontalMidpoint);
            Rectangle leftTopRect = new Rectangle(x, horizontalMidpoint, verticalMidpoint - x, y + height - horizontalMidpoint);
            Rectangle leftBottomRect = new Rectangle(x, y, verticalMidpoint - x, horizontalMidpoint - y);
            Rectangle rightBottom = new Rectangle(verticalMidpoint, y, x + width - verticalMidpoint, horizontalMidpoint - y);

            rectList.Add(rightTopRect);
            rectList.Add(leftTopRect);
            rectList.Add(leftBottomRect);
            rectList.Add(rightBottom);
        }
        return rectList;
    }

    public Fix x;

    public Fix y;

    public Fix width;

    public Fix height;


    public Fix getX()
    {
        return x;
    }

    public Fix getWidth()
    {
        return width;
    }

    public Fix getY()
    {
        return y;
    }

    public Fix getHeight()
    {
        return height;
    }
}