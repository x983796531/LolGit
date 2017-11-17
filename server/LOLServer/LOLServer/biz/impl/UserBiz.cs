using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLServer.dao.model;
using NetFrame;
using LOLServer.cache;

namespace LOLServer.biz.impl
{
    /// <summary>
    /// 用户事物处理
    /// </summary>
    public class UserBiz : IUserBiz
    {
        IAccountBiz accBiz = BizFactory.accountBiz;
        IUserCache userCache = CacheFactory.userCache;
        public bool Create(UserToken token, string name)
        {
            //账号是否登陆 获取账号ID
            int accountId = accBiz.get(token);
            if (accountId == -1) return false;
            //判断当前账号是否已经拥有角色
            if (userCache.hasByAccountId(accountId)) return false;
            userCache.create(token, name, accountId);
            return true;
        }

        public USER getByAccount(UserToken token)
        {
            //账号是否登陆 获取账号ID
            int accountId = accBiz.get(token);
            if (accountId == -1) return null;

            return userCache.getByAccountId(accountId);
        }

        public USER get(int id)
        {
            return userCache.get(id);
        }

        public USER online(UserToken token)
        {
            int accountId = accBiz.get(token);
            if (accountId == -1) return null;
            USER user = userCache.getByAccountId(accountId);
            if (userCache.isOnline(user.id)) return null;
            userCache.online(token, user.id);
            return user;
        }

        public void offline(UserToken token)
        {
            userCache.offline(token);
        }

        public UserToken getToken(int id)
        {
            return userCache.getToken(id);
        }

        public USER get(UserToken token)
        {
            return userCache.get(token);
        }

       
        public void addRecord(int userId, int state)
        {
            USER user = get(userId);
            switch(state)
            {
                case 0:
                    user.win += 1;
                    break;
                case 1:
                    user.lose += 1;
                    break;
                default:
                    user.ran += 1;
                    break;
            }

            userCache.Update(user);
        }
    }
}
