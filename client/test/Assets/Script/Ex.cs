using FixedPointy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Ex
{
    /// <summary>
    /// 扩展monobehaviour 发送消息方法
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public static void WriteMessage(this MonoBehaviour mono, byte type, int area, int command, object message)
    {
        NetIO.Instance.write(type, area, command, message);
    }

    public static Fix ToFix(this float f)
    {
        Fix a = new Fix();
        a = (int)(f * 1000);
        Fix b = a / 1000;
        return b;
    }

    public static Fix ToFix(this int i)
    {
        Fix a = new Fix();
        a = i;
        return a;
    }

    public static float toFloat(this Fix fix)
    {
        return (float)fix;
    }

    public static Vector2 toVector2(this FixVec2 fixVec)
    {
        return new Vector2(fixVec.X.toFloat(), fixVec.Y.toFloat());
    }

    public static FixVec2 toFixVec2(this Vector2 v)
    {
        return new FixVec2(v.x.ToFix(), v.y.ToFix());       
    }
}
