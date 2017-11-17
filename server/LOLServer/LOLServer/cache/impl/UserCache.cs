using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLServer.dao.model;
using NetFrame;

namespace LOLServer.cache.impl
{
    public class UserCache : IUserCache
    {
        /// <summary>
        /// 用户id和模型的映射表
        /// </summary>
        Dictionary<int, USER> idToModel = new Dictionary<int, USER>();

        /// <summary>
        /// 账号ID 和角色ID的绑定
        /// </summary>
        Dictionary<int, int> accToUid = new Dictionary<int, int>();

        Dictionary<int, UserToken> idToToken = new Dictionary<int, UserToken>();
        Dictionary<UserToken, int> tokenToId = new Dictionary<UserToken, int>();
        int index = 0;

        public bool create(UserToken token, string name,int accountId)
        {
            USER user = new USER();
            user.name = name;
            
            user.accountId = accountId;
            List<int> list = new List<int>();
            for(int i=1;i<9;i++)
            {
                list.Add(i);
            }
            user.heroList = list.ToArray();
            user.Add();
            //创建成功 进行账号ID和用户ID的绑定
            accToUid.Add(accountId, user.id);
            //创建成功 进行用户id和用户模型的绑定
            idToModel.Add(user.id, user);
            return true;
        }

        public bool has(UserToken token)
        {
            return tokenToId.ContainsKey(token);
        }

        public bool hasByAccountId(int id)
        {
            init(id);
            return accToUid.ContainsKey(id);
        }

        public USER get(UserToken token)
        {
            if (!has(token)) return null;

            return idToModel[tokenToId[token]];
        }

        public USER get(int id)
        {
            return idToModel[id];
        }

        public void offline(UserToken token)
        {
            if(tokenToId.ContainsKey(token))
            {
                if(idToToken.ContainsKey(tokenToId[token]))
                {
                    idToToken.Remove(tokenToId[token]);
                }
                tokenToId.Remove(token);
            }
        }

        public UserToken getToken(int id)
        {
            return idToToken[id];
        }

        public USER online(UserToken token, int id)
        {
            idToToken.Add(id, token);
            tokenToId.Add(token, id);
            return idToModel[id];
        }

        public USER getByAccountId(int accId)
        {
            init(accId);
            if ((!accToUid.ContainsKey(accId))) return null;
            return idToModel[accToUid[accId]];
        }

        public bool isOnline(int id)
        {
            return idToToken.ContainsKey(id);
        }

        public void init(int accountId)
        {

            if (accToUid.ContainsKey(accountId)) return;
            USER user = new USER(accountId);
            if (user.id >= 0)
            {
                //获取成功 进行帐号ID和用户ID的绑定
                accToUid.Add(accountId, user.id);
                //获取成功 进行用户ID和用户模型的绑定
                idToModel.Add(user.id, user);
            }
        }

        public void Update(USER user)
        {
            user.Update();
            idToModel[user.id] = user;
        }
    }
}
