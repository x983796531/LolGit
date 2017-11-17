using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 动画状态机表
/// </summary>
public class AnimState
{
    public const int IDLE = 0;
    public const int RUN = 1;
    public const int ATTACK = 2;
    public const int SKILL = 3;
    public const int DEAD = 4;
}