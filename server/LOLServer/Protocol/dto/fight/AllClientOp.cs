using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
    public class AllClientOp
    {
        public int frameCount;
        public Dictionary<int,ClientOp> allClientOp;

        public override string ToString()
        {
            string a = frameCount+ "{";

            foreach(int index in allClientOp.Keys)
            {
                a += "[index=" + index+"---" + allClientOp[index].ToString() + "],";
            }
           
           
            a += "}";
            return a;
        }
    }
}
