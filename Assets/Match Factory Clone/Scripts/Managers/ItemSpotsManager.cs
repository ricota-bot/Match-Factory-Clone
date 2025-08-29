using System;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpot;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    private void Awake()
    {
        InputManager.onItemClicked += OnItemClickedCallBack;
    }

    private void OnDestroy()
    {
        InputManager.onItemClicked -= OnItemClickedCallBack;

    }


    private void OnItemClickedCallBack(Item item)
    {
        // 1. Turn the item as a child of the item Spot
        item.transform.SetParent(itemSpot);

        // 2. Scale the Item down
        item.transform.localScale = itemLocalScaleOnSpot;

        // 3. Set your local position to (0,0,0)
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localRotation = Quaternion.Euler(0, 0, 0);

        // 4. Disable the Shadow
        item.DisableShadows();

        // 5. Disable Item Collider
        item.DisablePhysics();
    }
}
