using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public enum ItemType
{
    黛玉,
    鲁提辖,
    无
}

[CreateAssetMenu(fileName ="New Item Data", menuName = "Data/Item")]
public class ItemData :ScriptableObject
{
    public string itemName;//装备名称
    public Sprite icon;//图标
    public ItemType itemType;
    public int size;
    public float gravity;
    public GameObject perfabe;
    public int score;
    
}
