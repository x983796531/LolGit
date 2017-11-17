using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto.fight
{
    [Serializable]
    public class AttackDTO
    {
        public int userId;//攻击者ID
        public int target;//被攻击者ID
    }
}
