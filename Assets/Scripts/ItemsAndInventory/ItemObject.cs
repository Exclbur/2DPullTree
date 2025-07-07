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
        gameObject.name = "Item object-" + itemData.name;//���ýű����������ϵ����Ƹ�Ϊ��ʵ�����������
    }

    public void SetUpItem(ItemData _item ,Vector2 _velocity)
    {
        itemData = _item;
        rb.velocity = _velocity;

        SetUpVisuals ();
    }
    public void PickUpItem()
    {
        if(Inventory.instance.inventory.Count == 4)//ֻ�����ĸ�
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
