using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class SpriteSlider : MonoBehaviour
{
    [SerializeField]
    private Transform front;

    public float Value
    {
        get { return front.localScale.x; }         
        set
        {
            front.localScale = new Vector3(value, 1, 1);           
        }
    }

   


}
