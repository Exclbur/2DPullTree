using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    Inventory inventory;
    BoxCollider2D bx;
    Score score;

    public string[] fitThing;//当前洞口适用的物品名称

    private bool canTrue;//当前物品是否被该洞口使用

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
            if(item.scoreTimes >0 && item.GetItemData().itemType == ItemType.黛玉)
            {
                foreach (var item1 in fitThing)//检测当前碰撞物是否符合本洞口加分项
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
