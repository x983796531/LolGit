using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameProtocol;
using GameProtocol.dto;

public class LoginScreen : MonoBehaviour {

    #region 登录面板部分
    [SerializeField]
    private InputField accountInput;

    [SerializeField]
    private InputField passWordInput;

    #endregion

    [SerializeField]
    private Button loginBtn;

    [SerializeField]
    private GameObject regPanel;

    #region 注册面板部分
    [SerializeField]
    private InputField regAcountInput;

    [SerializeField]
    private InputField regPwInput;

    [SerializeField]
    private InputField regPw1Input;
    #endregion


    //private void Start()
    //{
    //    NetIO io = NetIO.Instance;
    //}


    public void LoginOnClick()
    {
        if(accountInput.text.Length==0||accountInput.text.Length>6)
        {
            WarrningManager.errors.Add(new WarningModel("账号不合法"));
            Debug.Log("账号不合法");
            return;
        }
        if (passWordInput.text.Length == 0 || passWordInput.text.Length > 6)
        {
            Debug.Log("密码不合法");
            return;
        }
        //验证通过 申请登录
        //loginBtn.interactable = false;
        AccountInfoDTO dto = new AccountInfoDTO();

        dto.account = accountInput.text;
        dto.password = passWordInput.text;
        this.WriteMessage(GameProtocol.Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, dto);
        loginBtn.interactable = false;
    }

    public void openLoginBtn()
    {
        passWordInput.text = string.Empty;
        loginBtn.interactable = true;
    }

   

    public void RegClick()
    {
        regPanel.SetActive(true);
    }

    public void regCloseClick()
    {
        regAcountInput.text = string.Empty;
        regPwInput.text = string.Empty;
        regPw1Input.text = string.Empty;
        regPanel.SetActive(false);
    }

    public void regpanelRegClick()
    {
        if (regAcountInput.text.Length == 0 || regAcountInput.text.Length > 6)
        {
            Debug.Log("账号不合法");
            return;
        }
        if (regPwInput.text.Length == 0 || regPwInput.text.Length > 6)
        {
            Debug.Log("密码不合法");
            return;
        }
        if (!regPwInput.text.Equals(regPw1Input.text))
        {
            Debug.Log("两次输入密码不一致");
            return;
        }

        //验证通过 申请注册  并关闭注册面板
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = regAcountInput.text;
        dto.password = regPw1Input.text;
        this.WriteMessage(Protocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, dto);
        regCloseClick();
    }
}
