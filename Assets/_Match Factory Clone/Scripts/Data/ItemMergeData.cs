using UnityEngine;
using System.Collections.Generic;

public struct ItemMergeData
{
    public ItemNameEnum itemName;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.ItemName;
        items = new List<Item>(); // Inicializamos

        items.Add(firstItem); // Adicionamos o que foi passado como parametro dentro da Lista de items
    }

    public void Add(Item item) => items.Add(item);

    public bool CanMergeItems()
    {
        if (items.Count >= 3)
            return true;

        return false;
    }
}
