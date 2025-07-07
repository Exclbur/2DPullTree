using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public enum ItemType
{
    ����,
    ³��Ͻ,
    ��
}

[CreateAssetMenu(fileName ="New Item Data", menuName = "Data/Item")]
public class ItemData :ScriptableObject
{
    public string itemName;//װ������
    public Sprite icon;//ͼ��
    public ItemType itemType;
    public int size;
    public float gravity;
    public GameObject perfabe;
    public int score;
    
}
