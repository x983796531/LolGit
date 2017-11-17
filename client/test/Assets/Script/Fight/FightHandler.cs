using UnityEngine;
using System.Collections;
using System;
using GameProtocol;
using GameProtocol.dto.fight;
using System.Collections.Generic;

public class FightHandler : MonoBehaviour,IHandler {
    public bool initFinished = false;

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        initFinished = true;
    }

    FightRoomModel room;
    [SerializeField]
    private Transform[] positions;//队伍1的建筑初始位置表
    [SerializeField]
    private Transform[] positions1;//队伍2的建筑初始位置表
    [SerializeField]
    private Transform startPosition;//队伍1的复活（出生）点
    [SerializeField]
    private Transform startPosition1;//队伍2的复活（出生）点

    private Dictionary<int, PlayerCon> models = new Dictionary<int, PlayerCon>();
    //private Dictionary<int, GameObject> teamTwo = new Dictionary<int, GameObject>();


    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocol.START_BRO:
                start(model.GetMessage<FightRoomModel>());
                break;
            case FightProtocol.MOVE_BRO:
                move(model.GetMessage<MoveDTO>());
                break;
            case FightProtocol.ATTACK_BRO:
                atk(model.GetMessage<AttackDTO>());
                break;
            case FightProtocol.DAMAGE_BRO:
                damage(model.GetMessage<DamageDTO>());
                break;
            case FightProtocol.SKILL_BRO:
                skill(model.GetMessage<SkillAtkModel>());
                break;
            case FightProtocol.REFRESH_BRO:
                refreshMonster(model.GetMessage<FightMonsterModel[]>());            
                break;
            case FightProtocol.op:
                AllClientOp clientOp = model.GetMessage<AllClientOp>();                
                if(initFinished)
                {
                    AllFlGameObjectController.Instance.SendAllFlGameObjectMessage(clientOp);
                }
               //Debug.Log(clientOp.ToString());
                break;

        }
    }

    private void refreshMonster(FightMonsterModel [] monsters)
    {

    }

    private void skill(SkillAtkModel model)
    {
        List<Transform> list = new List<Transform>();
        if(model.type==0)
        {
            list.Add(models[model.userId].transform);

        }

        models[model.userId].skill(model.skill, list.ToArray(),new Vector3(model.position[0],model.position[1],model.position[2]));
        if(model.userId==GameData.User.id)
        {
            FightScene.instance.SkillMask(model.skill);
        }
    }

    private void damage(DamageDTO value)
    {
        foreach(int[] item in value.target)
        {
            PlayerCon pc = models[item[0]];
            pc.data.hp -= item[1];
            //实例化掉血数字
            FightScene.instance.NumUp(pc.transform, item[1].ToString());
            pc.HpChange();
            if(pc.data.id==GameData.User.id)
            {
                FightScene.instance.refreshView(pc.data);
            }
            if(item[2]==0)
            {
                if(item[0]>=0)
                {
                    pc.gameObject.SetActive(false);
                    if(pc.data.id==GameData.User.id)
                    {
                        FightScene.instance.dead = true;
                    }
                }
                else
                {
                    Destroy(pc.gameObject);
                }
            }
        }
    }

    private void atk(AttackDTO dto)
    {
        PlayerCon obj = models[dto.userId];
        PlayerCon target = models[dto.target];
        obj.GetComponent<PlayerCon>().attack(new Transform[] { target.transform });
    }

    private void move(MoveDTO value)
    {
        Vector3 target = new Vector3(value.x, value.y, value.z);
        models[value.userId].SendMessage("move", target);
    }


    private void start(FightRoomModel value)
    {

        FlGameObjectFactory.instance.GenerateFlGameObject(value);
        //room = value;

        //int myTeam = -1;

        //foreach(AbsFightModel item in value.teamOne)
        //{
        //    if (item.id == GameData.User.id)
        //        myTeam = item.team;
        //}
        //if(myTeam==-1)
        //{
        //    foreach (AbsFightModel item in value.teamTwo)
        //    {
        //        if (item.id == GameData.User.id)
        //            myTeam = item.team;
        //    }
        //}


        //foreach (AbsFightModel item in value.teamOne)
        //{
        //    GameObject go;
        //    PlayerCon pc;
        //    if (item.type==ModelType.HUMAN)
        //    {
        //        go = (GameObject)Instantiate(Resources.Load<GameObject>("prefab/Player/" + item.code), startPosition.position+new Vector3(UnityEngine.Random.Range(0,2),0, UnityEngine.Random.Range(0,2)),startPosition.rotation);

        //        pc = go.GetComponent<PlayerCon>();

        //        pc.init((FightPlayerModel)item, myTeam);
        //        FlGameObjectFactory.instance.GenerateFlGameObject(item.id);
        //    }
        //    else
        //    {
        //        go =(GameObject) Instantiate(Resources.Load<GameObject>("prefab/build/1_" + item.code),positions[item.code-1].position,positions[item.code-1].rotation);
        //        pc = go.GetComponent<PlayerCon>();
        //        //pc.init((FightPlayerModel)item, myTeam);
        //    }
        //    this.models.Add(item.id, pc);

        //    if(item.id==GameData.User.id)
        //    {
        //        FightScene.instance.initView((FightPlayerModel)item,pc);
        //        FightScene.instance.lookAt();
        //    }
            
        //}


        //foreach (AbsFightModel item in value.teamTwo)
        //{
        //    GameObject go;
        //    PlayerCon pc;
        //    if (item.type == ModelType.HUMAN)
        //    {
        //        go = (GameObject)Instantiate(Resources.Load<GameObject>("prefab/Player/" + item.code), startPosition1.position + new Vector3(UnityEngine.Random.Range(0, 2), 0, UnityEngine.Random.Range(0, 2)), startPosition1.rotation);

        //        pc = go.GetComponent<PlayerCon>();

        //        pc.init((FightPlayerModel)item, myTeam);

        //        FlGameObjectFactory.instance.GenerateFlGameObject(item.id);
        //    }
        //    else
        //    {
        //        go = (GameObject)Instantiate(Resources.Load<GameObject>("prefab/build/2_" + item.code), positions1[item.code - 1].position, positions1[item.code - 1].rotation);
        //        pc = go.GetComponent<PlayerCon>();

        //        //pc.init((FightPlayerModel)item, myTeam);
        //    }
        //    this.models.Add(item.id, pc);

        //    if (item.id == GameData.User.id)
        //    {
        //        FightScene.instance.initView((FightPlayerModel)item,pc);
        //        FightScene.instance.lookAt();
        //    }

        //    //FlGameObjectFactory.instance.GenerateFlGameObject(item.id);
        //}


    }

    
}
