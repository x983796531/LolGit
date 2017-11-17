using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using LOLServer.tool;
using System.Collections.Concurrent;

namespace LOLServer.logic.select
{
    public class SelectHandler : AbsOnceHandler, HandlerInterface
    {
        /// <summary>
        /// 多线程处理中  防止数据竞争导致脏数据 使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// 房间id与模型映射
        /// </summary>
        ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();

        /// <summary>
        /// 回收利用过的房间再次利用，减少gc性能开销
        /// </summary>
        ConcurrentStack<SelectRoom> cache = new ConcurrentStack<SelectRoom>();

        /// <summary>
        /// 房间id自增器
        /// </summary>
        ConCurrentInteger index = new ConCurrentInteger();

        public SelectHandler ()
        {
            EventUtil.createSelect = Create;
            EventUtil.destroySelect = destroy;
        }

        public void Create(List<int> teamOne, List<int> teamTwo)
        {
            SelectRoom room;
            if(!cache.TryPop(out room))
            {
                room = new select.SelectRoom();
                //添加唯一ID
                room.SetArea(index.GetAndAdd());
            }          
            //房间数据初始化
            room.Init(teamOne, teamTwo);
            //绑定映射关系
            foreach (int item in teamOne)
            {
                userRoom.TryAdd(item, room.GetArea());
            }
            
            foreach (int item in teamTwo)
            {
                userRoom.TryAdd(item, room.GetArea());
            }

            roomMap.TryAdd(room.GetArea(), room);
        }

        public void destroy(int roomId)
        {
            SelectRoom room;
            if(roomMap.TryRemove(roomId,out room))
            {
                //移除角色和房间之间的绑定关系
                int temp;
                foreach (int item in room.teamOne.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }

                foreach (int item in room.teamTwo.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                room.list.Clear();
                room.readList.Clear();
                room.teamOne.Clear();
                room.teamTwo.Clear();
                
                //将房间放入缓存队列 供下次选择使用
                cache.Push(room);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            int userId = getUserId(token);
            if(userRoom.ContainsKey(userId))
            {
                int roomId;
                userRoom.TryRemove(userId, out roomId);
                if(roomMap.ContainsKey(roomId))
                {
                    roomMap[roomId].ClientClose(token, error);
                }
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            int userId = getUserId(token);
            if(userRoom.ContainsKey(userId))
            {
                int roomId = userRoom[userId];
                if(roomMap.ContainsKey(roomId))
                {
                    roomMap[roomId].MessageReceive(token, message);
                }
            }
        }
    }
}
