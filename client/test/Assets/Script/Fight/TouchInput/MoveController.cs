using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using GameProtocol;

public class MoveController : MonoBehaviour
{
    public float joyPositionX;
    public float joyPositionY;

    private float lastSendTime;
    private float lastJoyPositionX;
    private float lastJoyPositionY;

    public bool initFinished = false;

    private void Start()
    {
        StartCoroutine(init());
    }

    public IEnumerator init()
    {
        yield return new WaitForSeconds(0.5f);
        initFinished = true;
    }
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }


    //移动摇杆结束
    void OnJoystickMoveEnd(MovingJoystick move)
    {
        //停止时，角色恢复idle
        if (move.joystickName == "moveJoystick")
        {    
            Debug.Log("移动摇杆结束");
            ClientOp op = new ClientOp();
            op.clientId =(byte)FightScene.instance.myHero.data.id;
            op.op = new System.Collections.Generic.Dictionary<int, string>();
            op.op .Add( 1,"-1");
            this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.op, op);
        }
    }


    //移动摇杆中
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "moveJoystick")
        {
            return;
        }

        //获取摇杆中心偏移的坐标
        joyPositionX = move.joystickAxis.x;
        joyPositionY = move.joystickAxis.y;
        if(initFinished)
        SyncMove(move.joystickAxis.x, move.joystickAxis.y);


    }

    void SyncMove(float x,float y)
    {
        if (Time.realtimeSinceStartup - lastSendTime > FightGlobalInit.Instance.inputSendInterval&&(lastJoyPositionX!=x&& lastJoyPositionY!=y))
        {
            lastSendTime = Time.realtimeSinceStartup;
            lastJoyPositionX = x;
            lastJoyPositionY = y;
            ClientOp op = new ClientOp();
            op.op = new System.Collections.Generic.Dictionary<int, string>();
            op.clientId = (byte)FightScene.instance.myHero.data.id;
            op.op.Add(1, x+"?"+y);
            this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.op, op);
        }
        
        
        
    }

}