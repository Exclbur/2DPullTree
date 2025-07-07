using System;
using System.Collections.Generic;


[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
      
    }

    public static implicit operator InventoryItem(List<InventoryItem> v)
    {
        throw new NotImplementedException();
    }
}
