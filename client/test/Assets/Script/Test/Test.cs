using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using GameProtocol;
using System;
using System.Collections.Generic;
using System.Reflection;
using FixedPointy;

public class Test : MonoBehaviour {

    public bool test;
    public static Test instance;
    public GameObject a, b;

    
    private void Awake()
    {
        instance = this;
    }
    int count = 0;
    public GameObject GetGameObject()
    {
        count++;
        if (count == 1)       
            return a;
        else
            return b;
        
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            //send();
            //get();
            //TestCombination();
            //testRef();
            TestFix();
        }
    }



    void TestFix()
    {
        Fix m = new Fix(2);
        int n = m.Raw;
        Fix a = 2f.ToFix();
        float b = a.toFloat();
        int c = a.Raw;

        float v= Mathf.Pow(2, 2);
        Fix q = new Fix(2);
        //Fix i = new Fix();
        //i= q * q;
        //Fix w = FixMath.Pow(q, 2f.ToFix());
        //Fix r = FixMath.Sqrt(w);
        
        Debug.Log("q:"+(float)q/*+ " w:" + w+" r:"+r.toFloat()+" v:"+v+" i:"+i*/);
        
    }

    void send()
    {
        ClientOp op = new ClientOp();
        op.clientId = 1;
        //op.op = "1,2";
        this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.op, op);
    }

    void get()
    {
       Debug.Log(  GetComponent<BoxCollider>());
    }


    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="strLength">字符串长度</param>
    /// <param name="Seed">随机函数种子值</param>
    /// <returns>指定长度的随机字符串</returns>
    public string GetString(int strLength, params int[] Seed)
    {
        string strSep = ",";
        char[] chrSep = strSep.ToCharArray();
        string strChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z"
         + ",A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] aryChar = strChar.Split(chrSep, strChar.Length);
        string strRandom = string.Empty;
        System.Random Rnd;
        if (Seed != null && Seed.Length > 0)
        {
            Rnd = new System.Random(Seed[0]);
        }
        else
        {
            Rnd = new System.Random();
        }
        //生成随机字符串
        for (int i = 0; i < strLength; i++)
        {
            strRandom += aryChar[Rnd.Next(aryChar.Length)];
        }
        return strRandom;
    }

   

    /// <summary>
    /// 递归算法求数组的组合(私有成员)
    /// </summary>
    /// <param name="list">返回的范型</param>
    /// <param name="t">所求数组</param>
    /// <param name="n">辅助变量</param>
    /// <param name="m">辅助变量</param>
    /// <param name="b">辅助数组</param>
    /// <param name="M">辅助变量M</param>
    private static void GetCombination(ref List<int[]> list, int[] t, int n, int m, int[] b, int M)
    {
        for (int i = n; i >= m; i--)
        {
            b[m - 1] = i - 1;
            if (m > 1)
            {
                GetCombination(ref list, t, i - 1, m - 1, b, M);
            }
            else
            {
                if (list == null)
                {
                    list = new List<int[]>();
                }
                int[] temp = new int[M];
                for (int j = 0; j < b.Length; j++)
                {
                    temp[j] = t[b[j]];
                }
                list.Add(temp);
            }
        }
    }

   

   
    void TestCombination()
    {
        string[] a = new string[] { "1", "2", "3", "4","5", "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5" , "1", "2", "3", "4", "5"  };
        Debug.Log(Time.realtimeSinceStartup);
        var result= Combinations(a, 2);
        Debug.Log(Time.realtimeSinceStartup);
        int qa = 0;
        foreach (var x in result)
        {
            string q = "";
            foreach (var y in x)
            {
                q += y;
            }
           // Debug.Log(q);
            qa++;
        }
        Debug.Log(qa);

    }

    IEnumerable<List<string>> Combinations(string[] characters, int length)
    {
        return Combinations(characters, 0, characters.Length - 1, length);
    }

     IEnumerable<List<string>> Combinations(string[] characters, int start, int end, int length)
    {
        if (end < start)
            yield break;
        else if (length == 1)
            for (var i = start; i <= end; i++)
                yield return new List<string> { characters[i] };
        else
        {
            foreach (var r in Combinations(characters, start, end - 1, length))
                yield return r;
            foreach (var r in Combinations(characters, start, end - 1, length - 1))
            {
                r.Add(characters[end]);
                yield return r;
            }
        }
    }


    void testRef()
    {
        FlGameObject a= CreateInstance< FlGameObject>("Assembly-CSharp",  "FlGameObject");
        a.tag = "124";
        Debug.Log(a.tag);
    }


    public static T CreateInstance<T>(string assemblyName, string className)
    {
        try
        {
            string fullName = className;//命名空间.类型名
                                                          //此为第一种写法
            object ect = Assembly.Load(assemblyName).CreateInstance(fullName);//加载程序集，创建程序集里面的 命名空间.类型名 实例
            return (T)ect;//类型转换并返回
                          //下面是第二种写法
                          //string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
                          //Type o = Type.GetType(path);//加载类型
                          //object obj = Activator.CreateInstance(o, true);//根据类型创建实例
                          //return (T)obj;//类型转换并返回
        }
        catch
        {
            //发生异常，返回类型的默认值
            return default(T);
        }
    }

}
