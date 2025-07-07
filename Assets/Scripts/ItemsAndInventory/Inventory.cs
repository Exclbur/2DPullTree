using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

//库存
public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<InventoryItem> inventory;//统计物品的队列
    public Dictionary<ItemData,List < InventoryItem>> inventoryDic;//字典，用于查询物品，将物品和他的属性关联起来

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//确定每个物品在UI中的布局是怎样的,这个是材料栏

    private UI_ItemSlot[] inventoryItem;//控制背包里的UI；
    private void Awake()//每个场景中只能有一个这个管理组件
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>(4); 
        inventoryDic = new Dictionary<ItemData,List <InventoryItem>>();  

        inventoryItem = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
    }

  

    private void UpdateUISlot()//在UI方面显示出标注的库存
    {
       

        for (int i = 0; i < inventoryItem.Length; i++)
        {
            inventoryItem[i].CleanUpSlot();
        }

       

        for (int i = 0;i< inventory.Count; i++)
        {
            inventoryItem[i].UpdateSlot(inventory[i]);
        }

    }


    public void AddItem(ItemData _item)//添加物品
    {
       
        AddToInventory(_item);
        UpdateUISlot();
    }

    private void AddToInventory(ItemData _item)
    {

        InventoryItem newItem = new InventoryItem(_item);
        inventory.Add(newItem);

        // 如果字典中已经存在此物品的数据
        if (!inventoryDic.ContainsKey(_item))
        {
            inventoryDic[_item] = new List<InventoryItem>();
        }

        // 将物品实例加入字典中
        inventoryDic[_item].Add(newItem);

    }

    public void RemoveItem(ItemData _item)
    {

        if (inventoryDic.TryGetValue(_item, out List<InventoryItem> items))
        {
            if (items.Count > 0)
            {
                // 移除第一个实例
                InventoryItem itemToRemove = items[0];
                items.RemoveAt(0); // 移除物品实例
                inventory.Remove(itemToRemove); // 从背包中移除

                // 如果该物品类型没有实例，移除字典中的记录
                if (items.Count == 0)
                {
                    inventoryDic.Remove(_item);
                }
            }
        }

        UpdateUISlot() ;
    }

    public void RemoveBall(int a)
    { 
        RemoveItem(inventory[a].data);
    }
}
