using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
    public interface IAccountBiz
    {
        /// <summary>
        /// 账号创建
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>返回创建结果  0 成功 1 账号重复 2账号不合法 3 密码不合法</returns>
        int create(UserToken token, string account, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>登陆结果 0 成功  -1 账号不存在  -2 账号在线 -3 密码错误 -4输入不合法</returns>
        int login(UserToken token, string account, string password);

        /// <summary>
        /// 客户端下线（下线）
        /// </summary>
        /// <param name="token"></param>
        void close(UserToken token);

        /// <summary>
        /// 获取账号ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回用户的登录账号ID</returns>
        int get(UserToken token);
    }
}
