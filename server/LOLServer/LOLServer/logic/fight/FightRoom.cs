using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using GameProtocol.dto;
using GameProtocol.dto.fight;
using GameProtocol.constans;
using GameProtocol;
using LOLServer.tool;
using LOLServer.biz;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections;

namespace LOLServer.logic.fight
{
    class FightRoom : AbsMulitHandler, HandlerInterface
    {

        public Dictionary<int, AbsFightModel> teamOne = new Dictionary<int, AbsFightModel>();
        public Dictionary<int, AbsFightModel> teamTwo = new Dictionary<int, AbsFightModel>();

        List<int> herosId = new List<int>();

        private ConCurrentInteger enterCount;

        private List<int> off = new List<int>();

        IAccountBiz accountBiz = BizFactory.accountBiz;

        private int heroCount;

     

        private int monsterId = -21;

        //Thread syncThread;
        // BlockingCollection<ClientOp> clientOpCache = new BlockingCollection<ClientOp>();
        //List<ClientOp> clientOpCache = new List<ClientOp>();
        ArrayList clientOpCache = new ArrayList();
        
        Timer sendTimer;
        public int frameCount;
        public void init(SelectModel[] teamOne, SelectModel[] teamTwo) 
        {
            
            enterCount = new ConCurrentInteger(teamOne.Count() + teamTwo.Count());

            heroCount = teamOne.Count() + teamTwo.Count();
            off.Clear();
            //初始化英雄数据
            foreach (var item in teamOne)
            {
                herosId.Add(item.userId);
                this.teamOne.Add(item.userId, create(item, 1));
            }
            foreach (var item in teamTwo)
            {
                herosId.Add(item.userId);
                this.teamTwo.Add(item.userId, create(item,2));
            }

            ///实例化队伍一的建筑
            ///预留ID段 -1到-10为队伍一建筑
            for (int i=-1; i >= -3;i--)
            {
                this.teamOne.Add(i, createBuild(i, Math.Abs(i),1));
            }

            ///实例化队伍二的建筑
            ///预留ID段 -11到-20为队伍一建筑
            for (int i = -11; i >= -13; i--)
            {
                this.teamTwo.Add(i, createBuild(i, Math.Abs(i)-10,2));
            }
        }


        private FightBuildModel createBuild(int id,int code,int team)
        {
            BuildDataModel data = BuildData.buildMap[code];
            FightBuildModel model = new FightBuildModel(id, code, data.hp, data.hp, data.atk, data.def, data.reborn, data.rebornTime, data.initiative, data.infrared, data.name);
            model.type = ModelType.BUILD;
            model.team = team;
            return model;
        }
         
        private FightPlayerModel create(SelectModel model,int team)
        {
            FightPlayerModel player = new FightPlayerModel();
            player.id = model.userId;
            player.code = model.hero;
            player.type = ModelType.HUMAN;
            player.name = getUser(model.userId).name;
            player.exp = 0;
            player.level = 1;
            player.free = 1;
            player.money = 0;
            player.team = team;

            //从配置表里 取出对应的英雄数据
            HeroDataModel data = HeroData.heroMap[model.hero];
            player.hp = data.hpBase;
            player.maxHp = data.hpBase;
            player.atk = data.atkBase;
            player.def = data.defBase;
            player.aSpeed = data.aSpeed;
            player.speed = data.speed;
            player.aRange = data.range;
            player.eyeRange = data.eyeRange;
            player.skills = initSkill(data.skills);
            player.equs = new int[3];

            return player;
        }


        private FightSkill[] initSkill(int [] value)
        {
            FightSkill[] skills = new FightSkill[value.Length];

            for(int i=0;i<value.Length;i++)
            {
                int skillCode = value[i];
                SkillDataModel data = SkillData.skillMap[skillCode];
                SkillLevelData levelData = data.levels[0];
                FightSkill skill = new FightSkill(skillCode, 0, levelData.level, levelData.time, data.name, levelData.range, data.info, data.target, data.type);
                skills[i] = skill;
            }
            return skills;
        }
        public int refreshId = -1;

        private void refreshMonster()
        {
            refreshId= ScheduleUtil.Instance.schedule(delegate
            {
                List<FightMonsterModel> list = new List<FightMonsterModel>();
                FightMonsterModel monster = new FightMonsterModel();
                monster.id = monsterId;
                monster.name = "队伍一士兵";
                monster.team = 1;
                teamOne.Add(monster.id, monster);
                list.Add(monster);

               
                monster = new FightMonsterModel();
                monster.id = monsterId;
                monster.name = "队伍二士兵";
                monster.team = 2;
                teamTwo.Add(monster.id, monster);
                list.Add(monster);
                brocast(FightProtocol.REFRESH_BRO, list.ToArray());
                refreshMonster();
            },30*1000);
        }

        

