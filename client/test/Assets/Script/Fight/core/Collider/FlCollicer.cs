using UnityEngine;
using System.Collections;
using FixedPointy;
using System.Collections.Generic;

public class FlCollicer : FlComponent {
    public bool enabled;
    public List<FlGameObject> contactGameObject = new List<FlGameObject>();
    public virtual Rectangle GetRectangle()
    {
        return null;
    }
}
