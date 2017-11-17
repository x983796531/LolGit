using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
    public class ClientOp
    {
        public byte clientId;
        public Dictionary<int, string> op;

        public override string ToString()
        {
            string opStr = " | clientId:"+clientId+" ,";
            foreach(int i in op.Keys)
            {
                opStr += i + ":" + op[i];
            }
            opStr += "| ";
            return "clientId:" + clientId + ", op:" + opStr;
        }
    }
}
