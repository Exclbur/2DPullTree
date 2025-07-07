using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

//���
public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<InventoryItem> inventory;//ͳ����Ʒ�Ķ���
    public Dictionary<ItemData,List < InventoryItem>> inventoryDic;//�ֵ䣬���ڲ�ѯ��Ʒ������Ʒ���������Թ�������

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;//ȷ��ÿ����Ʒ��UI�еĲ�����������,����ǲ�����

    private UI_ItemSlot[] inventoryItem;//���Ʊ������UI��
    private void Awake()//ÿ��������ֻ����һ������������
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

  

    private void UpdateUISlot()//��UI������ʾ����ע�Ŀ��
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


    public void AddItem(ItemData _item)//�����Ʒ
    {
       
        AddToInventory(_item);
        UpdateUISlot();
    }

    private void AddToInventory(ItemData _item)
    {

        InventoryItem newItem = new InventoryItem(_item);
        inventory.Add(newItem);

        // ����ֵ����Ѿ����ڴ���Ʒ������
        if (!inventoryDic.ContainsKey(_item))
        {
            inventoryDic[_item] = new List<InventoryItem>();
        }

        // ����Ʒʵ�������ֵ���
        inventoryDic[_item].Add(newItem);

    }

    public void RemoveItem(ItemData _item)
    {

        if (inventoryDic.TryGetValue(_item, out List<InventoryItem> items))
        {
            if (items.Count > 0)
            {
                // �Ƴ���һ��ʵ��
                InventoryItem itemToRemove = items[0];
                items.RemoveAt(0); // �Ƴ���Ʒʵ��
                inventory.Remove(itemToRemove); // �ӱ������Ƴ�

                // �������Ʒ����û��ʵ�����Ƴ��ֵ��еļ�¼
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
