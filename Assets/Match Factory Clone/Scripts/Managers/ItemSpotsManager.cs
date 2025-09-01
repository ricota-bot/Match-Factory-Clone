using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] itemSpots;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private bool isBusy;

    [Header("Data")]
    private Dictionary<ItemNameEnum, ItemMergeData> itemMergeDataDictionary = new Dictionary<ItemNameEnum, ItemMergeData>();

    private void Awake()
    {
        // ACTIONS
        InputManager.onItemClicked += OnItemClickedCallBack;

        // METHODS
        StoreSpots();
    }

    private void OnDestroy()
    {
        // ACTIONS
        InputManager.onItemClicked -= OnItemClickedCallBack;
    }

    private void OnItemClickedCallBack(Item item)
    {
        if (isBusy)
        {
            Debug.LogWarning("ItemSpotManager is Busy... do anything!");
            return;
        }

        if (!IsFreeSpotAvaible()) // Checa se não tem spot disponivel
        {
            Debug.LogWarning("All Items Spot is Occupied.. Probaly gameover ma boii :(");
            return;
        }

        // Caso passe nas verificações anteriores vamos fazer os codigos abaixos
        isBusy = true; // Agora você esta ocupado, movendo o item para o primeiro FreeSpot Disponivel
        HandleItemClicked(item);
    }

    private void HandleItemClicked(Item item) // O que fazer quando clicarmos no Item ?
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName)) // Caso encontrar alguma Key parecida dentro do dicionario
        {// Temos um item parecido "Com o mesmo nome"
            HandleItemMergeDataFound(item);
        }
        else // Não tem outro com o mesmo nome, então Adicionamos como primeira vez
            MoveItemToFirstAvaibleSpot(item);

    }

    private void HandleItemMergeDataFound(Item item)
    {
        Debug.Log("Encontramos um similar dentro do Dictionary .... aeeeee...");
    }

    private void MoveItemToFirstAvaibleSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("Não tem mais posições Disponivel");
            return;
        }

        CreateItemMergeData(item);

        targetSpot.Populate(item);

        // 2. Scale the Item down
        item.transform.localScale = itemLocalScaleOnSpot;

        // 3. Set your local position to (0,0,0)
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localRotation = Quaternion.identity;

        // 4. Disable the Shadow
        item.DisableShadows();

        // 5. Disable Item Collider
        item.DisablePhysics();

        HandleFirstItemReachedSpot(item);
    }

    private void HandleFirstItemReachedSpot(Item item1)
    {
        CheckForGameOver();
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot() == null)
            Gameover();
        else
            isBusy = false;
    }
    private void Gameover() => Debug.LogWarning("Game Overrr !!!");

    // FUNCTIONAL FUNCTIONS
    private void StoreSpots()
    {
        itemSpots = new ItemSpot[itemSpotsParent.childCount]; // Inicializamos a Array com o tamanho do numero de Spots

        for (int i = 0; i < itemSpotsParent.childCount; i++) // For por todos os Spots
        {
            itemSpots[i] = itemSpotsParent.GetChild(i).GetComponent<ItemSpot>(); // Armazenamos o Spot na Array

        }
    }
    private ItemSpot GetFreeSpot() // GameObjects
    {
        for (int i = 0; i < itemSpots.Length; i++) // Loop por todos os Spots
        {
            if (itemSpots[i].IsEmpty()) // Caso Esteja vazio esta Disponivel "Não tem Classe Item dentro dele"
            {
                return itemSpots[i]; // Retornamos a posição desse spot, pois vamos usa-la
            }
        }

        return null; // Caso retornar null é porque não temos mais Spots
    }
    private bool IsFreeSpotAvaible() // Temos spots Livres ?
    {
        for (int i = 0; i < itemSpots.Length; i++) // Percorremos todos os Spots no Array criado antes e Armazenado 
        {
            if (itemSpots[i].IsEmpty()) // Se o Spot estiver vazio
                return true; // Retornamos true, pois tem um spot disponivel
        }
        return false; // Não tem mais posições Disponiveis
    }

}
