using UnityEngine;
using System.Collections;
using GameProtocol;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField]
    private GameObject mask;//防止用户误操作 顶层遮罩

    [SerializeField]
    private CreatePanel createPanel;

    [SerializeField]
    private Text matchText;//匹配按钮文本对象

    #region 数据显示UI组件 刷新调用
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Slider expBar;
    #endregion

    // Use this for initialization
    void Start()
    {
        if(GameData.User==null)
        {
            mask.SetActive(true);
            //向服务器申请用户数据
            this.WriteMessage(Protocol.TYPE_USER, 0, UserProtocol.INFO_CREQ, null);
        }
        
    }

    public void RefreshView()
    {
        nameText.text = GameData.User.name + "  等级：" + GameData.User.level;
        expBar.value = GameData.User.exp / 100;
    }

    /// <summary>
    /// 打开创建面板
    /// </summary>
    void OpenCreate()
    {
        createPanel.open();
    }

    /// <summary>
    /// 关闭创建面板
    /// </summary>
    void CloseCreate()
    {
        createPanel.close(); 
    }

    void closeMask()
    {
        mask.SetActive(false);
    }
    
    public void match()
    {
        if(matchText.text=="开始排队")
        {
            matchText.text = "取消排队";
            this.WriteMessage(Protocol.TYPE_MATCH, 0, MatchProtocol.ENTER_CREQ, null);
        }
        else
        {
            matchText.text = "开始排队";
            this.WriteMessage(Protocol.TYPE_MATCH, 0, MatchProtocol.LEAVE_CREQ, null);
        }
    }
}
