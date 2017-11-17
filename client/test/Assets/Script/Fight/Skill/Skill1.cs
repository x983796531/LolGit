using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;
using GameProtocol;

public class Skill1 : TryAgainSkill {

    private void OnTriggerEnter(Collider c)
    {
        int target;
        if(c.transform.gameObject.layer==LayerMask.NameToLayer("enemy"))
        {
            target = c.GetComponent<PlayerCon>().data.id;
        }
        else
        {
            return;
        }

        DamageDTO dto = new DamageDTO();
        dto.skill = 1;
        dto.userId = GameData.User.id;
        dto.target = new int[][] { new int[] { target } };
        this.WriteMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.DAMAGE_CREQ, dto);
    }
}
