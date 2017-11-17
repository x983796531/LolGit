using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.cache;

namespace LOLServer.biz.impl
{
    public class AccountBiz : IAccountBiz
    {

        IAccountCache accountCache = CacheFactory.accountCache;

        public int create(UserToken token, string account, string password)
        {
            if (accountCache.hasAccount(account)) return 1;
            accountCache.add(account, password);
            return 0;
        }

        public int login(UserToken token, string account, string password)
        {
            //账号密码为空 输入不合法
            if (account == null || password == null) return -4;
            //判断账号是否存在  不存在则无法登陆
            if (!accountCache.hasAccount(account)) return -1;
            //判断此账号当前是否在线
            if (accountCache.isOnLine(account)) return -2;
            //判断账号密码是否匹配
            if (!accountCache.match(account, password)) return -3;
            //验证都通过 说明可以登陆 调用上线方法并返回成功
            accountCache.onLine(token, account);
            return 0;
        }

        public void close(UserToken token)
        {
            accountCache.offLine(token);
        }

        public int get(UserToken token)
        {
            return accountCache.getId(token);
        }
    }
}
