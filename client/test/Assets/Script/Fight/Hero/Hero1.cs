using UnityEngine;
using System.Collections;
using GameProtocol.constans;
using GameProtocol.dto.fight;
using GameProtocol;
using System.Collections.Generic;

public class Hero1 : PlayerCon {
    private Transform[] list;
    [SerializeField]
    private GameObject effect;//攻击粒子

    private List<Transform> targetList = new List<Transform>();

    private int skillCode;
    private Vector3 skillPos;
    public override void attack(Transform[] target)
    {
        if(target!=null)
        {
            list = target;
            if (state == AnimState.RUN)
            {
                agent.CompleteOffMeshLink();
            }

            transform.LookAt(target[0]);

            state = AnimState.ATTACK;
            anim.SetInteger("state", AnimState.ATTACK);
        }
       
    }

    public override void attacked()
    {

        //如果是近战 直接向服务器发送伤害消息
        //如果是远程则进行下面的循环发送粒子
        foreach(Transform item in list)
        {
            GameObject go =(GameObject)Instantiate(effect,transform.position + new Vector3(0, 1), transform.rotation);
            //让粒子向敌人位移
            go.GetComponent<TargetSkill>().Init(list[0], -1, data.id);
            state = AnimState.IDLE;
            anim.SetInteger("state", AnimState.IDLE);
        }
    }

    public override void skill(int code, Transform[] target,Vector3 ps)
    {
        if (state == AnimState.RUN)
        {
            agent.CompleteOffMeshLink();
        }


        transform.LookAt(skillPos);
        state = AnimState.SKILL;
        anim.SetInteger("state", AnimState.SKILL);

        skillCode = code;
        skillPos = ps;
        targetList.Clear();
        if(target!=null)
        {
            targetList.AddRange(target);
        }
        
    }

    public override void skilled()
    {
        switch (skillCode)
        {
            case 1:
                
                GameObject go = Instantiate(Resources.Load<GameObject>("prefab/Skill/skill1"), transform.position + transform.up * 2, transform.rotation) as GameObject;
                go.GetComponent<Skill1>().init(this, 30, 20);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                return;
        }
        state = AnimState.IDLE;
        anim.SetInteger("state", AnimState.IDLE);
    }

    public override void baseSkill(int code, Transform[] target, Vector3 ps)
    {
        SkillAtkModel atk=new SkillAtkModel();
        switch(code)
        {
            case 1:
                atk.skill = code;
                atk.type = 1;
                atk.position = new float[] { ps.x, ps.y, ps.z };
                this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.SKILL_CREQ, atk);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;

        }
    }
}
