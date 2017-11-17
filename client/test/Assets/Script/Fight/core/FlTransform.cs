
using FixedPointy;
using System.Collections;
using UnityEngine;

public class FlTransform : FlComponent {

    public FixVec2 position;
    private FixVec2 _forword;

    public FixVec2 right
    {
        get
        {
            FixVec3 forwordVec3 = new FixVec3(_forword.X, _forword.Y, 0);
            FixVec3 up = new FixVec3(0, 0, 0);
            FixVec3 right = forwordVec3.Cross(up);
            return new FixVec2(right.X, right.Y).Normalize();  
        }
    }

    public FixVec2 left
    {
        get
        {
            FixVec3 forwordVec3 = new FixVec3(_forword.X, _forword.Y, 0);
            FixVec3 up = new FixVec3(0, 0, 0);
            FixVec3 left = up.Cross(forwordVec3);
            return new FixVec2(left.X, left.Y).Normalize();
        }
    }

    public FixVec2 forword
    {
        get
        {
            return _forword.Normalize();
        }
        set
        {
            _forword = value.Normalize();
        }
    }

    public void Translate(Fix speed)
    {
        position += forword.Normalize()*speed;
    }

    public void Rotate(FixVec2 newDirection)
    {
        forword = newDirection;
    }

    public Vector2 GetFloatPosition()
    {
        return position.toVector2();
    }

    public Vector2 GetFloatDirection()
    {
        return forword.toVector2();
    }
}
