using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Actions")]
    public static Action<Item> onItemClicked;

    [Header("Settings")]
    [SerializeField] private Material outlineMaterial;
    private Item currentItem;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) // Retorna enquanto o Botão do Mouse estiver pressionado
            HandleButtonDrag();
        else if (Input.GetMouseButtonUp(0)) // Retorna quando o Botão do Mouse for Solto
            HandleButtonUp();
    }

    private void HandleButtonDrag()
    {
        Ray cameraRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(cameraRayCast, out RaycastHit hit, 99);

        // Checks with Early Return
        if (hit.collider == null)// Caso não encontramos nada no RayCast Apenas Retornamos
        {
            DeselectCurrentItem();
            return;
        }

        if (!hit.collider.TryGetComponent(out Item item)) // Caso o que clicarmos não tiver o Component Item (Retornamos)
        {
            DeselectCurrentItem();
            return;
        }

        //----

        // Caso Passou nos Checks que não é Nulo e é um Item Chamamos tudo que esta abaixo disso :)
        DeselectCurrentItem(); // Antes de Selecionar o Novo Item "Deselecionamos o Item Atual" 
        currentItem = item;
        currentItem.Select(outlineMaterial); // Selecionamos o Item (Passamos o Material de Contorno)
    }

    private void HandleButtonUp()
    {
        // Verify if we have a current Item
        if (currentItem == null) // Se não tiver nenhum Item selecionado apenas Retornamos
            return;

        // If we have an Item -> Invoke the Action
        currentItem.Deselect(); // Deselecionamos o Item Atual
        onItemClicked?.Invoke(currentItem); // Se tiver o componente "Item" -> Passamos ele como parametro ....
        currentItem = null;
    }

    private void DeselectCurrentItem()
    {
        if (currentItem != null) // Se tiver algum Item selecionado
            currentItem.Deselect(); // Deselecionamos o Item Atual

        currentItem = null; // Limpamos o Item Atual
    }
}
