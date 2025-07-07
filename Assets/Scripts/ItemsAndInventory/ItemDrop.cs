using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfItem;
    [SerializeField] private GameObject dropPerfabs;
    [SerializeField] private ItemData item;

    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    private void Start()
    {
        InvokeRepeating("GenerateDrop", 1, 15);
    }


    public virtual void GenerateDrop()
    {

        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 40) >= possibleDrop[i].score)
                dropList.Add(possibleDrop[i]);
        }

        if (dropList.Count <= 0)
        {
            dropList.Add(possibleDrop[0]);
        }

        for (int i = 0; i < amountOfItem; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }


    protected void DropItem(ItemData _item)
    {
        GameObject newDrop = Instantiate(_item.perfabe, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

        newDrop.GetComponent<ItemObject>().SetUpItem(_item, randomVelocity);
    }

}
