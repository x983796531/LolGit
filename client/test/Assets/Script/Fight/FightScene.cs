using UnityEngine;
using System.Collections;
using GameProtocol;
using System;
using GameProtocol.dto.fight;
using UnityEngine.UI;
using GameProtocol.constans;
using System.Collections.Generic;

public class FightScene : MonoBehaviour
{
    public static FightScene instance;

    [SerializeField]
    /// <summary>
    /// 个人头像
    /// </summary>
    private Image head;

    [SerializeField]
    /// <summary>
    /// 自身血条
    /// </summary>
    private Slider hpSlider;

    [SerializeField]
    /// <summary>
    /// 角色名
    /// </summary>
    private Text nameText;

    [SerializeField]
    /// <summary>
    /// 技能Icon
    /// </summary>
    private SkillGrid[] skills;

    [SerializeField]
    /// <summary>
    /// 等级
    /// </summary>
    private Text levelText;

    public int skill = -1;//当前鼠标左键点击释放的技能ID

    public PlayerCon myHero;
    /// <summary>
    /// 当前自己的英雄是否死亡
    /// </summary>
    public bool dead = false;
    [SerializeField]
    private Transform numParent;//掉血数字父容器

    private Camera mainCamera;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        
        mainCamera = Camera.main;
        
        this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
    }

    public void NumUp(Transform p,string value)
    {
        GameObject hp = (GameObject)Instantiate(Resources.Load("Prefab/Hp"));
        hp.GetComponent<Text>().text = value;
        hp.transform.parent = numParent;
        hp.transform.localScale = Vector3.one;
        hp.transform.localPosition = Camera.main.WorldToScreenPoint(p.position) + new Vector3(30,20);
    }

    public void initView(FightPlayerModel model, PlayerCon hero)
    {
        myHero = hero;
        head.sprite = Resources.Load<Sprite>("HeroIcon/" + model.code);
        hpSlider.value = model.hp / model.maxHp;
        nameText.text = HeroData.heroMap[model.code].name;
        levelText.text = model.level.ToString();
        int i = 0;

        foreach (FightSkill item in model.skills)
        {
            skills[i].init(item);
            i++;
        }

    }

    public void refreshView(AbsFightModel model)
    {
        hpSlider.value = model.hp / model.maxHp;
        levelText.text =((FightPlayerModel)model).level.ToString();
    }

    private void RefreshLevelUp()
    {
        int i = 0;
        foreach(FightSkill item in((FightPlayerModel)(myHero.data)).skills)
        {
            if(item.nextLevel<= ((FightPlayerModel)(myHero.data)).level)
            {
                if(((FightPlayerModel)(myHero.data)).free>0)
                {
                    skills[i].setBtnState(true);
                }
                else
                {
                    skills[i].setBtnState(false);
                }
            }
            else
            {
                skills[i].setBtnState(false);
            }
            skills[i].SkillChange(item);
            skills[i].setMask(0);
            i++;
        }
    }

    public void lookAt()
    {
        //Debug.Log(myHero.name);
        if(myHero)
        {
            //mainCamera.transform.position = myHero.transform.position + new Vector3(-21.65f, 21.63f, 1.17f);
        }
        
    }

    private int cameraH;
    private int cameraV;

    public float cameraSpeed = 1f;

    /// <summary>
    /// 相机横移 向右传-1 向左传1
    /// </summary>
    public void cameraHMove(int dir)
    {
        if (cameraH != dir)
            cameraH = dir;
    }

    /// <summary>
    /// 相机纵移 向上传1 向下传-1
    /// </summary>
    public void cameraVMove(int dir)
    {
        if (cameraV != dir)
            cameraV = dir;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            switch (cameraH)
            {
                case 1:
                    if (mainCamera.transform.position.z < 150)
                    {
                        mainCamera.transform.Translate(Vector3.forward, Space.World);
                    }
                    break;
                case -1:
                    if (mainCamera.transform.position.z > 0)
                    {
                        mainCamera.transform.Translate(Vector3.back, Space.World);
                    }
                    break;

            }

            switch (cameraV)
            {
                case 1:
                    if (mainCamera.transform.position.x < 186)
                    {
                        mainCamera.transform.Translate(Vector3.right, Space.World);
                    }

                    break;
                case -1:
                    if (mainCamera.transform.position.x > 22.5)
                    {
                        mainCamera.transform.Translate(Vector3.left, Space.World);
                    }
                    break;
            }
        }
        else
        {

            lookAt();
        }

        
        
    }

    public void leftClick(Vector2 position)
    {
        if (skill == -1) return;
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        List<Transform> list = new List<Transform>();
        Vector3 tigger = Vector3.zero;
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            RaycastHit item = hits[i];
           
            
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                tigger = item.point;
            }
            if(item.transform.gameObject.layer==LayerMask.NameToLayer("enemy"))
            {
                list.Add(item.transform);
            }

           

        }
        myHero.baseSkill(skill, list.ToArray(), tigger);
        skill = -1;

    }

    public void rightClick(Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);

        for(int i=hits.Length-1;i>=0;i--)
        {
            RaycastHit item = hits[i];
            //如果是敌方单位则进行普通攻击

            if (item.transform.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                PlayerCon con =item.transform.GetComponent<PlayerCon>();
                if (Vector3.Distance(myHero.transform.position, item.transform.position) < con.data.aRange)
                {
                    this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.ATTACK_CREQ, con.data.id);
                    return;
                }
                continue;
            }
            //如果是己方单位无视
            //如果是地板曾 则开始寻路
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                MoveDTO dto = new MoveDTO();
                dto.x = item.point.x;
                dto.y = item.point.y;
                dto.z = item.point.z;
                this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.MOVE_CREQ, dto);
                return;
            }
        }
       
    }

    public void SkillMask(int code)
    {
        foreach(SkillGrid item in skills)
        {
            if(item.skill.code==code)
            {
                item.setMask(item.skill.time);
            }
        }
    }
}
