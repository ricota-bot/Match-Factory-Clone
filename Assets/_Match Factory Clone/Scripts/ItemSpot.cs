using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [Header("Settings")]
    private Item item;
    public Item Item => item; // GETTER da variavel item


    public void Populate(Item item) // Vamos chamar essa função quando coletar um Item e levar ele pro Spot
    { // Fazemos isso no ItemSpotsManager, quando chamamos o Action do MouseButtonUp "OnItemClickedCallBack"
        this.item = item; // Apenas armazenamos um item dentro do item que esta no escopo da função

        // 1. Turn the item as a child of the item Spot
        item.transform.SetParent(transform); // O item que pegarmos vai se tornar do portador desse script

        item.AssignSpot(this);
    }

    public void Clear()
    {
        item = null;
    }

    public bool IsEmpty() // Vamos fazer um metodo que retorna se o spot está vazio
    { // Vamos verificar se nesse Parent vai conter algum filho com a Class Item "Se tiver um Item não esta vazio"

        if (item != null) // Se não estiver Null (Tem algo), não esta vazio retorna false
            return false; // IsEmpty é falso pois tem coisa dentro
        else
            return true;

    }
}