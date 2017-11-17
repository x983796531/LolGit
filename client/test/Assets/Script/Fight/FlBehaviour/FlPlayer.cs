using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using FixedPointy;
using System.Collections.Generic;

public class FlPlayer : FlBehaviour
{


    private Fix speed=new Fix(0);
    //public override void Update(AllClientOp allClientOp)
    //{
    //    Move(allClientOp);
       
    //}

    public void Move(AllClientOp allOp)
    {
        if (allOp.allClientOp != null)
        {
            Dictionary<int, ClientOp> curPlayerMoveOp = new Dictionary<int, ClientOp>();
            foreach (int index in allOp.allClientOp.Keys)
            {
                if (allOp.allClientOp[index].clientId == gameObject.id && allOp.allClientOp[index].op.ContainsKey(1))
                {

                    curPlayerMoveOp.Add(index, allOp.allClientOp[index]);
                }
            }
            int maxIndex = 0;
            foreach (int index in curPlayerMoveOp.Keys)
            {
                if (index > maxIndex)
                    maxIndex = index;
            }

            if (curPlayerMoveOp.Count == 0)
                return;
            string moveStr = curPlayerMoveOp[maxIndex].op[1];
            if (moveStr != "-1")
            {
                string[] moveVecStr = moveStr.Split('?');
                Fix x = float.Parse(moveVecStr[0]).ToFix();
                Fix y = float.Parse(moveVecStr[1]).ToFix();
                GetComponent<FlTransform>().Rotate(new FixVec2(x, y));
                speed = (3 / 20f).ToFix();
                //Debug.Log("speedBefore:"+ speed.Raw);
            }
            else
            {
                speed = 0;
                //GetComponent<FlTransform>().Translate(0f.ToFix());
            }


        }
       

        MoveModel();
        // GetComponent<FlTransform>().Translate()
    }

    void MoveModel()
    {
        //Debug.Log("speed:" + speed.Raw);
        GetComponent<FlTransform>().Translate(speed);

        float positionX = transform.GetFloatPosition().x;
       
        float positionZ = transform.GetFloatPosition().y;
        //if (gameObject.id == 1)
        //{
        //    Debug.Log("fx:" + transform.position.X.toFloat() + " | fy:" + transform.position.Y.toFloat());
        //    Debug.Log("x:" + positionX + " | y:" + positionZ + " | speed:" + speed.toFloat());
        //}
        gameObject.GetComponent<FlGameModel>().gameModel.transform.position = new Vector3( positionX,0, positionZ);
        //float directionX = transform.GetFloatDirection().x;
        //float directionZ = transform.GetFloatDirection().y;
        //if (gameObject.id == 1)
        //{

        //Debug.Log("dir:" + transform.direction.toVector2());
        //}
        Vector2 dir = transform.forword.toVector2();
        gameObject.GetComponent<FlGameModel>().gameModel.transform.forward = new Vector3(dir.x, 0, dir.y);
    }
}
