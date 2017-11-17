using GameProtocol.dto.fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCon:MonoBehaviour
{
    [HideInInspector]
    public AbsFightModel data;

    protected Animator anim;

    public NavMeshAgent agent;

    [SerializeField]
    private GameObject mask;//战争迷雾剔除面板
    [SerializeField]
    private PlayerTitle title;//角色头顶信息

    [SerializeField]
    private MeshRenderer head;

    [SerializeField]
    private GameObject root;
    protected int state = AnimState.IDLE;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 攻击动画播放结束回调方法
    /// </summary>
    public virtual void attacked()
    {

    }

    public void HpChange()
    {
        title.hpChange(1f * data.hp / data.maxHp);
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="target"></param>
    public void move(Vector3 target)
    {
        Debug.Log("move");
        //agent.ResetPath();
        agent.SetDestination(target);
        anim.SetInteger("state", AnimState.RUN);
        state = AnimState.RUN;
    }
     
    void Update()
    {
        switch(state)
        {
            case AnimState.IDLE:
                break;
            case AnimState.RUN:
                if(agent.pathStatus==NavMeshPathStatus.PathComplete&&agent.remainingDistance<=0&&!agent.pathPending)
                {
                    state = AnimState.IDLE;
                    anim.SetInteger("state", AnimState.IDLE);
                }
                else
                {
                    if(agent.isOnOffMeshLink)
                    {
                        agent.CompleteOffMeshLink();
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 申请攻击
    /// </summary>
    /// <param name="target">攻击的目标</param>
    public virtual void attack(Transform[] target)
    {

    }

    /// <summary>
    /// 申请释放技能
    /// </summary>
    /// <param name="code"></param>
    /// <param name="target"></param>
    public virtual void skill(int code,Transform[]target,Vector3 ps)
    {

    }

    public virtual void skilled()
    {

    }

    public virtual void baseSkill(int code, Transform[] target, Vector3 ps)
    {

    }
    /// <summary>
    /// 设置是否拥有迷雾剔除
    /// </summary>
    /// <param name="state"></param>
    private void MaskState(bool state)
    {
        mask.SetActive(state);
    }

    public void init(FightPlayerModel data,int myTeam)
    {
        this.data = data;
        title.init(data, data.team == myTeam);

        head.material.SetTexture("_MainTex", Resources.Load<Texture>("HeroIcon/" + data.code));  

        //如果不是己方单位移除遮罩
        //是友方单位 移除战争视野处理
        if(data.team!=myTeam)
        {
            mask.SetActive(false);

            root.SetActive(false);

            title.gameObject.SetActive(false);

            gameObject.layer = LayerMask.NameToLayer("enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("friend");
            Destroy(GetComponentInChildren<Eye>());
        }

        if(data.id<-20)
        {
            //进行怪物寻路
        }
    }
}
