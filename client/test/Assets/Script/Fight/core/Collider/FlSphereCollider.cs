using UnityEngine;
using System.Collections;
using FixedPointy;

public class FlSphereCollider : FlCollicer {

    //public FixVec2 center ;
    public Fix radius;
    private Circle2D _circle2D = new Circle2D();
    public Circle2D circle2D
    {
        get
        {
            _circle2D.center = transform.position;

            _circle2D.radius = this.radius;
            return _circle2D;
        }
    }
}