        public void ClientClose(UserToken token, string error)
        {
            leave(token);
            int userId = getUserId(token);
            if(teamOne.ContainsKey(userId)||teamTwo.ContainsKey(userId))
            {
                if(!off.Contains(userId))
                {
                    off.Add(userId);
                }
            }

            ExecutorPool.Instance.execute(
            delegate ()
            {
                accountBiz.close(token);
            }
            );


            if (off.Count==heroCount)
            {
                if(refreshId!=-1)
                {
                    ScheduleUtil.Instance.removeMisson(refreshId);
                }
                EventUtil.destoryFight(GetArea());
                StopSyncThread();
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch(message.command)
            {
                case FightProtocol.ENTER_CREQ:
                    enter(token);
                    break;
                case FightProtocol.MOVE_CREQ:
                    move(token,message.GetMessage<MoveDTO>());
                    break;
                case FightProtocol.ATTACK_CREQ:
                    attack(token, message.GetMessage<int>());
                    break;
                case FightProtocol.DAMAGE_CREQ:
                    damage(token, message.GetMessage<DamageDTO>());
                    break;
                case FightProtocol.SKILL_UP_CREQ:
                    skillUp(token, message.GetMessage<int>());              
                    break;
                case FightProtocol.SKILL_CREQ:
                    skill(token, message.GetMessage<SkillAtkModel>());
                    break;
                case FightProtocol.GET_EQU_CREQ:
                    getEqu(token, message.GetMessage<int>());                
                    break;
                case FightProtocol.op:
                    lock (clientOpCache.SyncRoot)
                    {
                        clientOpCache.Add(message.GetMessage<ClientOp>());
                    }
                    break;
            }
        }


        void StartSyncThread()
        {
            //syncThread = new Thread(new ThreadStart(SyncThread));
            //syncThread.Start();
            sendTimer = new Timer(SyncTimerCallback,null,0,50); ;
            
            Console.WriteLine("开始同步线程");
        }

        private void SyncTimerCallback(object state)
        {
            frameCount++;
            AllClientOp allClientOp = new AllClientOp();
            allClientOp.frameCount = frameCount;
            lock (clientOpCache.SyncRoot)
            {
                if (clientOpCache.Count != 0)
                {
                    allClientOp.allClientOp = new Dictionary<int, GameProtocol.dto.fight.ClientOp>();
                }
                for (int i = 0; i < clientOpCache.Count; i++)
                {
                    allClientOp.allClientOp.Add(i,(ClientOp) clientOpCache[i]);
                }
               
                clientOpCache.Clear();
            }
            //if (allClientOp.allClientOp.Count == 0)
            //{
            //    ClientOp op = new ClientOp();
            //    op.clientId = 255;
            //    op.op = new Dictionary<int, string>();
            
            //}
            brocast(FightProtocol.op, allClientOp);
        }

        void StopSyncThread()
        {
            sendTimer.Dispose();
            Console.WriteLine("停止同步线程");
        }

       

        private void getEqu(UserToken token,int code)
        {
            if (!EquData.equMap.ContainsKey(code)) return;

            int userId = getUserId(token);
            AbsFightModel model;
            if(teamOne.ContainsKey(userId))
            {
                model = teamOne[userId];
            }
            else
            {
                model = teamTwo[userId];
            }

            EquModel e = EquData.equMap[code];

            if(((FightPlayerModel)model).money>=e.price)
            {
                for (int i = 0; i < ((FightPlayerModel)model).equs.Length; i++)
                {
                    if(((FightPlayerModel)model).equs[i]==0)
                    {
                        ((FightPlayerModel)model).money -= e.price;
                        ((FightPlayerModel)model).equs[i] = code;
                        ((FightPlayerModel)model).atk += e.atk;
                        ((FightPlayerModel)model).def += e.def;
                        ((FightPlayerModel)model).maxHp += e.hp;
                        write(token, FightProtocol.GET_EQU_SRES,true);
                        write(token, FightProtocol.GET_EQU_SRES, model);
                        return;
                    }
                }

                write(token, FightProtocol.GET_EQU_SRES, false);
                
            }
        }

        private void skill(UserToken token,SkillAtkModel value)
        {
            value.userId = getUserId(token);
            brocast(FightProtocol.SKILL_BRO, value);
        }

        private void skillUp(UserToken token,int value)
        {
            int userId = getUserId(token);
            
            FightPlayerModel player;
            if(teamOne.ContainsKey(userId))
            {              
                player = (FightPlayerModel)teamOne[userId];
               
            }
            else
            {
                player = (FightPlayerModel)teamTwo[userId];
            }
            
            if(player.free>0)
            {
                //遍历角色技能列表 判断是否有此技能
                foreach(FightSkill item in player.skills)
                {
                    if(item.code==value)
                    {
                        //判断玩家等级 是否达到技能提升等级
                        if(item.nextLevel!=-1&&item.level<=player.level)
                        {
                            player.free -= 1;
                            int level = item.level + 1;
                            SkillLevelData data = SkillData.skillMap[value].levels[level];
                            item.nextLevel = data.level;
                            item.range = data.range;
                            item.time = data.time;
                            item.level = level;
                            write(token, FightProtocol.SKILL_UP_SRES, item);
                        }
                        return;
                    }
                }
            }
        }

        private void damage(UserToken token,DamageDTO value)
        {
            int userId = getUserId(token);
            AbsFightModel atkModel;
            int skillLevel = 0;
            //判断攻击者是玩家英雄 还是小兵
            if(value.userId>=0)
            {
                if (value.userId != userId) return;
                atkModel = getPlayer(userId);
                if(value.skill>0)
                {
                    skillLevel = (atkModel as FightPlayerModel).skillLevel(value.skill);
                    if(skillLevel==-1)
                    {
                        return;
                    }
                }
            }
            else
            {
                //TODO:小兵
                atkModel = getPlayer(userId);
            }
            //获取技能算法
            //循环获取目标数据 和攻击者数据 进行伤害计算 得出伤害数值
            if (!SkillProcessMap.has(value.skill)) return;
            ISkill skill = SkillProcessMap.get(value.skill);
            List<int[]> damages = new List<int[]>();
            foreach(int[]item in value.target)
            {
                AbsFightModel target = getPlayer(item[0]);
                skill.damage(skillLevel, ref atkModel, ref target, ref damages);
                if(target.hp==0)
                {
                    switch(target.type)
                    {
                        case ModelType.HUMAN:
                            if(target.id>0)
                            {
                                //击杀英雄
                                if(atkModel.id>=0)
                                {
                                    ((FightPlayerModel)atkModel).money += 300;
                                    if (((FightPlayerModel)atkModel).AddExp(10))
                                    {
                                        //如果加经验升级了 通知客户端 用户升级
                                    }
                                }
                                //启动定时任务 指定时间之后发送英雄复活信息 并且将英雄数据设置为满状态
                                ScheduleUtil.Instance.schedule(delegate {
                                    target.hp = target.maxHp;
                                    //通知客户端 指定英雄复活了
                                }, ((FightPlayerModel)target).level * 10);
                               

                            }
                            else
                            {
                                //击杀小兵
                                ((FightPlayerModel)atkModel).money += 300;
                                 if(((FightPlayerModel)atkModel).AddExp(5))
                                {
                                    //如果加经验升级了 通知客户端 用户升级
                                }

                                //移除小兵数据
                                if(target.team==1)
                                {
                                    teamOne.Remove(target.id);
                                }
                                else
                                {
                                    teamTwo.Remove(target.id);
                                }
                            }
                            break;

                        case ModelType.BUILD:
                            //打破建筑 给钱
                            if(target.id==-1)
                            {
                                GameOver(1);
                            }
                            else if(target.id==-11)
                            {
                                GameOver(2);
                                return;
                            }
                            if(atkModel.id>=0)
                            {
                                ((FightPlayerModel)atkModel).money += 180;
                            }

                            if (target.team == 1)
                            {
                                teamOne.Remove(target.id);
                            }
                            else
                            {
                                teamTwo.Remove(target.id);
                            }

                            break;
                    }
                }
            }

            value.target = damages.ToArray();
            brocast(FightProtocol.DAMAGE_BRO, value);
            
        }

        private void GameOver(int winTeam)
        {
            brocast(FightProtocol.OVER_BRO, winTeam);

            foreach(int item in herosId)
            {
                if(off.Contains(item))
                {
                    //添加逃跑场
                    userBiz.addRecord(item, -1);
                }
                else if (winTeam==1)
                {
                    if(teamOne.ContainsKey(item))
                    {
                        //添加胜场
                        userBiz.addRecord(item, 0);
                    }
                    else
                    {
                        //添加败场
                        userBiz.addRecord(item, 1);
                    }
                }
                else
                {
                    if (teamTwo.ContainsKey(item))
                    {
                        //添加胜场
                        userBiz.addRecord(item, 0);
                    }
                    else
                    {
                        //添加败场
                        userBiz.addRecord(item, 1);
                    }
                }
            }

            if (refreshId != -1)
            {
                ScheduleUtil.Instance.removeMisson(refreshId);
            }
            EventUtil.destoryFight(GetArea());
        }

        AbsFightModel getPlayer(int userId)
        {
            if(teamOne.ContainsKey(userId))
            {
                return teamOne[userId];
            }
            return teamTwo[userId];
        }

        private void attack(UserToken token,int value)
        {
            AttackDTO atk = new GameProtocol.dto.fight.AttackDTO();
            atk.userId = getUserId(token);
            atk.target = value;
            brocast(FightProtocol.ATTACK_BRO, atk);

        }

        private void move(UserToken token,MoveDTO value)
        {
            int userId = getUserId(token);
            value.userId = userId;
            brocast(FightProtocol.MOVE_BRO, value);
        }

        private new void enter(UserToken token)
        {
            if (isEntered(token)) return;
            base.enter(token);
            enterCount.GetAndReduce();
            //所有人准备了  发送房间信息
            if (enterCount.get() == 0)
            {
                FightRoomModel room = new FightRoomModel();
                room.teamOne = teamOne.Values.ToArray();
                room.teamTwo = teamTwo.Values.ToArray();
                brocast(FightProtocol.START_BRO, room);
                StartSyncThread();
                refreshMonster();
            }
        }

        public override byte GetType()
        {
            return Protocol.TYPE_FIGHT;
        }
    }

   
}
