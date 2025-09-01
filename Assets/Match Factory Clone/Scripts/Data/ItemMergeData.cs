using UnityEngine;
using System.Collections.Generic;

public struct ItemMergeData
{
    public string itemName;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.name;
        items = new List<Item>(); // Inicializamos

        items.Add(firstItem); // Adicionamos o que foi passado como parametro dentro da Lista de items
    }
}
