using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.dao.model;

namespace LOLServer.cache.impl
{
    public class AccountCache : IAccountCache
    {
       

        /// <summary>
        /// 玩家连接对象与账号的映射绑定
        /// </summary>
        Dictionary<UserToken, string> onLineAccMap = new Dictionary<UserToken, string>();

        /// <summary>
        /// 账号与自身具体属性的映射绑定
        /// </summary>
        Dictionary<string, ACCOUNT> accMap = new Dictionary<string, ACCOUNT>();

        public bool hasAccount(string account)
        {
            init(account);
            return accMap.ContainsKey(account);
        }

        public bool match(string account, string password)
        {
            //判断账号是否存在 不存在就谈不上匹配了
            if (!hasAccount(account)) return false;
            //获取账号信息 判断密码是否匹配并返回
            return accMap[account].password.Equals(Convert.ToBase64String(Encoding.UTF8.GetBytes(password)));
        }

        public bool isOnLine(string account)
        {
            //判断当前在线字典中 是否有此账号 没有则说明不在线
            return onLineAccMap.ContainsValue(account);
        }

        public int getId(UserToken token)
        {
            //判断在线字典中是否有此连接的记录 没有则说明此连接没有登陆 无法获取账号id
            if (!onLineAccMap.ContainsKey(token)) return -1;
            //返回绑定账号的id
            return accMap[onLineAccMap[token]].id;
        }

        public void onLine(UserToken token, string account)
        {
            //添加映射
            onLineAccMap.Add(token, account);
        }

        public void offLine(UserToken token)
        {
            //如果当前连接有登陆 进行移除
            if (onLineAccMap.ContainsKey(token)) onLineAccMap.Remove(token);
        }

        public void add(string account, string password)
        {
            //创建账号实体进行绑定
            ACCOUNT model = new ACCOUNT();
            model.account = account;
            model.password =Convert.ToBase64String( Encoding.UTF8.GetBytes(password));
            model.Add();
            accMap.Add(account, model);

           
        }

        public void init(string account)
        {
            if (accMap.ContainsKey(account)) return;
            ACCOUNT ACC = new dao.model.ACCOUNT(account);
            if(ACC.id>=0)
            {
                accMap.Add(account, ACC);
            }
        }
    }
}
