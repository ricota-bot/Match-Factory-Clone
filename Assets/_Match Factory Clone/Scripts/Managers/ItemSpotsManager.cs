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

        if (!IsFreeSpotAvaible()) // Checa se n�o tem spot disponivel
        {
            Debug.LogWarning("All Items Spot is Occupied.. Probaly gameover ma boii :(");
            return;
        }

        // Caso passe nas verifica��es anteriores vamos fazer os codigos abaixos
        isBusy = true; // Agora voc� esta ocupado, movendo o item para o primeiro FreeSpot Disponivel
        HandleItemClicked(item);
    }

    private void HandleItemClicked(Item item) // O que fazer quando clicarmos no Item ?
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName)) // Caso encontrar alguma Key parecida dentro do dicionario
        {// Temos um item parecido "Com o mesmo nome"
            HandleItemMergeDataFound(item);
        }
        else // N�o tem outro com o mesmo nome, ent�o Adicionamos como primeira vez
            MoveItemToFirstAvaibleSpot(item);

    }

    private void HandleItemMergeDataFound(Item item) // Encontramos um Objeto similar dentro desse Dictionary
    {
        Debug.Log("Encontramos um similar dentro do Dictionary .... aeeeee...");

        ItemSpot idealSpot = GetIdealSpotFor(item);

        itemMergeDataDictionary[item.ItemName].Add(item); // Temos uma Lista dentro desse Dictionary, estamos adicionando esse item a essa lista

        TryMoveItemToIdealSpot(item, idealSpot);

    }

    private ItemSpot GetIdealSpotFor(Item item)
    {

        List<Item> items = itemMergeDataDictionary[item.ItemName].items;  // Vamos pegar o Item que esta armazenado dentro do Dicionario
        List<ItemSpot> spots = new List<ItemSpot>(); // Criamos uma Lista nova dos Spots dos Itens "Posi��es que eles foram colocados"

        for (int i = 0; i < items.Count; i++) // Loop por todos os items da List "Vamos pegar os Seus respectivos Spots"
        {
            spots.Add(items[i].Spot); // Adicionamos a posi��o que o item esta dentro da Lista de ItemSpots
        }

        // Temos uma Lista com todas as posi��es que o item esta ocupando "Lembrando seus similares"

        // Se voc� tem apenas um Spot o Spot ideal o o seu mais a direita
        if (spots.Count >= 2)
            spots.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));

        int idealSpotIndex = spots[0].transform.GetSiblingIndex() + 1;

        return itemSpots[idealSpotIndex];

    }

    private void TryMoveItemToIdealSpot(Item item, ItemSpot idealSpot)
    {
        if (!idealSpot.IsEmpty()) // Verificamos se tem algo dentro.. Se estiver com algum objeto faz "abaixo" "Queremos o FALSE"
        {
            HandleIdealSpotFull(item, idealSpot);
            return;
        }
        // Spot esta vazio, ent�o vamos mover esse item para l�
        MoveItemToSpot(item, idealSpot);
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot) // Vamos verificar quando o Spot ideal esta ocupado
    {
        // EX: Quando adicionamos algum objeto semelhante, depois vamos adicionar outro, temos que mover os diferentes para direita
        // Movendo os diferentes para direita liberamos o espa�o para colocar os semelhantes lado a lado :)
        MoveAllItemToTheRightFrom(idealSpot, item);
    }

    private void MoveAllItemToTheRightFrom(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex();

        for (int i = itemSpots.Length - 2; i >= spotIndex; i--) // LOOP come�ando por spots - 2 ("PENULTIMO SPOTS") at� a posi��o ideal
        {
            ItemSpot spot = itemSpots[i]; // Apenas guardamos um referencia desse Spot no index desejado
            if (spot.IsEmpty()) // Se estiver vazio DOUBLE CHECK
                continue;

            // O Item do index SpotIndex vai se mover para o spotIndex + 1 (("MOVER UM A DIREITA"))
            Item item = spot.Item; // Guardamos o Item que esta nessa posi��o "i"

            spot.Clear();

            ItemSpot targetSpot = itemSpots[i + 1];

            if (!targetSpot.IsEmpty())// CASO J� TENHA ALGUMA COISA NESSE SPOT
            {
                Debug.LogError("ERROR: this should not happen!");
                isBusy = false;
                return;
            }
            // CASO O TARGET SPOT IS EMPTY "OQUE � PRA SER"
            MoveItemToSpot(item, targetSpot, false);
        }

        MoveItemToSpot(itemToPlace, idealSpot);
    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot, bool checkForMerge = true)
    {
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

        HandleItemReachedSpot(item, checkForMerge);
    }

    private void MoveItemToFirstAvaibleSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("N�o tem mais posi��es Disponivel");
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

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {
        if (!checkForMerge)
            return;

        if (itemMergeDataDictionary[item.ItemName].CanMergeItems()) // Caso tem a Lista de Items for maior ou igual a 3
        {
            MergeItems(itemMergeDataDictionary[item.ItemName]);
        }
        else
            CheckForGameOver();
    }

    private void HandleFirstItemReachedSpot(Item item1)
    {
        CheckForGameOver();
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;

        // Removemos o Dicionario que contem esse item com o nome e etc..
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        // Vamos limpar os Items de sua posi��o
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Spot.Clear();
            Destroy(items[i].gameObject);
        }

        isBusy = false;
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot() == null)
            Gameover();
        else
            isBusy = false;
    }

    // FUNCTIONAL FUNCTIONS
    private void Gameover() => Debug.LogWarning("Game Overrr !!!");
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
            if (itemSpots[i].IsEmpty()) // Caso Esteja vazio esta Disponivel "N�o tem Classe Item dentro dele"
            {
                return itemSpots[i]; // Retornamos a posi��o desse spot, pois vamos usa-la
            }
        }

        return null; // Caso retornar null � porque n�o temos mais Spots
    }
    private bool IsFreeSpotAvaible() // Temos spots Livres ?
    {
        for (int i = 0; i < itemSpots.Length; i++) // Percorremos todos os Spots no Array criado antes e Armazenado 
        {
            if (itemSpots[i].IsEmpty()) // Se o Spot estiver vazio
                return true; // Retornamos true, pois tem um spot disponivel
        }
        return false; // N�o tem mais posi��es Disponiveis
    }

}
