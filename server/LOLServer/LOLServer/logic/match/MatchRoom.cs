using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.logic.match
{
    /// <summary>
    /// 战斗匹配房间模型
    /// </summary>
    public class MatchRoom
    {
        public int id;//房间唯一id
        public int teamMax = 1;//每支队伍需要匹配的人数
        public List<int> teamOne = new List<int>();//队伍1 人员id
        public List<int> teamTwo = new List<int>();//队伍2 人员id
    }
}
