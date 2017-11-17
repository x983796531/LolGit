using UnityEngine;
using System.Collections;
using GameProtocol.dto.fight;

public class PlayerTitle : MonoBehaviour
{
    [SerializeField]
    private SpriteSlider hp;

    [SerializeField]
    private TextMesh nameText;
    [SerializeField]
    private SpriteRenderer sr;

    private void Update()
    {
        if (transform.rotation != Camera.main.transform.rotation)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void init(FightPlayerModel model,bool friend)
    {
        hp.Value = model.hp / model.maxHp;
        nameText.text = model.name;
        if(friend )
        {
            sr.color = Color.blue;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    public void hpChange(float value)
    {
        hp.Value = value;
    }
}
