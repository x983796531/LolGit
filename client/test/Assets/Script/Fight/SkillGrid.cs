using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameProtocol.dto.fight;
using GameProtocol;

public class SkillGrid : MonoBehaviour {
    [SerializeField]
    private Image mask;

    public Image background;
    public FightSkill skill;

    private bool sclied = false;

    private float maxTime=0;
    private float nowTime = 0;
    [SerializeField]
    private Button levelUpBtn;
	public void init(FightSkill skill)
    {
        this.skill = skill;
        Sprite sp = Resources.Load<Sprite>("SkillIcon/" + skill.code);
        background.sprite = sp;
        mask.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(sclied)
        {
            nowTime -= Time.deltaTime;
            if(nowTime<0)
            {
                nowTime = 0;
                sclied = false;
                mask.gameObject.SetActive(false);

            }

            mask.fillAmount = nowTime / maxTime;
        }
    }

    public void setMask(int time)
    {
        //如果为0 说明要取消冷却遮罩
        if(time==0)
        {
            //判断是否满足  强制取消遮罩条件
            if(!sclied&&skill.level>0)
            {
                mask.gameObject.SetActive(false);
                return;
            }
            else
            {
                mask.gameObject.SetActive(true);
                return;
            }
        }

        nowTime = time;
        maxTime = time;
        mask.gameObject.SetActive(true);
        sclied = true;
    }

    /// <summary>
    /// 获取焦点
    /// </summary>
    public void pointerEnter()
    {
        //显示tip
    }

    /// <summary>
    /// 失去焦点
    /// </summary>
    public void pointerExit()
    {
        //关闭tip显示
    }

    public void pointerClick()
    {
        if(nowTime>0)
        {
            return;
        }
        FightScene.instance.skill = skill.code;
    }

    public void setBtnState(bool state)
    {
        levelUpBtn.interactable = state;
    }

    public void SkillChange(FightSkill skill)
    {

    }

    public void levelUp()
    {
        //向服务器发送消息 申请升级技能
        this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.SKILL_UP_CREQ, skill.code);
    }

}
