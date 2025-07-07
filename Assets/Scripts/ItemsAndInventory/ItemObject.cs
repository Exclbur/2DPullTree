using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;
    public float nextPickTime;

    public int scoreTimes;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        scoreTimes = 1;
    }

    private void Update()
    {
        nextPickTime -= Time.deltaTime;
    }

    private void SetUpVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object-" + itemData.name;//讲该脚本挂在物体上的名称改为真实所属类的名称
    }

    public void SetUpItem(ItemData _item ,Vector2 _velocity)
    {
        itemData = _item;
        rb.velocity = _velocity;

        SetUpVisuals ();
    }
    public void PickUpItem()
    {
        if(Inventory.instance.inventory.Count == 4)//只能拿四个
        {
            return;
        }
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

    public ItemData GetItemData()
    {
        return itemData;
    }
}
