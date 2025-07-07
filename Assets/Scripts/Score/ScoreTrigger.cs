using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    Inventory inventory;
    BoxCollider2D bx;
    Score score;

    public string[] fitThing;//��ǰ�������õ���Ʒ����

    private bool canTrue;//��ǰ��Ʒ�Ƿ񱻸ö���ʹ��

    private void Start()
    {
        canTrue = false;
        inventory = Inventory.instance;
        bx = GetComponent<BoxCollider2D>();
        score = GameObject.Find("Score").GetComponent<Score>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.root.GetComponent<ItemObject>())
        {
            ItemObject item = collision.transform.root.GetComponent<ItemObject>();
            if(item.scoreTimes >0 && item.GetItemData().itemType == ItemType.����)
            {
                foreach (var item1 in fitThing)//��⵱ǰ��ײ���Ƿ���ϱ����ڼӷ���
                {
                    if(item1 == item.GetItemData().itemName)
                    {
                       canTrue = true; break;
                    }
                }
                

                if(canTrue)
                {
                    item.scoreTimes--;
                    score.score[0] += item.GetItemData().score;
                    canTrue = false;
                }
                else
                {
                    item.scoreTimes--;
                    score.score[0] += item.GetItemData().score/2;
                    canTrue=false;
                }
            }
         
        }
    }
}
