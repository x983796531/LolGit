using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans
{
    public class EquData
    {
        public static readonly Dictionary<int, EquModel> equMap = new Dictionary<int, EquModel>();

        static EquData()
        {
            create(1, 10, 1, 100, "新手砍刀", 6666);
        }

        static void create(int code,int atk,int def,int hp,string name, int price)
        {
            EquModel equ = new constans.EquModel();
            equ.code = code;
            equ.atk = atk;
            equ.def = def;
            equ.hp = hp;
            equ.name = name;
            equ.price = price;
            equMap.Add(code, equ);

        }
    }

    public partial class EquModel
    {
        public int atk;
        public int def;
        public int hp;
        public int code;
        public int price;
        public string name;
    }
}
